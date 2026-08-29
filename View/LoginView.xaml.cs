using MetaheuristicOptimizationNTP.ViewModel;
using System.Windows;

namespace MetaheuristicOptimizationNTP.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView(LoginViewModel viewModel)
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
