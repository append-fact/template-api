using Application.Common.Wrappers;
using Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Roles.Queries.GetAllRoles
{
    public class GetAllRoles : IRequest<Response<List<RolesWithPermissionsResponseDTO>>>
    {
    }
}
