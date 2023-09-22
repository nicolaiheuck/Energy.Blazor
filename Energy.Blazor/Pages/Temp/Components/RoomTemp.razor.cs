using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Toolbelt.Blazor.HotKeys2;

namespace Energy.Blazor.Pages.Temp.Components
{
    public partial class RoomTemp : IDisposable
    {
        [Inject]
        public IToastService? ToastService { get; set; }

        [Inject]
        public HotKeys? HotKeys { get; set; }

        [Inject]
        public Toolbelt.Blazor.I18nText.I18nText? I18nText { get; set; }

        //public ? SelectedDetailedWing { get; set; }
        //public ? SelectedDetailedroom { get; set; }
        public const int PageSize = 5;
        public int Count;

        //private RadzenDataGrid<Room>? _roomInformationGrid;
        //private List<Room> _roomInformationVms = new();

        private HotKeysContext? _hotKeysContext;
        private I18nText.LanguageTable _languageTable = new();

        protected override async Task OnInitializedAsync()
        {
            ArgumentNullException.ThrowIfNull(HotKeys);
            ArgumentNullException.ThrowIfNull(I18nText);

            _languageTable = await I18nText.GetTextTableAsync<I18nText.LanguageTable>(this);
            _hotKeysContext = HotKeys.CreateContext()
                .Add(Code.F8, Toaster);
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
