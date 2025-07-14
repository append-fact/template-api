using Application.Common.Exceptions;
using Application.Common.Wrappers;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using Application.Features.Authenticate.Commands.ConfirmEmailCommand;
using Application.Features.Authenticate.Commands.ForgotPasswordCommand;
using Application.Features.Authenticate.Commands.RefreshTokenCommand;
using Application.Features.Authenticate.Commands.RegisterCommand;
using Application.Features.Authenticate.Commands.ResetPasswordCommand;
using Application.Features.Authenticate.Commands.RevokeRefreshTokenCommand;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers.v1
{
    /// <summary>
    /// Controller para gestion de cuenta
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class AccountController : BaseApiController
    {
        /// <summary>
        /// Logeo del usuario
        /// </summary>
        [ProducesResponseType(typeof(Response<AuthenticationResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            var result = await Mediator.Send(new AuthenticateCommand
            {
                Email = request.Email,
                Password = request.Password,
                IpAddress = GenerateIpAddress()
            });
            SetRefreshTokenInCookie(result.Data.RefreshToken, (DateTime)result.Data.RefreshTokenExpiration);
            return Ok(result);
        }

        /// <summary>
        /// Registro de usuario
        /// </summary>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterCommand command)
        {
            command.Origin = GetOriginFromRequest();
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Confirmar usuario mediante el email
        /// </summary>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailUserAsync([FromQuery] ConfirmEmailCommand request)
        {

            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Recuperar password
        /// </summary>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Resetear password
        /// </summary>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Genera un refresh para el usuario logeado
        /// </summary>
        [AllowAnonymous]
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await Mediator.Send(new RefreshTokenCommand
            {
                RefreshToken = RecuperarRefreshToken(),
                IpAddress = GenerateIpAddress()
            });
            SetRefreshTokenInCookie(result.Data.RefreshToken, (DateTime)result.Data.RefreshTokenExpiration);
            return Ok(result);
        }

        /// <summary>
        /// Deslogea al usuario
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [Route("Revoke-token")]
        public async Task<IActionResult> RevokeToken()
        {
            var result = await Mediator.Send(new RevokeRefreshtokenCommand
            {
                RefreshToken = RecuperarRefreshToken(),
                IpAddress = GenerateIpAddress()
            });
            return Ok(result);
        }


        
        private string GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"]!;
            else
                return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expiration)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = expiration,
                HttpOnly = true,
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.Lax
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        private string RecuperarRefreshToken()
        {
            //recupero el refresh token de la cookie
            string cookiesHeader = Request.Headers["Cookie"];
            Request.Cookies.TryGetValue("refreshToken", out string refreshToken);

            if (refreshToken == null)
                throw new ApiException("Token no encontrado");

            //refreshToken = request ?? refreshToken;
            return refreshToken;
        }

        private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
    }
}