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
using VBMS.Application.Features.vbms.diary.diary.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diary.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.diary;
using VBMS.Client.Extensions;
using VBMS.Client.Infrastructure.Managers.Catalog.Diary;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Client.Pages.vbms.diary.diary
{
    public partial class Diarys
    {
        [Inject] private IDiaryManager DiaryManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedDiarysResponse> _pagedData;
        private MudTable<GetAllPagedDiarysResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateDiarys;
        private bool _canEditDiarys;
        private bool _canDeleteDiarys;
        private bool _canExportDiarys;
        private bool _canSearchDiarys;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateDiarys = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diarys.Create)).Succeeded;
            _canEditDiarys = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diarys.Edit)).Succeeded;
            _canDeleteDiarys = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diarys.Delete)).Succeeded;
            _canExportDiarys = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diarys.Export)).Succeeded;
            _canSearchDiarys = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Diarys.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPagedDiarysResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedDiarysResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedDiarysRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await DiaryManager.GetDiarysAsync(request);
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
            var response = await DiaryManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Diarys).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Diarys exported"]
                    : _localizer["Filtered Diarys exported"], Severity.Success);
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
                var diary = _pagedData.FirstOrDefault(c => c.Id == id);
                if (diary != null)
                {
                    parameters.Add(nameof(AddEditDiaryModal.AddEditDiaryModel), new AddEditDiaryCommand
                    {
                        Id = diary.Id,
                        StartDateTime = diary.StartDatTime,
                        EndDateTime = diary.EndDatTime,
                        DiaryTypeId = diary.DiaryTypeId,
                        BookingId = diary.BookingId,
                        VehicleId = diary.VehicleId
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditDiaryModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await DiaryManager.DeleteAsync(id);
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