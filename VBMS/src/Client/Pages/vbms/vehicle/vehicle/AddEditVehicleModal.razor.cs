using VBMS.Application.Requests;
using VBMS.Client.Extensions;
using VBMS.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using VBMS.Client.Infrastructure.Managers.Catalog.Vehicle;
using VBMS.Application.Features.vbms.vehicle.vehicle.Commands.AddEdit;
using VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicleType;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetAll;

namespace VBMS.Client.Pages.vbms.vehicle.vehicle
{
    public partial class AddEditVehicleModal
    {
        [Inject] private IVehicleManager VehicleManager { get; set; }
        [Inject] private IVehicleTypeManager VehicleTypeManager { get; set; }

        [Parameter] public AddEditVehicleCommand AddEditVehicleModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllVehicleTypesResponse> _vehicleTypes = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await VehicleManager.SaveAsync(AddEditVehicleModel);
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
            await LoadVehicleTypesAsync();
        }

        private async Task LoadVehicleTypesAsync()
        {
            var data = await VehicleTypeManager.GetAllAsync();
            if (data.Succeeded)
            {
                _vehicleTypes = data.Data;
            }
        }

        private async Task LoadImageAsync()
        {
            var data = await VehicleManager.GetVehicleImageAsync(AddEditVehicleModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditVehicleModel.ImageDataURL = imageData;
                }
            }
        }

        private void DeleteAsync()
        {
            AddEditVehicleModel.ImageDataURL = null;
            AddEditVehicleModel.UploadRequest = new UploadRequest();
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
                AddEditVehicleModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditVehicleModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Vehicle, Extension = extension };
            }
        }

        private async Task<IEnumerable<int>> SearchVehicleTypes(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _vehicleTypes.Select(x => x.Id);

            return _vehicleTypes.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}