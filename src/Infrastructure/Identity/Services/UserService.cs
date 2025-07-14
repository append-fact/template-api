using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Parameters;
using Application.Common.Storage;
using Application.Common.Wrappers;
using Application.Features.Users.Commands.ChangePasswordCommand;
using Application.Features.Users.Commands.UpdateUserCommand;
using Application.Features.Users.Queries.GetAllUsersQuery;
using Application.Features.Users.Queries.GetUserProfileQuery;
using Identity.Context;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Identity.Services
{
    internal sealed partial class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStorageService _storageService;
        private readonly IdentityContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CurrentUser _user;

        public UserService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IStorageService storageService,
            ICurrentUserService currentUserService,
            IdentityContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _storageService = storageService;
            _context = context;
            _signInManager = signInManager;
            _user = currentUserService.User;
        }

        public async Task<Response<GetUserProfileResponse>> GetUser(string? userId, CancellationToken cancellationToken)
        {
            // Si viene con Id es porque no es el usuario actual
            var user = await _userManager.FindByIdAsync(userId ?? _user.Id.ToString());

            if (user == null)
            {
                Console.WriteLine("Usuario no autenticado.");
                throw new ApiException("Usuario no autenticado", (int)HttpStatusCode.Unauthorized);
            }

            // Generamos el JWT para el usuario
            //JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);

            // Creamos la respuesta con los datos del usuario
            var response = new GetUserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Apellido = user.LastName,
                Nombre = user.FirstName,
                UrlImage = user.UrlImage,
                IsVerified = user.EmailConfirmed,
                IsActive = user.IsActive,
                AreaNumber = user.AreaNumber,
                PhoneNumber = user.PhoneNumber,
            };

            // Obtener roles del usuario
            var rolesList = await _userManager.GetRolesAsync(user);
            response.Roles = rolesList.ToList();

            // Obtener claims del usuario (los permisos asignados directamente)
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Asignar los claims al response (si no hay, se inicializa como lista vacía)
            //response.Claims = userClaims?
            //    .Select(c => new ClaimDTO { Type = c.Type, Value = c.Value })
            //    .ToList() ?? new List<ClaimDTO>();

            // Filtrar y agregar los claims de tipo "Permission" que están asignados al usuario
            var userPermissions = userClaims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            // Obtener todos los RoleIds del usuario
            var roleIds = await _userManager.GetRolesAsync(user);
            var roleClaimsList = new List<Claim>();

            foreach (var roleName in roleIds)
            {
                var role = await _roleManager.FindByNameAsync(roleName);  // Obtener el rol por nombre
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);  // Obtener los claims de ese rol
                    roleClaimsList.AddRange(roleClaims.Where(c => c.Type == "Permission"));  // Filtrar solo los permisos
                }
            }

            // Agregar los permisos de los roles a la lista de permisos del usuario, evitando duplicados
            var allPermissions = userPermissions.Union(roleClaimsList.Select(c => c.Value)).Distinct().ToList();

            // Asignar permisos al response
            //response.Permissions = allPermissions;

            var lastAccess = await _context.RefreshTokens
                .Where(r => r.UserId == user.Id && r.Revoked == null && r.Expires > DateTime.Now)
                .OrderByDescending(r => r.Created)
                .Select(r => r.Created)
                .FirstOrDefaultAsync(cancellationToken);

            response.LastAccess = lastAccess;

            return new Response<GetUserProfileResponse>(response);
        }

        public async Task<PagedResponse<List<GetAllUsersResponse>>> GetAllUsersAsync(GetAllUsersQuery parameters)
        {

            var query = _userManager.Users.AsQueryable();


            if (parameters.Id.HasValue && parameters.Id != Guid.Empty)
                query = query.Where(u => u.Id.Equals(parameters.Id));

            if (!string.IsNullOrEmpty(parameters.FirstName))
                query = query.Where(u => u.FirstName.Contains(parameters.FirstName));

            if (!string.IsNullOrEmpty(parameters.LastName))
                query = query.Where(u => u.LastName.Contains(parameters.LastName));

            if (!string.IsNullOrEmpty(parameters.UserName))
                query = query.Where(u => u.UserName.Contains(parameters.UserName));

            if (!string.IsNullOrEmpty(parameters.Email))
                query = query.Where(u => u.Email.Contains(parameters.Email));

            if (!string.IsNullOrEmpty(parameters.PhoneNumber))
                query = query.Where(u => u.PhoneNumber.Contains(parameters.PhoneNumber));


            var totalCount = await query.CountAsync();


            // Aplica ordenamiento
            if (!string.IsNullOrEmpty(parameters.SortColumn))
                query = query.ApplyOrdering(parameters.SortColumn, parameters.SortOrder);
            else
                query = query.OrderBy(x => x.UserName);

            var users = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();


            var usersResponse = new List<GetAllUsersResponse>();

            foreach (var user in users)
            {
                usersResponse.Add(new GetAllUsersResponse
                {
                    Id = Guid.Parse(user.Id),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email
                });
            }


            return new PagedResponse<List<GetAllUsersResponse>>(usersResponse, parameters.PageNumber, parameters.PageSize, totalCount);
        }

        public async Task UpdateUserAsync(UpdateUserCommand request, Guid? userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString() ?? "");

            _ = user ?? throw new ApiException("Usuario no encontrado");

            Uri imageUri = user.UrlImage ?? null!;
            if (request.Image != null || request.DeleteCurrentImage)
            {
                user.UrlImage = await _storageService.UploadAsync<ApplicationUser>(request.Image, FileType.Image);
                if (request.DeleteCurrentImage && imageUri != null)
                {
                    _storageService.Remove(imageUri);
                }
            }

            user.FirstName = request.Nombre;
            user.LastName = request.Apellido;
            user.PhoneNumber = request.PhoneNumber;
            user.AreaNumber = request.AreaNumber;
            user.UserName = request.UserName;
            user.Email = request.Email;

            string? phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (request.PhoneNumber != phoneNumber)
            {
                await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
            }

            string? userName = await _userManager.GetUserNameAsync(user);
            if (request.UserName != userName)
            {
                await _userManager.SetUserNameAsync(user, request.UserName);
            }

            string? email = await _userManager.GetEmailAsync(user);
            if (request.Email != email)
            {
                await _userManager.SetEmailAsync(user, request.Email);
            }

            var result = await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            if (!result.Succeeded)
            {
                throw new ApiException("Falló la actualizacion del usuario");
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            _ = user ?? throw new NotFoundException("Usuario no encontrado.");

            user.IsActive = false;
            IdentityResult? result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                List<string> errors = result.Errors.Select(error => error.Description).ToList();
                throw new ApiException("Error al eliminar", errors);
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordCommand request)
        {
            var user = await _userManager.FindByIdAsync(_user.Id);

            _ = user ?? throw new NotFoundException("Usuario no encontrado");

            var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ApiException("Error al cambiar la password", errors);
            }
        }

        public async Task<Response<string>> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException("Usuario no encontrado.");

            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
                throw new NotFoundException("Rol no encontrado.");

            // Validación: solo superadmin puede asignar rol superadmin
           /* if (role.Name == "Superadmin" && !_user.Roles.Contains("Superadmin"))
                throw new UnauthorizedException("No tiene permisos para asignar el rol Superadmin.");*/

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
                throw new ApiException("No se pudo asignar el rol.", result.Errors.Select(e => e.Description).ToList());

            // Log de auditoría, si es necesario
            // await _auditService.LogAsync(_user.Id, $"Asignó el rol '{role.Name}' al usuario {user.Email}");

            return new Response<string>($"Rol '{role.Name}' asignado al usuario '{user.Email}' correctamente.");
        }

        public async Task<Response<string>> AssignPermissionsToUserAsync(Guid userId, List<string> permissionNames)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException("Usuario no encontrado.");

            var definedPermissions = Permissions.All; // Obtiene todos los permisos definidos.

            foreach (var permissionName in permissionNames)
            {
                // Validamos el permiso usando PermissionValidator
                string validPermissionName = PermissionValidator.GetValidPermissionName(permissionName, definedPermissions.ToList());

                // Verificar si el permiso ya está asignado
                var existingClaims = await _userManager.GetClaimsAsync(user);
                if (existingClaims.Any(c => c.Type == Claims.Permission && c.Value == validPermissionName))
                    continue;

                // Asignar el permiso al usuario
                var result = await _userManager.AddClaimAsync(user, new Claim(Claims.Permission, validPermissionName));
                if (!result.Succeeded)
                    throw new ApiException("No se pudo asignar el permiso.", result.Errors.Select(e => e.Description).ToList());
            }

            return new Response<string>($"Permisos asignados correctamente al usuario '{user.Email}'.");
        }



        public async Task<Response<string>> RemovePermissionsToUserAsync(Guid userId, List<string> permissionNames)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException("Usuario no encontrado.");

            var definedPermissions = Permissions.All;

            foreach (var permissionName in permissionNames)
            {
                // Validamos el permiso usando PermissionValidator
                string validPermissionName = PermissionValidator.GetValidPermissionName(permissionName, definedPermissions.ToList());

                var existingClaims = await _userManager.GetClaimsAsync(user);
                var claimToRemove = existingClaims.FirstOrDefault(c => c.Type == Claims.Permission && c.Value == validPermissionName);

                if (claimToRemove != null)
                {
                    var result = await _userManager.RemoveClaimAsync(user, claimToRemove);
                    if (!result.Succeeded)
                        throw new ApiException("No se pudo remover el permiso.", result.Errors.Select(e => e.Description).ToList());
                }
            }

            return new Response<string>($"Permisos removidos correctamente del usuario '{user.Email}'.");
        }

        public async Task<Response<string>> RemoveRoleToUserAsync(Guid userId, Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException("Usuario no encontrado.");

            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
                throw new NotFoundException("Rol no encontrado.");

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!result.Succeeded)
                throw new ApiException("No se pudo remover el rol.", result.Errors.Select(e => e.Description).ToList());

            return new Response<string>($"Rol '{role.Name}' removido del usuario '{user.UserName}' correctamente.");
        }



    }
}
