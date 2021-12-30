namespace VBMS.Client.Infrastructure.Routes
{
    public static class BrandTestsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/brandTests/export";

        public static string GetAll = "api/v1/brandTests";
        public static string Delete = "api/v1/brandTests";
        public static string Save = "api/v1/brandTests";
        public static string GetCount = "api/v1/brandTests/count";
    }
}