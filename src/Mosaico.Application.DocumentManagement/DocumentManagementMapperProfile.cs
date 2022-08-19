using AutoMapper;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.CreateProjectDocument;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.UpdateProjectDocument;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Application.DocumentManagement
{
    public class DocumentManagementMapperProfile : Profile
    {
        public DocumentManagementMapperProfile()
        {
            CreateMap<DocumentContent, DocumentContentDTO>();
            CreateMap<ProjectDocument, ProjectDocumentDTO>()
                .ForMember(pd => pd.Contents, opt => opt.Ignore());
            CreateMap<ModifyDocumentDTO, CreateProjectDocumentCommand>();
            CreateMap<ModifyDocumentDTO, UpdateProjectDocumentCommand>();
            CreateMap<CreateProjectDocumentCommand, ProjectDocument>();
            CreateMap<UpdateProjectDocumentCommand, ProjectDocument>()
                .ForMember(pd => pd.ProjectId, opt => opt.Ignore());
        }
    }
}
