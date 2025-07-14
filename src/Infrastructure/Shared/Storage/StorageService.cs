using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Storage;
using Domain.Settings;
using Microsoft.Extensions.Options;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Shared.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IOptions<OriginOptions> _originSettings;
        public StorageService(IOptions<OriginOptions> originSettings)
        {
            this._originSettings = originSettings;
        }

        public async Task<Uri> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
           where T : class
        {
            if (request == null || request.Data == null)
            {
                return null!;
            }

            if (request.Extension is null || !supportedFileType.GetDescriptionList().Contains(request.Extension.ToLower(System.Globalization.CultureInfo.CurrentCulture)))
                throw new InvalidOperationException("Formato no soportado.");
            if (request.Name is null)
                throw new InvalidOperationException("El nombre es requerido.");

            string base64Data = Regex.Match(request.Data, "data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

            var streamData = new MemoryStream(Convert.FromBase64String(base64Data));
            if (streamData.Length > 0)
            {
                string folder = typeof(T).Name;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    folder = folder.Replace(@"\", "/", StringComparison.Ordinal);
                }

                string folderName = supportedFileType switch
                {
                    FileType.Image => Path.Combine("resources", "images", folder),
                    _ => Path.Combine("resources", "others", folder),
                };
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                Directory.CreateDirectory(pathToSave);

                string fileName = request.Name.Trim('"');
                fileName = RemoveSpecialCharacters(fileName);
                fileName = fileName.ReplaceWhitespace("-");
                fileName += request.Extension.Trim();
                string fullPath = Path.Combine(pathToSave, fileName);
                string dbPath = Path.Combine(folderName, fileName);
                if (File.Exists(dbPath))
                {
                    dbPath = NextAvailableFilename(dbPath);
                    fullPath = NextAvailableFilename(fullPath);
                }

                using var stream = new FileStream(fullPath, FileMode.Create);
                await streamData.CopyToAsync(stream, cancellationToken);
                var path = dbPath.Replace("\\", "/", StringComparison.Ordinal);
                var imageUri = new Uri(_originSettings.Value.OriginUrl!, path);
                return imageUri;
            }
            else
            {
                return null!;
            }
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
        }

        public void Remove(Uri? path)
        {
            var pathString = path!.ToString();
            if (File.Exists(pathString))
            {
                File.Delete(pathString);
            }
        }

        private const string NumberPattern = "-{0}";
        

        private static string NextAvailableFilename(string path)
        {
            if (!File.Exists(path))
            {
                return path;
            }

            if (Path.HasExtension(path))
            {
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern));
            }

            return GetNextFilename(path + NumberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string tmp = string.Format(pattern, 1);

            if (!File.Exists(tmp))
            {
                return tmp;
            }

            int min = 1, max = 2;

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                {
                    min = pivot;
                }
                else
                {
                    max = pivot;
                }
            }

            return string.Format(pattern, max);
        }
    }
}
