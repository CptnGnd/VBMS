using AutoMapper;
using VBMS.Application.Features.Documents.Commands.AddEdit;
using VBMS.Application.Features.Documents.Queries.GetById;
using VBMS.Domain.Entities.Misc;

namespace VBMS.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}