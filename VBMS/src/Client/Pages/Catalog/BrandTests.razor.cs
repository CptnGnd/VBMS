using VBMS.Application.Features.BrandTests.Queries.GetAll;
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
using VBMS.Application.Features.BrandTests.Commands.AddEdit;
using VBMS.Client.Infrastructure.Managers.Catalog.BrandTest;
using VBMS.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

namespace VBMS.Client.Pages.Catalog
{
    public partial class BrandTests
    {
        [Inject] private IBrandTestManager BrandTestManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllVehicleTypessResponse> _brandTestList = new();
        private GetAllVehicleTypessResponse _brandTest = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = true;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateBrandTests;
        private bool _canEditBrandTests;
        private bool _canDeleteBrandTests;
        private bool _canExportBrandTests;
        private bool _canSearchBrandTests;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateBrandTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BrandTests.Create)).Succeeded;
            _canEditBrandTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BrandTests.Edit)).Succeeded;
            _canDeleteBrandTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BrandTests.Delete)).Succeeded;
            _canExportBrandTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BrandTests.Export)).Succeeded;
            _canSearchBrandTests = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BrandTests.Search)).Succeeded;

            await GetBrandTestsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetBrandTestsAsync()
        {
            var response = await BrandTestManager.GetAllAsync();
            if (response.Succeeded)
            {
                _brandTestList = response.Data.ToList();
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
                var response = await BrandTestManager.DeleteAsync(id);
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
            var response = await BrandTestManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(BrandTests).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["BrandTests exported"]
                    : _localizer["Filtered BrandTests exported"], Severity.Success);
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
                _brandTest = _brandTestList.FirstOrDefault(c => c.Id == id);
                if (_brandTest != null)
                {
                    parameters.Add(nameof(AddEditBrandTestModal.AddEditBrandTestModel), new AddEditBrandTestCommand
                    {
                        Id = _brandTest.Id,
                        Name = _brandTest.Name,
                        Description = _brandTest.Description,
                        Tax = _brandTest.Tax
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBrandTestModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _brandTest = new GetAllVehicleTypessResponse();
            await GetBrandTestsAsync();
        }

        private bool Search(GetAllVehicleTypessResponse brandTest)
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