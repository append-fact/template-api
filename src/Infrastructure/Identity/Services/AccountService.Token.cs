using Application.Common.Exceptions;
using Application.Common.Wrappers;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Identity.Services
{
    internal sealed partial class AccountService
    {
        public async Task<Response<AuthenticationResponse>> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var oldToken = await _identityContext.RefreshTokens.FirstOrDefaultAsync(q => q.Token == refreshToken);

            // Refresh token no existe, expiró o fue revocado manualmente
            // (Pensando que el usuario puede dar click en "Cerrar Sesión en todos lados" o similar)
            if (oldToken is null || oldToken.Expires <= DateTime.Now)
            {
                throw new ApiException("RefreshToken inactivo", (int)HttpStatusCode.Unauthorized);
            }

            // Se está intentando usar un Refresh Token que ya fue usado anteriormente,
            // puede significar que este refresh token fue robado.
            if (!oldToken.IsActive)
            {
                //_logger.LogWarning("El refresh token del {UserId} ya fue usado. RT={RefreshToken}", refreshToken.UserId, refreshToken.RefreshTokenValue);

                var refreshTokens = await _identityContext.RefreshTokens
                    .Where(q => q.IsActive && q.UserId == oldToken.UserId).ToListAsync();

                foreach (var rt in refreshTokens)
                {
                    rt.Revoked = DateTime.Now;
                }

                await _identityContext.SaveChangesAsync();

                throw new ApiException("Se ha intentado usar un RefreshToken inactivo", (int)HttpStatusCode.Unauthorized);
            }

            // TODO: Podríamos validar que el Access Token sí corresponde al mismo usuario
            oldToken.Revoked = DateTime.Now;

            var user = await _identityContext.Users.FindAsync(oldToken.UserId);

            if (user is null)
            {
                throw new ApiException("El usuario no corresponde a ningun RefreshToken", (int)HttpStatusCode.NotFound);
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            var newRT = await GenerateRefreshToken(ipAddress, user.Id);

            response.RefreshToken = newRT.Token;
            response.RefreshTokenExpiration = newRT.Expires;

            return new Response<AuthenticationResponse>(response, $"Usuario {user.UserName} autenticado");
        }
        public async Task<Response<bool>> RevokeTokenAsync(string refreshToken, string ipAddress)
        {

            var oldToken = await _identityContext.RefreshTokens.FirstOrDefaultAsync(q => q.Token == refreshToken);

            // Refresh token no existe, expiró o fue revocado manualmente
            // (Pensando que el usuario puede dar click en "Cerrar Sesión en todos lados" o similar)
            if (oldToken is null || oldToken.Expires <= DateTime.Now)
            {
                throw new ApiException("RefreshToken inactivo", (int)HttpStatusCode.Unauthorized);
            }

            // Se está intentando usar un Refresh Token que ya fue usado anteriormente,
            // puede significar que este refresh token fue robado.
            if (!oldToken.IsActive)
            {
                //_logger.LogWarning("El refresh token del {UserId} ya fue usado. RT={RefreshToken}", refreshToken.UserId, refreshToken.RefreshTokenValue);

                var refreshTokens = await _identityContext.RefreshTokens
                    .Where(q => q.IsActive && q.UserId == oldToken.UserId).ToListAsync();

                foreach (var rt in refreshTokens)
                {
                    rt.Revoked = DateTime.Now;
                }

                await _identityContext.SaveChangesAsync();

                throw new ApiException("Se ha intentado usar un RefreshToken inactivo", (int)HttpStatusCode.Unauthorized);
            }

            // TODO: Podríamos validar que el Access Token sí corresponde al mismo usuario
            oldToken.Revoked = DateTime.Now;

            var res = await _identityContext.SaveChangesAsync();


            return new Response<bool>(true);

        }

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser usuario)
        {
            var userClaims = await _userManager.GetClaimsAsync(usuario);
            var userRoles = await _userManager.GetRolesAsync(usuario);

            var roleClaims = userRoles
                            .Select(r => new Claim(ClaimTypes.Role, r));


            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var jwtId = Guid.NewGuid().ToString();


            var baseClaims = new[]
                    {
                        // sub = GUID único del usuario
                        new Claim(JwtRegisteredClaimNames.Sub,        usuario.Id!),

                        // unique_name = userName legible
                        new Claim(ClaimTypes.Name, usuario.UserName!),

                        // identificador de token y fecha de emisión
                        new Claim(JwtRegisteredClaimNames.Jti,        jwtId),
                        new Claim(JwtRegisteredClaimNames.Iat,        nowUnix, ClaimValueTypes.Integer64),
                    };

            // Combina todos los claims
            var claims = baseClaims
                .Union(userClaims)
                .Union(roleClaims);


            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecutiryToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signinCredentials
                );

            return jwtSecutiryToken;
        }
        private async Task<RefreshToken> GenerateRefreshToken(string ipAddress, string idUser)
        {

            var newRefreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString("N"),
                Expires = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                Created = DateTime.Now,
                CreatedByIp = ipAddress,
                UserId = idUser
            };

            _identityContext.RefreshTokens.Add(newRefreshToken);

            await _identityContext.SaveChangesAsync();

            return newRefreshToken;
        }

    }
}
