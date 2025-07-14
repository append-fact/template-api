using Application.Common.Wrappers;
using Application.Features.Users.Commands.ChangePasswordCommand;
using Application.Features.Users.Commands.UpdateUserCommand;
using Application.Features.Users.Queries.GetAllUsersQuery;
using Application.Features.Users.Queries.GetUserProfileQuery;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        
        Task<PagedResponse<List<GetAllUsersResponse>>> GetAllUsersAsync(GetAllUsersQuery parameters);
        Task<Response<GetUserProfileResponse>> GetUser(string? userId, CancellationToken cancellationToken);
        Task UpdateUserAsync(UpdateUserCommand request, Guid? userId);
        Task DeleteUserAsync(string userId);
        Task ChangePasswordAsync(ChangePasswordCommand request);
        Task<Response<string>> AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task<Response<string>> AssignPermissionsToUserAsync(Guid userId, List<string> permissionValues);
        Task<Response<string>> RemovePermissionsToUserAsync(Guid userId, List<string> permissionValues);
        Task<Response<string>> RemoveRoleToUserAsync(Guid userId, Guid roleId);

    }
}
