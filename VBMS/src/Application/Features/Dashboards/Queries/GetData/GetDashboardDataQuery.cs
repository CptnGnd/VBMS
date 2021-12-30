using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services.Identity;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Domain.Entities.ExtendedAttributes;
using VBMS.Domain.Entities.Misc;
using Microsoft.Extensions.Localization;

namespace VBMS.Application.Features.Dashboards.Queries.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {

    }

    internal class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer;

        public GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IRoleService roleService, IStringLocalizer<GetDashboardDataQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
            _localizer = localizer;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            var response = new DashboardDataResponse
            {
                ProductTestCount = await _unitOfWork.Repository<ProductTest>().Entities.CountAsync(cancellationToken),
                BrandTestCount = await _unitOfWork.Repository<BrandTest>().Entities.CountAsync(cancellationToken),
                DocumentCount = await _unitOfWork.Repository<Document>().Entities.CountAsync(cancellationToken),
                DocumentTypeCount = await _unitOfWork.Repository<DocumentType>().Entities.CountAsync(cancellationToken),
                DocumentExtendedAttributeCount = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.CountAsync(cancellationToken),
                UserCount = await _userService.GetCountAsync(),
                RoleCount = await _roleService.GetCountAsync()
            };

            var selectedYear = DateTime.Now.Year;
            double[] productTestsFigure = new double[13];
            double[] brandTestsFigure = new double[13];
            double[] documentsFigure = new double[13];
            double[] documentTypesFigure = new double[13];
            double[] documentExtendedAttributesFigure = new double[13];
            for (int i = 1; i <= 12; i++)
            {
                var month = i;
                var filterStartDate = new DateTime(selectedYear, month, 01);
                var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based

                productTestsFigure[i - 1] = await _unitOfWork.Repository<ProductTest>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                brandTestsFigure[i - 1] = await _unitOfWork.Repository<BrandTest>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentsFigure[i - 1] = await _unitOfWork.Repository<Document>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentTypesFigure[i - 1] = await _unitOfWork.Repository<DocumentType>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentExtendedAttributesFigure[i - 1] = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
            }

            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["ProductTests"], Data = productTestsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["BrandTests"], Data = brandTestsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Documents"], Data = documentsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Document Types"], Data = documentTypesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Document Extended Attributes"], Data = documentExtendedAttributesFigure });

            return await Result<DashboardDataResponse>.SuccessAsync(response);
        }
    }
}