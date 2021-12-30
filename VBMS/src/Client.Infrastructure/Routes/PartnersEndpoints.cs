using System.Linq;

namespace VBMS.Client.Infrastructure.Routes
{
    public static class PartnersEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/partners?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetCount = "api/v1/partners/count";

        public static string GetPartnerImage(int partnerId)
        {
            return $"api/v1/partners/image/{partnerId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Save = "api/v1/partners";
        public static string Delete = "api/v1/partners";
        public static string Export = "api/v1/partners/export";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";
    }
}