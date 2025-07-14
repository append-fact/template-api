using Application.Common.Storage;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : IRequest<Response<string>>
    {
        public Guid? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AreaNumber { get; set; }
        public FileUploadRequest? Image { get; set; }
        public bool DeleteCurrentImage { get; set; }

    }
}
