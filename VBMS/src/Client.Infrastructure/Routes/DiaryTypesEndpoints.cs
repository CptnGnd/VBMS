namespace VBMS.Client.Infrastructure.Routes
{
    public static class DiaryTypesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/diaryTypes/export";

        public static string GetAll = "api/v1/diaryTypes";
        public static string Delete = "api/v1/diaryTypes";
        public static string Save = "api/v1/diaryTypes";
        public static string GetCount = "api/v1/diaryTypes/count";
    }
}
