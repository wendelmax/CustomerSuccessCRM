namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(string diretorio, string subdiretorio, string nomeArquivo, byte[] conteudo);
        Task<byte[]> GetFileAsync(string caminhoArquivo);
        Task DeleteFileAsync(string caminhoArquivo);
        Task<bool> FileExistsAsync(string caminhoArquivo);
        Task<long> GetFileSizeAsync(string caminhoArquivo);
        Task<DateTime> GetFileLastModifiedAsync(string caminhoArquivo);
        Task<IEnumerable<string>> ListFilesAsync(string diretorio, string? padrao = null);
        Task<string> GetFileHashAsync(string caminhoArquivo);
    }
} 