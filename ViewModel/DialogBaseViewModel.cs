using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class DialogBaseViewModel : ObservableValidator
{
    public event Action<bool?>? CloseRequested;

    protected void OnCloseRequested(bool result)
    {
        CloseRequested?.Invoke(result);
        CloseRequested = null;
    }

    [RelayCommand]
    private void Confirm()
    {
        OnCloseRequested(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        OnCloseRequested(false);
    }
}