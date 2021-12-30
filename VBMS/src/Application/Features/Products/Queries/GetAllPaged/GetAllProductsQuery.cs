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

namespace VBMS.Application.Features.ProductTests.Queries.GetAllPaged
{
    public class GetAllProductTestsQuery : IRequest<PaginatedResult<GetAllPagedProductTestsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllProductTestsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllProductTestsQueryHandler : IRequestHandler<GetAllProductTestsQuery, PaginatedResult<GetAllPagedProductTestsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductTestsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductTestsResponse>> Handle(GetAllProductTestsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductTest, GetAllPagedProductTestsResponse>> expression = e => new GetAllPagedProductTestsResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Rate = e.Rate,
                Barcode = e.Barcode,
                BrandTest = e.BrandTest.Name,
                BrandTestId = e.BrandTestId
            };
            var productTestFilterSpec = new ProductTestFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<ProductTest>().Entities
                   .Specify(productTestFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<ProductTest>().Entities
                   .Specify(productTestFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}