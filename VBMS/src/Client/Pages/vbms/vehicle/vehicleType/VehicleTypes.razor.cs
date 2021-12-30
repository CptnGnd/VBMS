using VBMS.Client.Extensions;
using VBMS.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VBMS.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicleType;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetAll;
using static VBMS.Shared.Constants.Permission.Permissions;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Commands.AddEdit;

namespace VBMS.Client.Pages.vbms.vehicle.vehicleType
{
    public partial class VehicleTypes
    {
        [Inject] private IVehicleTypeManager VehicleTypeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllVehicleTypesResponse> _vehicleTypeList = new();
        private GetAllVehicleTypesResponse _vehicleType = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateVehicleTypes;
        private bool _canEditVehicleTypes;
        private bool _canDeleteVehicleTypes;
        private bool _canExportVehicleTypes;
        private bool _canSearchVehicleTypes;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateVehicleTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypes.Create)).Succeeded;
            _canEditVehicleTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypes.Edit)).Succeeded;
            _canDeleteVehicleTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypes.Delete)).Succeeded;
            _canExportVehicleTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypes.Export)).Succeeded;
            _canSearchVehicleTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypes.Search)).Succeeded;

            await GetVehicleTypesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetVehicleTypesAsync()
        {
            var response = await VehicleTypeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _vehicleTypeList = response.Data.ToList();
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
                var response = await VehicleTypeManager.DeleteAsync(id);
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
            var response = await VehicleTypeManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(VehicleTypes).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["VehicleTypes exported"]
                    : _localizer["Filtered VehicleTypes exported"], Severity.Success);
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
                _vehicleType = _vehicleTypeList.FirstOrDefault(c => c.Id == id);
                if (_vehicleType != null)
                {
                    parameters.Add(nameof(AddEditVehicleTypeModal.AddEditVehicleTypeModel), new AddEditVehicleTypeCommand
                    {
                        Id = _vehicleType.Id,
                        Name = _vehicleType.Name,
                        Description = _vehicleType.Description
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditVehicleTypeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _vehicleType = new GetAllVehicleTypesResponse();
            await GetVehicleTypesAsync();
        }

        private bool Search(GetAllVehicleTypesResponse vehicleType)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (vehicleType.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (vehicleType.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}