using Blazored.Toast.Services;
using Energy.Blazor.Services;
using Energy.Services.DTO;
using Energy.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Toolbelt.Blazor.HotKeys2;

namespace Energy.Blazor.Pages.Temp
{
    public partial class WingTemp : IDisposable
    {
        [Inject]
        public IToastService? ToastService { get; set; }

        [Inject]
        public HotKeys? HotKeys { get; set; }

        [Inject]
        public Toolbelt.Blazor.I18nText.I18nText? I18nText { get; set; }

        [Inject]
        public IsTaskRunningService? IsTaskRunningService { get; set; }

        [Inject]
        public IEgonService EgonService { get; set; }

        public LocationDTO SelectedDetailedLocation { get; set; } = new();

        public const int PageSize = 5;
        public int Count;

        private RadzenDataGrid<LocationDTO>? _locationInformationGrid;
        private List<LocationDTO> _locationInformationFloor = new();
        private List<LocationDTO> _locationInformationRoom = new();

        private HotKeysContext? _hotKeysContext;
        private I18nText.LanguageTable _languageTable = new();

        protected override async Task OnInitializedAsync()
        {
            ArgumentNullException.ThrowIfNull(HotKeys);
            ArgumentNullException.ThrowIfNull(I18nText);
            ArgumentNullException.ThrowIfNull(IsTaskRunningService);

            IsTaskRunningService.RefreshRequested += RefreshMe;

            _languageTable = await I18nText.GetTextTableAsync<I18nText.LanguageTable>(this);
            _hotKeysContext = HotKeys.CreateContext()
                .Add(Code.F8, Toaster);

            _locationInformationFloor = await EgonService.GetAllLocationsBySchoolAsync("EUC");
        }


        public async Task OnSelectedLocationFloor(LocationDTO locationfloor)
        {
            ArgumentNullException.ThrowIfNull(IsTaskRunningService);

            IsTaskRunningService.IsTaskRunning = true;
            SelectedDetailedLocation.School = "EUC";
            SelectedDetailedLocation.Floor = locationfloor.Floor;
            _locationInformationRoom = await EgonService.GetAllRoomsByFloorAsync(locationfloor.Floor);
            IsTaskRunningService.IsTaskRunning = false;
        }


        public async Task OnSelectedLocationRoom(LocationDTO locationfloor)
        {
            ArgumentNullException.ThrowIfNull(IsTaskRunningService);

            IsTaskRunningService.IsTaskRunning = true;
            SelectedDetailedLocation.Room = locationfloor.Room;
            IsTaskRunningService.IsTaskRunning = false;
        }

		private async Task LoadData(LoadDataArgs args)
		{
			ArgumentNullException.ThrowIfNull(IsTaskRunningService);

			IsTaskRunningService.IsTaskRunning = true;
			_locationInformationFloor = await EgonService.GetAllLocationsBySchoolAsync("EUC");
			IsTaskRunningService.IsTaskRunning = false;
		}


		void Toaster()
        {
            ArgumentNullException.ThrowIfNull(ToastService);
            ToastService.ShowInfo("Congratulations, you just pressed hotkey: F8");
        }


        private void RefreshMe()
        {
            StateHasChanged();
        }


        public void Dispose()
        {
            ArgumentNullException.ThrowIfNull(IsTaskRunningService);
            IsTaskRunningService.RefreshRequested -= RefreshMe;
            _hotKeysContext?.Dispose();
        }
    }
}
