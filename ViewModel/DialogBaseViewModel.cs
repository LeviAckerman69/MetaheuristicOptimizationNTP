using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MetaheuristicOptimizationNTP.ViewModel
{
    public partial class DialogBaseViewModel : ObservableValidator
    {
        public event Action<bool?>? CloseRequested;

        protected void OnCloseRequested(bool result)
        {
            CloseRequested?.Invoke(result);
            CloseRequested = null;
        }

        [RelayCommand]
        public void Confirm()
        {
            CloseRequested?.Invoke(true);
            CloseRequested = null;
        }

        [RelayCommand]
        public void Cancel()
        {
            CloseRequested?.Invoke(false);
            CloseRequested = null;
        }
    }
}
