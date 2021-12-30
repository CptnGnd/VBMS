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

namespace VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAllPaged
{
    public class GetAllVehiclesQuery : IRequest<PaginatedResult<GetAllPagedVehiclesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllVehiclesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, PaginatedResult<GetAllPagedVehiclesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllVehiclesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedVehiclesResponse>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Vehicle, GetAllPagedVehiclesResponse>> expression = e => new GetAllPagedVehiclesResponse
            {
                Id = e.Id,
                Rego = e.Rego,
                Description = e.Description,
                VehicleType = e.VehicleType.Name,
                VehicleTypeId = e.VehicleTypeId
            };
            var productTestFilterSpec = new VehicleFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Vehicle>().Entities
                   .Specify(productTestFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Vehicle>().Entities
                   .Specify(productTestFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}