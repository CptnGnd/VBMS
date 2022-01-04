using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.diary.diaryType.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.GetAll;
using VBMS.Client.Extensions;
using VBMS.Client.Infrastructure.Managers.vbms.diary.diaryType;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Client.Pages.vbms.diary.diaryType
{
    public partial class DiaryTypes
    {
        [Inject] private IDiaryTypeManager DiaryTypeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllDiaryTypesResponse> _diaryTypeList = new();
        private GetAllDiaryTypesResponse _diaryType = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateDiaryTypes;
        private bool _canEditDiaryTypes;
        private bool _canDeleteDiaryTypes;
        private bool _canExportDiaryTypes;
        private bool _canSearchDiaryTypes;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateDiaryTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DiaryTypes.Create)).Succeeded;
            _canEditDiaryTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DiaryTypes.Edit)).Succeeded;
            _canDeleteDiaryTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DiaryTypes.Delete)).Succeeded;
            _canExportDiaryTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DiaryTypes.Export)).Succeeded;
            _canSearchDiaryTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.DiaryTypes.Search)).Succeeded;

            await GetDiaryTypesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetDiaryTypesAsync()
        {
            var response = await DiaryTypeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _diaryTypeList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await DiaryTypeManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await DiaryTypeManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(DiaryTypes).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["DiaryTypes exported"]
                    : _localizer["Filtered DiaryTypes exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _diaryType = _diaryTypeList.FirstOrDefault(c => c.Id == id);
                if (_diaryType != null)
                {
                    parameters.Add(nameof(AddEditDiaryTypeModal.AddEditDiaryTypeModel), new AddEditDiaryTypeCommand
                    {
                        Id = _diaryType.Id,
                        Name = _diaryType.Name,
                        Description = _diaryType.Description,
                        Colour = _diaryType.Color
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditDiaryTypeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _diaryType = new GetAllDiaryTypesResponse();
            await GetDiaryTypesAsync();
        }

        private bool Search(GetAllDiaryTypesResponse diaryType)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (diaryType.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (diaryType.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}