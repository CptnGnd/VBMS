using AutoMapper;
using VBMS.Application.Features.BrandTests.Commands.AddEdit;
using VBMS.Application.Features.BrandTests.Queries.GetById;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetAll;
using VBMS.Domain.Entities.Catalog;

namespace VBMS.Application.Mappings
{
    public class BrandTestProfile : Profile
    {
        public BrandTestProfile()
        {
            CreateMap<AddEditBrandTestCommand, BrandTest>().ReverseMap();
            CreateMap<GetBrandTestByIdResponse, BrandTest>().ReverseMap();
            CreateMap<GetAllBookingsResponse, BrandTest>().ReverseMap();
        }
    }
}