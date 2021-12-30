using AutoMapper;
using VBMS.Application.Features.ProductTests.Commands.AddEdit;
using VBMS.Domain.Entities.Catalog;

namespace VBMS.Application.Mappings
{
    public class ProductTestProfile : Profile
    {
        public ProductTestProfile()
        {
            CreateMap<AddEditProductTestCommand, ProductTest>().ReverseMap();
        }
    }
}