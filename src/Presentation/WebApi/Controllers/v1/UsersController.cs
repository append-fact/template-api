using Application.Common.Wrappers;
using Application.DTOs;
using Application.Features.Users.Commands.AssignPermissionsToUserCommand;
using Application.Features.Users.Commands.AssignRoleToUserCommand;
using Application.Features.Users.Commands.ChangePasswordCommand;
using Application.Features.Users.Commands.DeleteUserCommand;
using Application.Features.Users.Commands.RemovePermissionsToUserCommand;
using Application.Features.Users.Commands.RemoveRoleToUserCommand;
using Application.Features.Users.Commands.UpdateUserCommand;
using Application.Features.Users.Queries.GetAllUsersQuery;
using Application.Features.Users.Queries.GetUserByIdQuery;
using Application.Features.Users.Queries.GetUserProfileQuery;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers.v1
{
    /// <summary>
    /// Controller para gestion de usuarios
    /// </summary>

    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseApiController
    {

        /// <summary>
        /// Obtener informacion del usuario basado en el token
        /// </summary>
        [ProducesResponseType(typeof(Response<UserDTO>), StatusCodes.Status200OK)]
        [HttpGet("Profile")]
        [PermissionAuthorization("Users.View")]
        public async Task<IActionResult> GetUserProfile()
        {
            var response = await Mediator.Send(new GetUserProfileQuery());
            return Ok(response);
        }

        /// <summary>
        /// Devuelve una lista paginada de usuarios
        /// </summary>
        [ProducesResponseType(typeof(Response<List<UserDTO>>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] GetAllUsersQuery request)
        {

            return Ok(await Mediator.Send(request));
        }

        /// <summary>
        /// Obtener informacion del usuario por ID
        /// </summary>
        [ProducesResponseType(typeof(Response<UserDTO>), StatusCodes.Status200OK)]
        [HttpGet("{Id:Guid}")]
        [PermissionAuthorization("Users.View")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid Id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = Id }));
        }

        /// <summary>
        /// Actualiza la informacion del usuario
        /// </summary>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [HttpPut("{Id:Guid}/Profile")]
        [PermissionAuthorization("Users.Update")]
        public async Task<IActionResult> UpdateUserProfile([FromRoute] Guid Id, [FromBody] UpdateUserCommand command)
        {
            command.Id = Id;

            var response = await Mediator.Send(command);
            return Ok(response);
        }

        /// <summary>
        /// Eliminar un usuario
        /// </summary>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [HttpDelete("{Id:Guid}")]
        [PermissionAuthorization("Users.Delete")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] Guid Id)
        {
            return Ok(await Mediator.Send(new DeleteUserCommand { Id = Id}));
        }


        /// <summary>
        /// Modificar password desde perfil
        /// </summary>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [HttpPost("ChangePassword")]
        [PermissionAuthorization("Users.Update")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        #region Roles y Permisos
        /// <summary>
        /// Asignar un rol a un usuario
        /// </summary>
        [HttpPost("{Id:Guid}/Role")] 
        [PermissionAuthorization("UserRoles.Update")]
        public async Task<IActionResult> AssignRoleToUser([FromRoute] Guid Id, [FromBody] AssignRoleToUserCommand command) 
        {
            command.UserId = Id;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Asignar permisos a un usuario
        /// </summary>
        [HttpPost("{Id:Guid}/Permissions")]
        [PermissionAuthorization("UserUpdate")]
        public async Task<IActionResult> AssignPermissionsToUser([FromRoute] Guid Id, [FromBody] AssignPermissionsToUserCommand command) 
        {
            command.UserId = Id;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Remover permisos de un usuario
        /// </summary>
        [HttpDelete("{Id:Guid}/Permissions")]
        [PermissionAuthorization("UserDelete")]
        public async Task<IActionResult> RemovePermissionsToUser([FromRoute] Guid Id, [FromBody] RemovePermissionsToUserCommand command)
        {
            command.UserId = Id;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Revocar un rol a un usuario
        /// </summary>
        [HttpDelete("{Id:Guid}/Role")]
        [PermissionAuthorization("UserRoles.Delete")]
        public async Task<IActionResult> RemoveRoleToUser([FromRoute] Guid Id, [FromBody] RemoveRoleToUserCommand command)
        {
            command.UserId = Id;
            return Ok(await Mediator.Send(command));
        }

        #endregion
    }
}
