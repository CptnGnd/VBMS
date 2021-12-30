using MediatR;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Extensions;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Specifications.vbms;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.partner.partner.Queries.GetAllPaged
{
    public class GetAllPartnersQuery : IRequest<PaginatedResult<GetAllPagedPartnersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPartnersQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllPartnersQueryHandler : IRequestHandler<GetAllPartnersQuery, PaginatedResult<GetAllPagedPartnersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPartnersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedPartnersResponse>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Partner, GetAllPagedPartnersResponse>> expression = e => new GetAllPagedPartnersResponse
            {
                Id = e.Id,
                Name = e.Name,
                ShortName = e.ShortName,
                PartnerType = e.PartnerType.Name,
                PartnerTypeId = e.PartnerTypeId
            };
            var partnerFilterSpec = new PartnerFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Partner>().Entities
                   .Specify(partnerFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Partner>().Entities
                   .Specify(partnerFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}