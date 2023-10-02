namespace Energy.Blazor.Services
{
    public class IsTaskRunningService
    {
        private bool _isTaskRunning;

        public bool IsTaskRunning
        {
            get
            {
                return _isTaskRunning;
            }
            set
            {
                _isTaskRunning = value;
                CallRequestRefresh();
            }
        }

        public event Action? RefreshRequested;
        private void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
    }
}
