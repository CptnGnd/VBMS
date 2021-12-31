using VBMS.Application.Extensions;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Specifications.Catalog;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Domain.Entities.vbms.vehicles;
using VBMS.Application.Specifications.vbms;
using VBMS.Application.Features.VehicleTypeAttributes.Queries.GetAllPaged;

namespace VBMS.Application.Features.vbms.vehicle.vehicleTypeAttributes.Queries.GetAllPaged
{
    public class GetAllVehicleTypeAttributesQuery : IRequest<PaginatedResult<GetAllPagedVehicleTypeAttributesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllVehicleTypeAttributesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllVehicleTypeAttributesQueryHandler : IRequestHandler<GetAllVehicleTypeAttributesQuery, PaginatedResult<GetAllPagedVehicleTypeAttributesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllVehicleTypeAttributesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedVehicleTypeAttributesResponse>> Handle(GetAllVehicleTypeAttributesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<VehicleTypeAttribute, GetAllPagedVehicleTypeAttributesResponse>> expression = e => new GetAllPagedVehicleTypeAttributesResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                AttributeType = e.AttributeType,
                VehicleType = e.VehicleType.Name,
                VehicleTypeId = e.VehicleTypeId
            };
            var vehicleTypeAttributeFilterSpec = new VehicleTypeAttributeFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<VehicleTypeAttribute>().Entities
                   .Specify(vehicleTypeAttributeFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<VehicleTypeAttribute>().Entities
                   .Specify(vehicleTypeAttributeFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}