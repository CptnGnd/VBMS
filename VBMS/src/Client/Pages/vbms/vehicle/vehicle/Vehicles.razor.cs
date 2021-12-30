using VBMS.Client.Extensions;
using VBMS.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VBMS.Client.Infrastructure.Managers.Catalog.Vehicle;
using VBMS.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.vehicle;
using VBMS.Application.Features.vbms.vehicle.vehicle.Commands.AddEdit;

namespace VBMS.Client.Pages.vbms.vehicle.vehicle
{
    public partial class Vehicles
    {
        [Inject] private IVehicleManager VehicleManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedVehiclesResponse> _pagedData;
        private MudTable<GetAllPagedVehiclesResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateVehicles;
        private bool _canEditVehicles;
        private bool _canDeleteVehicles;
        private bool _canExportVehicles;
        private bool _canSearchVehicles;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateVehicles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Vehicles.Create)).Succeeded;
            _canEditVehicles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Vehicles.Edit)).Succeeded;
            _canDeleteVehicles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Vehicles.Delete)).Succeeded;
            _canExportVehicles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Vehicles.Export)).Succeeded;
            _canSearchVehicles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Vehicles.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPagedVehiclesResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedVehiclesResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedVehiclesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await VehicleManager.GetVehiclesAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task ExportToExcel()
        {
            var response = await VehicleManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Vehicles).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Vehicles exported"]
                    : _localizer["Filtered Vehicles exported"], Severity.Success);
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
                var vehicle = _pagedData.FirstOrDefault(c => c.Id == id);
                if (vehicle != null)
                {
                    parameters.Add(nameof(AddEditVehicleModal.AddEditVehicleModel), new AddEditVehicleCommand
                    {
                        Id = vehicle.Id,
                        Rego = vehicle.Rego,
                        Description = vehicle.Description,
                        VehicleTypeId = vehicle.VehicleTypeId
                        //v = vehicle.VehicleType.
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditVehicleModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
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
                var response = await VehicleManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
    }
}