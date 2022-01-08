using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetAll;
using VBMS.Application.Features.vbms.diary.diary.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.GetAll;
using VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAll;
using VBMS.Client.Extensions;
using VBMS.Client.Infrastructure.Managers.Catalog.BrandTest;
using VBMS.Client.Infrastructure.Managers.Catalog.Diary;
using VBMS.Client.Infrastructure.Managers.Catalog.Vehicle;
using VBMS.Client.Infrastructure.Managers.vbms.bookings;
using VBMS.Client.Infrastructure.Managers.vbms.diary.diaryType;
using VBMS.Shared.Constants.Application;

namespace VBMS.Client.Pages.vbms.diary.diary
{
    public partial class AddEditDiaryModal
    {
        [Inject] private IDiaryManager DiaryManager { get; set; }
        [Inject] private IBookingManager BookingManager { get; set; }
        [Inject] private IVehicleManager VehicleManager { get; set; }
        [Inject] private IDiaryTypeManager DiaryTypeManager { get; set; }

        [Parameter] public AddEditDiaryCommand AddEditDiaryModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllBookingsResponse> _bookings = new();
        private List<GetAllVehiclesResponse> _vehicles = new();
        private List<GetAllDiaryTypesResponse> _diaryTypes = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await DiaryManager.SaveAsync(AddEditDiaryModel);
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
            await LoadVehiclesAsync();
            await LoadBookingsAsync();
            await LoadDiaryTypesAsync();
        }

        private async Task LoadDiaryTypesAsync()
        {
            var data = await DiaryTypeManager.GetAllAsync();
            if (data.Succeeded)
            {
                _diaryTypes = data.Data;
            }
        }
        private async Task LoadBookingsAsync()
        {
            var data = await BookingManager.GetAllAsync();
            if (data.Succeeded)
            {
                _bookings = data.Data;
            }
        }
        private async Task LoadVehiclesAsync()
        {
            var data = await VehicleManager.GetAllAsync();
            if (data.Succeeded)
            {
                _vehicles = data.Data;
            }
        }
    
        private async Task<IEnumerable<int>> SearchDiaryTypes(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _diaryTypes.Select(x => x.Id);

            return _diaryTypes.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        private async Task<IEnumerable<int>> SearchBookings(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _bookings.Select(x => x.Id);

            return _bookings.Where(x => x.BookingCode.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        private async Task<IEnumerable<int>> SearchVehicles(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _vehicles.Select(x => x.Id);

            return _vehicles.Where(x => x.Rego.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
    }
}