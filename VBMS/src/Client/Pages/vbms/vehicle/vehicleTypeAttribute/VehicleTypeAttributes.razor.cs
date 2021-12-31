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
using VBMS.Application.Features.VehicleTypeAttributes.Commands.AddEdit;
using VBMS.Application.Features.VehicleTypeAttributes.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.vehicle;
using VBMS.Client.Extensions;
using VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicleTypeAttribute;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Client.Pages.vbms.vehicle.vehicleTypeAttribute
{
    public partial class VehicleTypeAttributes
    {
        [Inject] private IVehicleTypeAttributeManager VehicleTypeAttributeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedVehicleTypeAttributesResponse> _pagedData;
        private MudTable<GetAllPagedVehicleTypeAttributesResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateVehicleTypeAttributes;
        private bool _canEditVehicleTypeAttributes;
        private bool _canDeleteVehicleTypeAttributes;
        private bool _canExportVehicleTypeAttributes;
        private bool _canSearchVehicleTypeAttributes;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateVehicleTypeAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypeAttributes.Create)).Succeeded;
            _canEditVehicleTypeAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypeAttributes.Edit)).Succeeded;
            _canDeleteVehicleTypeAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypeAttributes.Delete)).Succeeded;
            _canExportVehicleTypeAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypeAttributes.Export)).Succeeded;
            _canSearchVehicleTypeAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.VehicleTypeAttributes.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPagedVehicleTypeAttributesResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedVehicleTypeAttributesResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedVehicleTypeAttributesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await VehicleTypeAttributeManager.GetVehicleTypeAttributesAsync(request);
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
            var response = await VehicleTypeAttributeManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(VehicleTypeAttributes).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["VehicleTypeAttributes exported"]
                    : _localizer["Filtered VehicleTypeAttributes exported"], Severity.Success);
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
                var productTest = _pagedData.FirstOrDefault(c => c.Id == id);
                if (productTest != null)
                {
                    parameters.Add(nameof(AddEditVehicleTypeAttributeModal.AddEditVehicleTypeAttributeModel), new AddEditVehicleTypeAttributeCommand
                    {
                        Id = productTest.Id,
                        Name = productTest.Name,
                        Description = productTest.Description,
                        AttributeType = productTest.AttributeType,
                        VehicleTypeId = productTest.VehicleTypeId
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditVehicleTypeAttributeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await VehicleTypeAttributeManager.DeleteAsync(id);
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