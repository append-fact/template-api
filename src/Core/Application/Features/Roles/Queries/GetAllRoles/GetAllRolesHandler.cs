using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Roles.Queries.GetAllRoles
{
    public class GetAllRolesHandler : IRequestHandler<GetAllRoles, Response<List<RolesWithPermissionsResponseDTO>>>
    {
        private readonly IRolesService _rolesService;

        public GetAllRolesHandler(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<Response<List<RolesWithPermissionsResponseDTO>>> Handle(GetAllRoles request, CancellationToken cancellationToken)
        {
            return await _rolesService.GetAllRolesWithPermissionsAsync();
        }
    }
}
