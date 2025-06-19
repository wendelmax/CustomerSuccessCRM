using CustomerSuccessCRM.Lib.Services.Contracts;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.IO.Image;
using iText.Kernel.Geom;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Scriban;
using System.Text;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class PdfService : IPdfService
    {
        public async Task<byte[]> GeneratePdfAsync(string templateName, object model)
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfTemplates", $"{templateName}.html");
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Template PDF não encontrado: {templateName}");
            }

            var templateContent = await File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(templateContent);
            var html = template.Render(model);

            return await GeneratePdfFromHtmlAsync(html);
        }

        public async Task<byte[]> GeneratePdfFromHtmlAsync(string html)
        {
            using (var stream = new MemoryStream())
            {
                // Usar QuestPDF para converter HTML em PDF
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Content().Html(html);
                    });
                })
                .GeneratePdf(stream);

                return stream.ToArray();
            }
        }

        public async Task<byte[]> MergePdfsAsync(IEnumerable<byte[]> pdfs)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new PdfWriter(stream))
                {
                    using (var mergedPdf = new PdfDocument(writer))
                    {
                        foreach (var pdf in pdfs)
                        {
                            using (var reader = new PdfReader(new MemoryStream(pdf)))
                            {
                                using (var sourcePdf = new PdfDocument(reader))
                                {
                                    sourcePdf.CopyPagesTo(1, sourcePdf.GetNumberOfPages(), mergedPdf);
                                }
                            }
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public async Task<byte[]> AddWatermarkAsync(byte[] pdf, string watermark)
        {
            using (var stream = new MemoryStream())
            {
                using (var reader = new PdfReader(new MemoryStream(pdf)))
                {
                    using (var writer = new PdfWriter(stream))
                    {
                        using (var pdfDoc = new PdfDocument(reader, writer))
                        {
                            var font = iText.Kernel.Font.PdfFontFactory.CreateFont();
                            var numberOfPages = pdfDoc.GetNumberOfPages();

                            for (int i = 1; i <= numberOfPages; i++)
                            {
                                var page = pdfDoc.GetPage(i);
                                var pageSize = page.GetPageSize();
                                var canvas = new PdfCanvas(page);
                                var document = new Document(pdfDoc);

                                // Configurar watermark
                                canvas.SaveState()
                                    .SetFillColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                                    .BeginText()
                                    .SetFontAndSize(font, 60)
                                    .SetTextMatrix(30, 30)
                                    .ShowText(watermark)
                                    .EndText()
                                    .RestoreState();
                            }
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public async Task<byte[]> AddPasswordAsync(byte[] pdf, string password)
        {
            var writerProperties = new WriterProperties()
                .SetStandardEncryption(
                    Encoding.UTF8.GetBytes(password),
                    Encoding.UTF8.GetBytes(password),
                    EncryptionConstants.ALLOW_PRINTING,
                    EncryptionConstants.ENCRYPTION_AES_128);

            using (var stream = new MemoryStream())
            {
                using (var reader = new PdfReader(new MemoryStream(pdf)))
                {
                    using (var writer = new PdfWriter(stream, writerProperties))
                    {
                        using (var pdfDoc = new PdfDocument(reader, writer))
                        {
                            // O documento é automaticamente criptografado
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public async Task<byte[]> AddSignatureAsync(byte[] pdf, byte[] signature, string location)
        {
            using (var stream = new MemoryStream())
            {
                using (var reader = new PdfReader(new MemoryStream(pdf)))
                {
                    using (var writer = new PdfWriter(stream))
                    {
                        using (var pdfDoc = new PdfDocument(reader, writer))
                        {
                            var document = new Document(pdfDoc);
                            var image = ImageDataFactory.Create(signature);
                            var imageModel = new Image(image);

                            // Adicionar assinatura na última página
                            var lastPage = pdfDoc.GetNumberOfPages();
                            var page = pdfDoc.GetPage(lastPage);
                            var pageSize = page.GetPageSize();

                            // Posicionar assinatura conforme location
                            switch (location.ToLower())
                            {
                                case "bottom-right":
                                    imageModel.SetFixedPosition(
                                        pageSize.GetWidth() - image.GetWidth() - 30,
                                        30);
                                    break;
                                case "bottom-left":
                                    imageModel.SetFixedPosition(30, 30);
                                    break;
                                case "top-right":
                                    imageModel.SetFixedPosition(
                                        pageSize.GetWidth() - image.GetWidth() - 30,
                                        pageSize.GetHeight() - image.GetHeight() - 30);
                                    break;
                                case "top-left":
                                    imageModel.SetFixedPosition(30,
                                        pageSize.GetHeight() - image.GetHeight() - 30);
                                    break;
                                default:
                                    imageModel.SetFixedPosition(
                                        (pageSize.GetWidth() - image.GetWidth()) / 2,
                                        (pageSize.GetHeight() - image.GetHeight()) / 2);
                                    break;
                            }

                            document.Add(imageModel);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public async Task<int> GetPageCountAsync(byte[] pdf)
        {
            using (var reader = new PdfReader(new MemoryStream(pdf)))
            {
                using (var pdfDoc = new PdfDocument(reader))
                {
                    return pdfDoc.GetNumberOfPages();
                }
            }
        }

        public async Task<byte[]> ExtractPagesAsync(byte[] pdf, int startPage, int endPage)
        {
            using (var stream = new MemoryStream())
            {
                using (var reader = new PdfReader(new MemoryStream(pdf)))
                {
                    using (var writer = new PdfWriter(stream))
                    {
                        using (var pdfDoc = new PdfDocument(reader))
                        {
                            using (var newPdf = new PdfDocument(writer))
                            {
                                pdfDoc.CopyPagesTo(startPage, endPage, newPdf);
                            }
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public async Task<byte[]> CompressPdfAsync(byte[] pdf, PdfCompressionLevel compressionLevel)
        {
            var writerProperties = new WriterProperties();

            switch (compressionLevel)
            {
                case PdfCompressionLevel.Low:
                    writerProperties.SetCompressionLevel(0);
                    break;
                case PdfCompressionLevel.Medium:
                    writerProperties.SetCompressionLevel(4);
                    break;
                case PdfCompressionLevel.High:
                    writerProperties.SetCompressionLevel(9);
                    break;
                default:
                    writerProperties.SetFullCompressionMode(false);
                    break;
            }

            using (var stream = new MemoryStream())
            {
                using (var reader = new PdfReader(new MemoryStream(pdf)))
                {
                    using (var writer = new PdfWriter(stream, writerProperties))
                    {
                        using (var pdfDoc = new PdfDocument(reader, writer))
                        {
                            // O documento é automaticamente comprimido
                        }
                    }
                }

                return stream.ToArray();
            }
        }
    }
} 