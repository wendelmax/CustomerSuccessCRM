namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfAsync(string templateName, object model);
        Task<byte[]> GeneratePdfFromHtmlAsync(string html);
        Task<byte[]> MergePdfsAsync(IEnumerable<byte[]> pdfs);
        Task<byte[]> AddWatermarkAsync(byte[] pdf, string watermark);
        Task<byte[]> AddPasswordAsync(byte[] pdf, string password);
        Task<byte[]> AddSignatureAsync(byte[] pdf, byte[] signature, string location);
        Task<int> GetPageCountAsync(byte[] pdf);
        Task<byte[]> ExtractPagesAsync(byte[] pdf, int startPage, int endPage);
        Task<byte[]> CompressPdfAsync(byte[] pdf, PdfCompressionLevel compressionLevel);
    }

    public enum PdfCompressionLevel
    {
        None,
        Low,
        Medium,
        High
    }
} 