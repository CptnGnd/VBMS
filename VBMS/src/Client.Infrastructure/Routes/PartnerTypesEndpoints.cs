namespace VBMS.Client.Infrastructure.Routes
{
    public static class PartnerTypesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/partnerTypes/export";

        public static string GetAll = "api/v1/partnerTypes";
        public static string Delete = "api/v1/partnerTypes";
        public static string Save = "api/v1/partnerTypes";
        public static string GetCount = "api/v1/partnerTypes/count";
    }
}