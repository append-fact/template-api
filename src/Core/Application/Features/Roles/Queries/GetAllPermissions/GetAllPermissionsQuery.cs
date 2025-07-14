using Application.Common.Wrappers;
using MediatR;
using System;

namespace Application.Features.Roles.Queries.GetAllPermissions
{
    public class GetAllPermissionsQuery : IRequest<Response<List<GetAllPermissionsResponse>>>
    {
    }

}
