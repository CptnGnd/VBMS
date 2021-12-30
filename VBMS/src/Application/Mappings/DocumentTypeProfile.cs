using AutoMapper;
using VBMS.Application.Features.DocumentTypes.Commands.AddEdit;
using VBMS.Application.Features.DocumentTypes.Queries.GetAll;
using VBMS.Application.Features.DocumentTypes.Queries.GetById;
using VBMS.Domain.Entities.Misc;

namespace VBMS.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}