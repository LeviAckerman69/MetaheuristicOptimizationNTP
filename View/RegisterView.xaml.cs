using MetaheuristicOptimizationNTP.ViewModel;
using System.Windows;

namespace MetaheuristicOptimizationNTP.View
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        public RegisterView(RegisterViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();

            viewModel.CloseRequested += (result) =>
            {
                DialogResult = result;
                Close();
            };
        }
    }
}
