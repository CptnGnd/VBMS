using System.Linq;

namespace VBMS.Client.Infrastructure.Routes
{
    public static class ProductTestsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/productTests?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }

        public static string GetCount = "api/v1/productTests/count";

        public static string GetProductTestImage(int productTestId)
        {
            return $"api/v1/productTests/image/{productTestId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Save = "api/v1/productTests";
        public static string Delete = "api/v1/productTests";
        public static string Export = "api/v1/productTests/export";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";
    }
}