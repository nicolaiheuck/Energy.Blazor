using Blazored.Toast.Services;
using Energy.Services.DTO;
using Energy.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.HotKeys2;

namespace Energy.Blazor.Pages.Power.Components
{
	public partial class PowerReadings : IDisposable
	{
		[Inject]
		public IToastService? ToastService { get; set; }

		[Inject]
		public HotKeys? HotKeys { get; set; }

		[Inject]
		public Toolbelt.Blazor.I18nText.I18nText? I18nText { get; set; }

		[Inject]
		public IEgonService? EgonService { get; set; }

		[CascadingParameter]
		public LocationDTO SelectedDetailedLocation { get; set; }

		private ThermostatSettingsDTO _thermostatSettings = new();
		private List<TelemetryDTO> _telemetryData = new();
		private HotKeysContext? _hotKeysContext;
		private I18nText.LanguageTable _languageTable = new();

		protected override async Task OnInitializedAsync()
		{
			ArgumentNullException.ThrowIfNull(HotKeys);
			ArgumentNullException.ThrowIfNull(I18nText);

			_languageTable = await I18nText.GetTextTableAsync<I18nText.LanguageTable>(this);
			_hotKeysContext = HotKeys.CreateContext()
				.Add(Code.F8, Toaster);
			_telemetryData = await EgonService.GetAllDataReadingsByLocationIdAsync(SelectedDetailedLocation, DateTime.Now.AddDays(-2), DateTime.Now);
		}

		async Task OnTempDateChange(DateTime? value)
		{
			_telemetryData = await EgonService.GetAllDataReadingsByLocationIdAsync(SelectedDetailedLocation, value, DateTime.Now);
			await InvokeAsync(() => StateHasChanged());
		}

		void Toaster()
		{
			ArgumentNullException.ThrowIfNull(ToastService);
			ToastService.ShowInfo("Congratulations, you just pressed hotkey: F8");
		}

		public void Dispose()
		{
			_hotKeysContext?.Dispose();
		}
	}
}
