using VBMS.Application.Features.ProductTests.Queries.GetAllPaged;
using VBMS.Application.Requests.Catalog;
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
using VBMS.Application.Features.ProductTests.Commands.AddEdit;
using VBMS.Client.Infrastructure.Managers.Catalog.ProductTest;
using VBMS.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace VBMS.Client.Pages.Catalog
{
    public partial class ProductTests
    {
        [Inject] private IProductTestManager ProductTestManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedProductTestsResponse> _pagedData;
        private MudTable<GetAllPagedProductTestsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProductTests;
        private bool _canEditProductTests;
        private bool _canDeleteProductTests;
        private bool _canExportProductTests;
        private bool _canSearchProductTests;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProductTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductTests.Create)).Succeeded;
            _canEditProductTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductTests.Edit)).Succeeded;
            _canDeleteProductTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductTests.Delete)).Succeeded;
            _canExportProductTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductTests.Export)).Succeeded;
            _canSearchProductTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.ProductTests.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPagedProductTestsResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductTestsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] {$"{state.SortLabel} {state.SortDirection}"} : new[] {$"{state.SortLabel}"};
            }

            var request = new GetAllPagedProductTestsRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await ProductTestManager.GetProductTestsAsync(request);
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
            var response = await ProductTestManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(ProductTests).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["ProductTests exported"]
                    : _localizer["Filtered ProductTests exported"], Severity.Success);
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
                    parameters.Add(nameof(AddEditProductTestModal.AddEditProductTestModel), new AddEditProductTestCommand
                    {
                        Id = productTest.Id,
                        Name = productTest.Name,
                        Description = productTest.Description,
                        Rate = productTest.Rate,
                        BrandTestId = productTest.BrandTestId,
                        Barcode = productTest.Barcode
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditProductTestModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await ProductTestManager.DeleteAsync(id);
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