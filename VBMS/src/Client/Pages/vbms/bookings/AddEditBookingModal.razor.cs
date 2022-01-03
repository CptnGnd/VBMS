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
using VBMS.Application.Features.vbms.booking.booking.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partner.Queries.GetAll;
using VBMS.Application.Features.vbms.partner.partner.Queries.GetAllPaged;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetAll;
using VBMS.Application.Requests;
using VBMS.Client.Extensions;
using VBMS.Client.Infrastructure.Managers.Catalog.BrandTest;
using VBMS.Client.Infrastructure.Managers.vbms.bookings;
using VBMS.Client.Infrastructure.Managers.vbms.partner.partner;
using VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicleType;
using VBMS.Shared.Constants.Application;

namespace VBMS.Client.Pages.vbms.bookings
{
    public partial class AddEditBookingModal
    {
        [Inject] private IBookingManager BookingManager { get; set; }
        [Inject] private IVehicleTypeManager VehicleTypeManager { get; set; }
        [Inject] private IPartnerManager PartnerManager { get; set; }

        [Parameter] public AddEditBookingCommand AddEditBookingModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllVehicleTypesResponse> _vehicleTypes = new();
        private List<GetAllPartnersResponse> _partners = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await BookingManager.SaveAsync(AddEditBookingModel);
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
            //   await LoadImageAsync();
            await LoadVehicleTypesAsync();
            await LoadPartnersAsync();
        }

        private async Task LoadVehicleTypesAsync()
        {
            var data = await VehicleTypeManager.GetAllAsync();
            if (data.Succeeded)
            {
                _vehicleTypes = data.Data;
            }
        }

        private async Task LoadPartnersAsync()
        {
            var data = await PartnerManager.GetAllPartnersAsync();
            if (data.Succeeded)
            {
                _partners = data.Data;
            }
        }

        //private async Task LoadImageAsync()
        //{
        //    var data = await BookingManager.GetBookingImageAsync(AddEditBookingModel.Id);
        //    if (data.Succeeded)
        //    {
        //        var imageData = data.Data;
        //        if (!string.IsNullOrEmpty(imageData))
        //        {
        //            AddEditBookingModel.ImageDataURL = imageData;
        //        }
        //    }
        //}

        //private void DeleteAsync()
        //{
        //    AddEditBookingModel.ImageDataURL = null;
        //    AddEditBookingModel.UploadRequest = new UploadRequest();
        //}

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
                //AddEditBookingModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                //AddEditBookingModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Booking, Extension = extension };
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

        private async Task<IEnumerable<int>> SearchPartners(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _partners.Select(x => x.Id);

            return _partners.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}