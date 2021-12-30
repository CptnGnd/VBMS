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
using VBMS.Application.Features.vbms.partner.partnerType.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partnerType.Queries.GetAll;
using VBMS.Client.Extensions;
using VBMS.Client.Infrastructure.Managers.vbms.partner.partnerType;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Client.Pages.vbms.partner.partnerType
{
    public partial class PartnerTypes
    {
        [Inject] private IPartnerTypeManager PartnerTypeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllPartnerTypesResponse> _partnerTypeList = new();
        private GetAllPartnerTypesResponse _partnerType = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePartnerTypes;
        private bool _canEditPartnerTypes;
        private bool _canDeletePartnerTypes;
        private bool _canExportPartnerTypes;
        private bool _canSearchPartnerTypes;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePartnerTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PartnerTypes.Create)).Succeeded;
            _canEditPartnerTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PartnerTypes.Edit)).Succeeded;
            _canDeletePartnerTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PartnerTypes.Delete)).Succeeded;
            _canExportPartnerTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PartnerTypes.Export)).Succeeded;
            _canSearchPartnerTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PartnerTypes.Search)).Succeeded;

            await GetPartnerTypesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetPartnerTypesAsync()
        {
            var response = await PartnerTypeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _partnerTypeList = response.Data.ToList();
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
                var response = await PartnerTypeManager.DeleteAsync(id);
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
            var response = await PartnerTypeManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(PartnerTypes).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["PartnerTypes exported"]
                    : _localizer["Filtered PartnerTypes exported"], Severity.Success);
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
                _partnerType = _partnerTypeList.FirstOrDefault(c => c.Id == id);
                if (_partnerType != null)
                {
                    parameters.Add(nameof(AddEditPartnerTypeModal.AddEditPartnerTypeModel), new AddEditPartnerTypeCommand
                    {
                        Id = _partnerType.Id,
                        Name = _partnerType.Name,
                        Description = _partnerType.Description
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditPartnerTypeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _partnerType = new GetAllPartnerTypesResponse();
            await GetPartnerTypesAsync();
        }

        private bool Search(GetAllPartnerTypesResponse brandTest)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (brandTest.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (brandTest.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}