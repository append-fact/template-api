using Application.Common.Exceptions;
using Application.Common.RazorModels;
using Application.DTOs.Common;
using Application.Features.Authenticate.Commands.ForgotPasswordCommand;
using Application.Features.Authenticate.Commands.ResetPasswordCommand;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Services
{
    internal sealed partial class AccountService
    {
        public async Task ForgotPasswordAsync(ForgotPasswordCommand request, string origin, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InvalidOperationException("El email no puede estar vacio");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //var registrationUrl = _preRegistrationJWTSettings.RegistrationUrl;
            //var fullUrl = $"{registrationUrl}/reset?token={token}&email={request.Email}";

            var registrationUrl = await GenerateForgotPasswordUrlAsync(user.Id, user.Email!);

            var emailModel = new ForgotPasswordEmailModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                RegistrationUrl = registrationUrl
            };

            var emailBody = await _templateRenderer.RenderTemplateAsync("Emails.ForgotPasswordMail.cshtml", emailModel);

            var emailRequest = new EmailDTO
            {
                To = user.Email,
                Subject = "Instrucciones de recuperación de contraseña",
                Body = emailBody,
                From = "jpaez@append.com.ar"
            };

            await _emailService.SendEmailAsync(emailRequest, cancellationToken);

            //var mailRequest = new MailRequest(
            //    new Collection<string> { user.Email },
            //    "Reset Password",
            //    $"Please reset your password using the following link: {resetPasswordUri}");

            //jobService.Enqueue(() => mailService.SendAsync(mailRequest, CancellationToken.None));
        }

        public async Task ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ApiException("Error al resetear password", errors);
            }
        }

        public async Task<string> GenerateForgotPasswordUrlAsync(string personId, string personEmail)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("personId", personId.ToString()),
                        new Claim("personEmail", personEmail)
                }),
                Expires = _dateTimeService.Now.AddDays(_jwtSettings.DurationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Usar la URL base definida en la configuración para construir el enlace final
            //var registrationUrl = _preRegistrationJWTSettings.RegistrationUrl;
            var registrationUrl = await GenerateForgotPasswordUrlAsync(personId, personEmail);
            var fullUrl = $"{registrationUrl}/reset?token={tokenString}";

            return await Task.FromResult(fullUrl);
        }
    }
}
