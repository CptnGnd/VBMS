using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VBMS.Shared.Constants.Permission
{
    public static class Permissions
    {
        public static class Vehicles
        {
            public const string View = "Permissions.Vehicles.View";
            public const string Create = "Permissions.Vehicles.Create";
            public const string Edit = "Permissions.Vehicles.Edit";
            public const string Delete = "Permissions.Vehicles.Delete";
            public const string Export = "Permissions.Vehicles.Export";
            public const string Search = "Permissions.Vehicles.Search";
        }
        public static class Partners
        {
            public const string View = "Permissions.Partners.View";
            public const string Create = "Permissions.Partners.Create";
            public const string Edit = "Permissions.Partners.Edit";
            public const string Delete = "Permissions.Partners.Delete";
            public const string Export = "Permissions.Partners.Export";
            public const string Search = "Permissions.Partners.Search";
        }

        public static class ProductTests
        {
            public const string View = "Permissions.ProductTests.View";
            public const string Create = "Permissions.ProductTests.Create";
            public const string Edit = "Permissions.ProductTests.Edit";
            public const string Delete = "Permissions.ProductTests.Delete";
            public const string Export = "Permissions.ProductTests.Export";
            public const string Search = "Permissions.ProductTests.Search";
        }
        public static class VehicleTypes
        {
            public const string View = "Permissions.VehicleTypes.View";
            public const string Create = "Permissions.VehicleTypes.Create";
            public const string Edit = "Permissions.VehicleTypes.Edit";
            public const string Delete = "Permissions.VehicleTypes.Delete";
            public const string Export = "Permissions.VehicleTypes.Export";
            public const string Search = "Permissions.VehicleTypes.Search";
        }
        public static class PartnerTypes
        {
            public const string View = "Permissions.PartnerTypes.View";
            public const string Create = "Permissions.PartnerTypes.Create";
            public const string Edit = "Permissions.PartnerTypes.Edit";
            public const string Delete = "Permissions.PartnerTypes.Delete";
            public const string Export = "Permissions.PartnerTypes.Export";
            public const string Search = "Permissions.PartnerTypes.Search";
        }
       public static class BrandTests
        {
            public const string View = "Permissions.BrandTests.View";
            public const string Create = "Permissions.BrandTests.Create";
            public const string Edit = "Permissions.BrandTests.Edit";
            public const string Delete = "Permissions.BrandTests.Delete";
            public const string Export = "Permissions.BrandTests.Export";
            public const string Search = "Permissions.BrandTests.Search";
        }
        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Create = "Permissions.Documents.Create";
            public const string Edit = "Permissions.Documents.Edit";
            public const string Delete = "Permissions.Documents.Delete";
            public const string Search = "Permissions.Documents.Search";
        }

        public static class DocumentTypes
        {
            public const string View = "Permissions.DocumentTypes.View";
            public const string Create = "Permissions.DocumentTypes.Create";
            public const string Edit = "Permissions.DocumentTypes.Edit";
            public const string Delete = "Permissions.DocumentTypes.Delete";
            public const string Export = "Permissions.DocumentTypes.Export";
            public const string Search = "Permissions.DocumentTypes.Search";
        }

        public static class DocumentExtendedAttributes
        {
            public const string View = "Permissions.DocumentExtendedAttributes.View";
            public const string Create = "Permissions.DocumentExtendedAttributes.Create";
            public const string Edit = "Permissions.DocumentExtendedAttributes.Edit";
            public const string Delete = "Permissions.DocumentExtendedAttributes.Delete";
            public const string Export = "Permissions.DocumentExtendedAttributes.Export";
            public const string Search = "Permissions.DocumentExtendedAttributes.Search";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Export = "Permissions.Users.Export";
            public const string Search = "Permissions.Users.Search";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
        }

        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }

        public static class Communication
        {
            public const string Chat = "Permissions.Communication.Chat";
        }

        public static class Preferences
        {
            public const string ChangeLanguage = "Permissions.Preferences.ChangeLanguage";

            //TODO - add permissions
        }

        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }

        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Export = "Permissions.AuditTrails.Export";
            public const string Search = "Permissions.AuditTrails.Search";
        }
       /// <summary>
       /// Returns a list of Permissions.
       /// </summary>
       /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permssions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permssions.Add(propertyValue.ToString());
            }
            return permssions;
        }
    }
}