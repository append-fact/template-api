using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Roles.Queries.GetAllPermissions
{

    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, Response<List<GetAllPermissionsResponse>>>
    {
        private readonly IRolesService _rolesService;

        public GetAllPermissionsQueryHandler(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<Response<List<GetAllPermissionsResponse>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            return await _rolesService.GetAllPermissionsAsync();
        }
    }

}

