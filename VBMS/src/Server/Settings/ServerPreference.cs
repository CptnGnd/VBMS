using System.Linq;
using VBMS.Shared.Constants.Localization;
using VBMS.Shared.Settings;

namespace VBMS.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

        //TODO - add server preferences
    }
}