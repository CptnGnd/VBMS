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
using VBMS.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using VBMS.Client.Infrastructure.Managers.vbms.bookings;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.booking;
using VBMS.Application.Features.vbms.booking.booking.Commands.AddEdit;

namespace VBMS.Client.Pages.vbms.bookings
{
    public partial class Bookings
    {
        [Inject] private IBookingManager BookingManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedBookingsResponse> _pagedData;
        private MudTable<GetAllPagedBookingsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateBookings;
        private bool _canEditBookings;
        private bool _canDeleteBookings;
        private bool _canExportBookings;
        private bool _canSearchBookings;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateBookings = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Bookings.Create)).Succeeded;
            _canEditBookings = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Bookings.Edit)).Succeeded;
            _canDeleteBookings = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Bookings.Delete)).Succeeded;
            _canExportBookings = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Bookings.Export)).Succeeded;
            _canSearchBookings = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Bookings.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPagedBookingsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedBookingsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedBookingsRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await BookingManager.GetBookingsAsync(request);
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
            var response = await BookingManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Bookings).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Bookings exported"]
                    : _localizer["Filtered Bookings exported"], Severity.Success);
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
                    parameters.Add(nameof(AddEditBookingModal.AddEditBookingModel), new AddEditBookingCommand
                    {
                        Id = productTest.Id,
                        BookingCode = productTest.BookingCode,
                        BookingType = productTest.BookingType,
                        StartDate = productTest.StartDate,
                        EndDate = productTest.EndDate,
                        PartnerId = productTest.PartnerId,
                     //   P = productTest.Barcode,
                        VehicleTypeId = productTest.VehicleTypeId,
                      //  V = productTest.VehicleType
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBookingModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await BookingManager.DeleteAsync(id);
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