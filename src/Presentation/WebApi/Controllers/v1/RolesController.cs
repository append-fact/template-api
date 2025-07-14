using Application.Features.Roles.Commands.CreateRoleCommand;
using Application.Features.Roles.Commands.UpdateRoleCommand;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Roles.Queries.GetAllRoles;
using Application.Features.Roles.Commands.DeleteRoleCommand;
using Application.Features.Roles.Queries.GetAllPermissions;
using Application.Features.Roles.Commands.AssignPermissionsToRoleCommand;
using Application.Features.Roles.Commands.RemovePermissionsToRoleCommand;
using WebApi.Filters;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class RolesController : BaseApiController
    {

        /// <summary>
        /// Crear un nuevo rol
        /// </summary>
        [HttpPost]
        [PermissionAuthorization("Roles.Create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)//controller role
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Obtener todos los roles
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()//controller role
        {
            return Ok(await Mediator.Send(new GetAllRoles()));
        }


        /// <summary>
        /// Actualizar un rol existente
        /// </summary>
        [HttpPut("{Id:Guid}")]
        [PermissionAuthorization("Roles.Update")]
        public async Task<IActionResult> UpdateRole([FromRoute] Guid Id, [FromBody] UpdateRoleCommand command)//controller role
        {
            command.RoleId = Id;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Eliminar un rol existente
        /// </summary>
        [HttpDelete("{Id:Guid}")]
        [PermissionAuthorization("Roles.Delete")]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid Id)
        {
            return Ok(await Mediator.Send(new DeleteRoleCommand { RoleId = Id}));
        }

        /// <summary>
        /// Obtener todos los permisos
        /// </summary>
        [HttpGet("Permissions")]
        [PermissionAuthorization("RoleClaims.View")]
        public async Task<IActionResult> GetAllPermissions()
        {
            return Ok(await Mediator.Send(new GetAllPermissionsQuery()));
        }


        /// <summary>
        /// Asignar permisos a un rol
        /// </summary>
        [HttpPost("{Id:Guid}/Permissions")]
        [PermissionAuthorization("RoleClaims.Update")]
        public async Task<IActionResult> AssignPermissionsToRole([FromRoute] Guid Id, [FromBody] AssignPermissionsToRoleCommand command) //controller role
        {
            command.RoleId = Id;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Remover permisos de un rol
        /// </summary>
        [HttpDelete("{Id:Guid}/Permissions")]
        [PermissionAuthorization("RoleClaims.Delete")]
        public async Task<IActionResult> RemovePermissionToRole([FromRoute] Guid Id, [FromBody] RemovePermissionsToRoleCommand command) //controller role
        {
            command.RoleId = Id;
            return Ok(await Mediator.Send(command));
        }
    }
}
