using VBMS.Shared.Settings;
using System.Threading.Tasks;
using VBMS.Shared.Wrapper;

namespace VBMS.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}