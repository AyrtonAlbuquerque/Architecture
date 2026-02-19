using Architecture.Api.Abstractions.Services;
using Architecture.Api.Infrastructure.Documents;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Architecture.Api.Infrastructure.Services
{
    public class PDFService : IPDFService
    {
        public PDFService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<string> CreateUserPDF()
        {
            using(var stream = new MemoryStream())
            {
                var documento = new UserDocument();

                documento.GeneratePdf(stream);

                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}