using AutoMapper;
using VBMS.Application.Features.BrandTests.Commands.AddEdit;
using VBMS.Application.Features.BrandTests.Queries.GetAll;
using VBMS.Application.Features.BrandTests.Queries.GetById;
using VBMS.Domain.Entities.Catalog;

namespace VBMS.Application.Mappings
{
    public class BrandTestProfile : Profile
    {
        public BrandTestProfile()
        {
            CreateMap<AddEditBrandTestCommand, BrandTest>().ReverseMap();
            CreateMap<GetBrandTestByIdResponse, BrandTest>().ReverseMap();
            CreateMap<GetAllBrandTestsResponse, BrandTest>().ReverseMap();
        }
    }
}