using QuestPDF.Infrastructure;

namespace Architecture.Infrastructure.Documents
{
    public class UserDocument : IDocument
    {
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            throw new NotImplementedException();
        }
    }
}