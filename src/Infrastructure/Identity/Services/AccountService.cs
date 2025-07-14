using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mailing;
using Application.Common.Wrappers;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using Application.Features.Authenticate.Commands.RegisterCommand;
using Domain.Settings;
using Identity.Common;
using Identity.Context;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Identity.Services
{
    internal sealed partial class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IdentityContext _identityContext;
        private readonly SecuritySettings _securitySettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IEmailTemplateService _templateService;
        private readonly CurrentUser _user;
        private readonly IEmailService _emailService;
        private readonly IRazorRenderer _templateRenderer;

        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JWTSettings> jwtSettings,
            IdentityContext identityContext,
                        IOptions<SecuritySettings> securitySettings,
            IDateTimeService dateTimeService,
            IEmailTemplateService templateService,
            IEmailService emailService,
            IRazorRenderer templateRenderer,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _identityContext = identityContext;
            _user = currentUserService.User;
            _securitySettings = securitySettings.Value;
            _dateTimeService = dateTimeService;
            _templateService = templateService;
            _emailService = emailService;
            _templateRenderer = templateRenderer;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email!);
            if (usuario == null)
            {
                throw new ApiException($"No hay cuenta registrada con el Email {request.Email}", (int)HttpStatusCode.NotFound);
            }



            if (!await _userManager.CheckPasswordAsync(usuario, request!.Password!))
                throw new ApiException("Credenciales inválidas", 401);


            var jwtSecurityToken = await GenerateJwtToken(usuario);
            var response = new AuthenticationResponse
            {
                Id = usuario.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = usuario.Email,
                UserName = usuario.UserName,
                Apellido = usuario.LastName,
                Nombre = usuario.FirstName,
                UrlImage = usuario.UrlImage,
                Roles = (await _userManager.GetRolesAsync(usuario)).ToList(),
                IsVerified = usuario.EmailConfirmed
            };



            List<string> rolClaims = new List<string>();

            foreach (var roleName in response.Roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                    rolClaims.AddRange(
                        (await _roleManager.GetClaimsAsync(role))
                          .Select(c => c.Value.StartsWith("Permissions.")
                                        ? c.Value.Substring("Permissions.".Length)
                                        : c.Value)
                    );
            }

            response.RoleClaims = rolClaims;

            var newRT = await GenerateRefreshToken(ipAddress, usuario.Id);

            response.RefreshToken = newRT.Token;
            response.RefreshTokenExpiration = newRT.Expires;

            return new Response<AuthenticationResponse>(response, $"Usuario {usuario.UserName} autenticado");
        }


        public async Task<Response<string>> RegisterAsync(RegisterCommand request)
        {
            var usuarioConElMismoUserName = await _userManager.FindByNameAsync(request.UserName);
            if (usuarioConElMismoUserName != null)
                throw new ApiException($"El nombre de usuario {request.UserName} ya fue registrado previamente", (int)HttpStatusCode.Conflict);

            var usuarioConElMismoCorreo = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioConElMismoCorreo != null)
                throw new ApiException($"El mail {request.Email} ya fue registrado previamente", (int)HttpStatusCode.Conflict);

            var usuario = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = false, // Ahora no se confirma automáticamente
                PhoneNumberConfirmed = true
            };

            // � Determinar el rol automáticamente
            string role = Roles.Admin.ToString();


            // � Verificar si el rol existe
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new ApiException($"El rol '{role}' no existe.", (int)HttpStatusCode.BadRequest);
            }

            // Crear usuario en la base de datos
            var result = await _userManager.CreateAsync(usuario, request.Password);
            if (!result.Succeeded)
                throw new ApiException($"{result.Errors}");

            await _userManager.AddToRoleAsync(usuario, role);

            // � Verificar si se requiere confirmación de email y enviar el correo
            if (_securitySettings.RequireConfirmedAccount && !string.IsNullOrEmpty(usuario.Email))
            {
                // Generar link de verificación
                string emailVerificationUri = await GetEmailVerificationUriAsync(usuario, request.Origin);
                RegisterUserEmail emailModel = new RegisterUserEmail()
                {
                    Email = usuario.Email,
                    UserName = usuario.UserName,
                    Url = emailVerificationUri
                };

                var mailRequest = new MailRequest(
                    new List<string> { usuario.Email },
                    "Confirmación de Registro",
                    _templateService.GenerateEmailTemplate("email-confirmation", emailModel)
                );

                usuario.AddDomainEvent(new RegisterCommandEvent(mailRequest));

                //_jobService.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
            }

            return new Response<string>(usuario.Id, message: $"Usuario {usuario.UserName} registrado correctamente. Por favor revisa tu correo ({usuario.Email}) para verificar tu cuenta.");
        }
       
        public async Task<string> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken)
        {

            var user = await _userManager.Users
                .Where(u => u.Id == userId && !u.EmailConfirmed)
                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new ApiException("Ocurrió un error mientras se confirmaba el E-Mail.");

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded
                ? string.Format("Cuenta confirmada para E-Mail {0}..", user.Email)
                : throw new ApiException(string.Format("Ocurrió un error mientras se confirmaba el E-Mail {0}", user.Email));
        }

        private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
        {
            //EnsureValidTenant();

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            const string route = "api/users/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
            //verificationUri = QueryHelpers.AddQueryString(verificationUri, MultitenancyConstants.TenantIdName, _currentTenant.Id!);
            return verificationUri;
        }


    }
}
