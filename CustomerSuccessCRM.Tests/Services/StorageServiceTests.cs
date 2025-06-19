using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using CustomerSuccessCRM.Lib.Services.Implementations;
using CustomerSuccessCRM.Lib.Configuration;
using System.Text;

namespace CustomerSuccessCRM.Tests.Services
{
    public class StorageServiceTests : IDisposable
    {
        private readonly string _testBasePath;
        private readonly StorageSettings _settings;
        private readonly StorageService _service;

        public StorageServiceTests()
        {
            _testBasePath = Path.Combine(Path.GetTempPath(), "CrmTestStorage");
            _settings = new StorageSettings
            {
                BasePath = _testBasePath,
                MaxFileSize = 5 * 1024 * 1024, // 5MB
                AllowedFileTypes = new[] { ".txt", ".pdf", ".doc" }
            };

            var mockOptions = new Mock<IOptions<StorageSettings>>();
            mockOptions.Setup(x => x.Value).Returns(_settings);

            _service = new StorageService(mockOptions.Object);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testBasePath))
            {
                Directory.Delete(_testBasePath, true);
            }
        }

        [Fact]
        public async Task SaveFileAsync_ValidFile_ReturnsFileName()
        {
            // Arrange
            var content = "Test content";
            var fileName = "test.txt";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            // Act
            var result = await _service.SaveFileAsync(stream, fileName);

            // Assert
            Assert.NotNull(result);
            Assert.EndsWith(".txt", result);
            Assert.True(File.Exists(Path.Combine(_testBasePath, result)));
        }

        [Fact]
        public async Task SaveFileAsync_InvalidExtension_ThrowsException()
        {
            // Arrange
            var content = "Test content";
            var fileName = "test.invalid";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveFileAsync(stream, fileName));
        }

        [Fact]
        public async Task SaveFileAsync_FileTooLarge_ThrowsException()
        {
            // Arrange
            var content = new byte[6 * 1024 * 1024]; // 6MB
            var fileName = "test.txt";
            using var stream = new MemoryStream(content);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.SaveFileAsync(stream, fileName));
        }

        [Fact]
        public async Task GetFileAsync_ExistingFile_ReturnsStream()
        {
            // Arrange
            var content = "Test content";
            var fileName = "test.txt";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var savedFileName = await _service.SaveFileAsync(stream, fileName);

            // Act
            using var result = await _service.GetFileAsync(savedFileName);
            using var reader = new StreamReader(result);
            var retrievedContent = await reader.ReadToEndAsync();

            // Assert
            Assert.Equal(content, retrievedContent);
        }

        [Fact]
        public async Task GetFileAsync_NonExistingFile_ThrowsException()
        {
            // Arrange
            var fileName = "nonexistent.txt";

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(
                () => _service.GetFileAsync(fileName));
        }

        [Fact]
        public async Task DeleteFileAsync_ExistingFile_DeletesFile()
        {
            // Arrange
            var content = "Test content";
            var fileName = "test.txt";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var savedFileName = await _service.SaveFileAsync(stream, fileName);

            // Act
            await _service.DeleteFileAsync(savedFileName);

            // Assert
            Assert.False(File.Exists(Path.Combine(_testBasePath, savedFileName)));
        }

        [Fact]
        public async Task FileExists_ExistingFile_ReturnsTrue()
        {
            // Arrange
            var content = "Test content";
            var fileName = "test.txt";
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var savedFileName = await _service.SaveFileAsync(stream, fileName);

            // Act
            var result = _service.FileExists(savedFileName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetFileUrl_ReturnsExpectedUrl()
        {
            // Arrange
            var fileName = "test.txt";

            // Act
            var result = _service.GetFileUrl(fileName);

            // Assert
            Assert.Equal($"/storage/{fileName}", result);
        }
    }
} 