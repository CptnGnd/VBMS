using VBMS.Shared.Managers;
using MudBlazor;
using System.Threading.Tasks;

namespace VBMS.Client.Infrastructure.Managers.Preferences
{
    public interface IClientPreferenceManager : IPreferenceManager
    {
        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();
    }
}