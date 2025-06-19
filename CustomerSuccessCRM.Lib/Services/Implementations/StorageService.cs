using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Configuration;
using Microsoft.Extensions.Options;
using System.IO;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class StorageService : IStorageService
    {
        private readonly StorageSettings _storageSettings;
        private readonly string _basePath;

        public StorageService(IOptions<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings.Value;
            _basePath = Path.GetFullPath(_storageSettings.BasePath);
            
            // Criar diretório base se não existir
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            // Validar tamanho do arquivo
            if (fileStream.Length > _storageSettings.MaxFileSize)
            {
                throw new InvalidOperationException($"Arquivo excede o tamanho máximo permitido de {_storageSettings.MaxFileSize / 1024 / 1024}MB");
            }

            // Validar extensão
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!_storageSettings.AllowedFileTypes.Contains(extension))
            {
                throw new InvalidOperationException($"Tipo de arquivo não permitido. Tipos permitidos: {string.Join(", ", _storageSettings.AllowedFileTypes)}");
            }

            // Gerar nome único
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_basePath, uniqueFileName);

            // Salvar arquivo
            using (var fileWriter = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileWriter);
            }

            return uniqueFileName;
        }

        public async Task<Stream> GetFileAsync(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Arquivo não encontrado", fileName);
            }

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
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