
using CustomerSuccessCRM.Lib.Configuration;
using Microsoft.Extensions.Options;
using System.IO;
using System.Security.Cryptography;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class StorageService : IStorageService
    {
        private readonly StorageSettings _settings;
        private readonly string _basePath;

        public StorageService(IOptions<StorageSettings> settings)
        {
            _settings = settings.Value;
            _basePath = _settings.BasePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
            Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveFileAsync(string folder, string fileName, string contentType, byte[] content)
        {
            var folderPath = Path.Combine(_basePath, folder);
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            await File.WriteAllBytesAsync(filePath, content);

            // Salvar metadados do arquivo
            var metadataPath = filePath + ".metadata";
            var metadata = new
            {
                ContentType = contentType,
                Size = content.Length,
                CreatedAt = DateTime.UtcNow,
                Hash = await GetFileHashAsync(filePath)
            };

            await File.WriteAllTextAsync(metadataPath, System.Text.Json.JsonSerializer.Serialize(metadata));

            return filePath;
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Arquivo não encontrado", filePath);

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<bool> FileExistsAsync(string filePath)
        {
            return await Task.FromResult(File.Exists(filePath));
        }

        public async Task<long> GetFileSizeAsync(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return await Task.FromResult(fileInfo.Length);
        }

        public async Task<DateTime> GetFileLastModifiedAsync(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return await Task.FromResult(fileInfo.LastWriteTimeUtc);
        }

        public async Task<IEnumerable<string>> ListFilesAsync(string folder, string? searchPattern = null)
        {
            var folderPath = Path.Combine(_basePath, folder);
            if (!Directory.Exists(folderPath))
                return new List<string>();

            searchPattern ??= "*";
            var files = Directory.GetFiles(folderPath, searchPattern, SearchOption.AllDirectories);
            return await Task.FromResult(files.ToList());
        }

        public async Task<string> GetFileHashAsync(string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = await md5.ComputeHashAsync(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public bool FileExists(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);
            return File.Exists(filePath);
        }

        public string GetFileUrl(string fileName)
        {
            // Aqui você pode implementar a lógica para gerar URLs públicas
            // Por exemplo, se estiver usando armazenamento em nuvem ou um servidor web
            return $"/storage/{fileName}";
        }
    }
} 