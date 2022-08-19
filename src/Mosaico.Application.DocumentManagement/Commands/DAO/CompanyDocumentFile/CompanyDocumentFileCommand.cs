using MediatR;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.CompanyDocumentFile
{
    public class CompanyDocumentFileCommand : IRequest<string>
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
