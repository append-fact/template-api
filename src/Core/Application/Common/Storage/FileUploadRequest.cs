using MediatR;

namespace Application.Common.Storage
{
    public class FileUploadRequest : IRequest<FileUploadResponse>
    {
        public string Name { get; set; } = default!;
        public string Extension { get; set; } = default!;
        public string Data { get; set; } = default!;
    }
}
