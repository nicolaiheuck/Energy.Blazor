using Blazored.Toast.Services;
using Energy.Services.DTO;
using Energy.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Toolbelt.Blazor.HotKeys2;

namespace Energy.Blazor.Pages
{
	public partial class Index : IDisposable
	{
		[Inject]
		public IToastService? ToastService { get; set; }

		[Inject]
		public HotKeys? HotKeys { get; set; }

		[Inject]
		public Toolbelt.Blazor.I18nText.I18nText? I18nText { get; set; }

        [Inject]
        public IEgonService? EgonService { get; set; }

        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private HotKeysContext? _hotKeysContext;
		private I18nText.LanguageTable _languageTable = new();
		private List<TelemetryDTO> _telemetryData = new();
        private AuthenticationState _authenticationState;

        protected override async Task OnInitializedAsync()
		{
            _authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            ArgumentNullException.ThrowIfNull(HotKeys);
			ArgumentNullException.ThrowIfNull(I18nText);

			_languageTable = await I18nText.GetTextTableAsync<I18nText.LanguageTable>(this);
			_hotKeysContext = HotKeys.CreateContext()
				.Add(Code.F8, Toaster);
			_telemetryData = await EgonService.GetAveragedTelemetryAsync(DateTime.Now.AddMonths(-6), DateTime.Now, "EUC");
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
