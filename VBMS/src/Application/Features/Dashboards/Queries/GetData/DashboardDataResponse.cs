using System.Collections.Generic;

namespace VBMS.Application.Features.Dashboards.Queries.GetData
{
    public class DashboardDataResponse
    {
        public int PartnerCount { get; set; }
        public int VehicleCount { get; set; }
        public int ProductTestCount { get; set; }
        public int BrandTestCount { get; set; }
        public int DocumentCount { get; set; }
        public int DocumentTypeCount { get; set; }
        public int DocumentExtendedAttributeCount { get; set; }
        public int UserCount { get; set; }
        public int RoleCount { get; set; }
        public List<ChartSeries> DataEnterBarChart { get; set; } = new();
        public Dictionary<string, double> ProductTestByBrandTestTypePieChart { get; set; }
    }

    public class ChartSeries
    {
        public ChartSeries() { }

        public string Name { get; set; }
        public double[] Data { get; set; }
    }

}