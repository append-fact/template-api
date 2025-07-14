using Application.Common.Storage;

namespace Application.Common.Interfaces
{
    public interface IStorageService
    {
        public Task<Uri> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;

        public void Remove(Uri? path);
    }
}
