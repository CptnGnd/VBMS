using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VBMS.Application.Features.BrandTests.Queries.GetAll;
using VBMS.Application.Features.ProductTests.Commands.AddEdit;
using VBMS.Application.Requests;
using VBMS.Client.Extensions;
using VBMS.Client.Infrastructure.Managers.Catalog.BrandTest;
using VBMS.Client.Infrastructure.Managers.Catalog.ProductTest;
using VBMS.Shared.Constants.Application;

namespace VBMS.Client.Pages.Catalog
{
    public partial class AddEditProductTestModal
    {
        [Inject] private IProductTestManager ProductTestManager { get; set; }
        [Inject] private IBrandTestManager BrandTestManager { get; set; }

        [Parameter] public AddEditProductTestCommand AddEditProductTestModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllBrandTestsResponse> _brandTests = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await ProductTestManager.SaveAsync(AddEditProductTestModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadImageAsync();
            await LoadBrandTestsAsync();
        }

        private async Task LoadBrandTestsAsync()
        {
            var data = await BrandTestManager.GetAllAsync();
            if (data.Succeeded)
            {
                _brandTests = data.Data;
            }
        }

        private async Task LoadImageAsync()
        {
            var data = await ProductTestManager.GetProductTestImageAsync(AddEditProductTestModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditProductTestModel.ImageDataURL = imageData;
                }
            }
        }

        private void DeleteAsync()
        {
            AddEditProductTestModel.ImageDataURL = null;
            AddEditProductTestModel.UploadRequest = new UploadRequest();
        }

        private IBrowserFile _file;

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditProductTestModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditProductTestModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductTest, Extension = extension };
            }
        }

        private async Task<IEnumerable<int>> SearchBrandTests(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _brandTests.Select(x => x.Id);

            return _brandTests.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}