using Grocery.App.ViewModels;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;

namespace Grocery.App;

public partial class App : Application
{
    public App(LoginViewModel viewModel, IClientService clientService)
    {
        InitializeComponent();

        MainPage = new NavigationPage(new LoginView(viewModel, clientService));
    }
}
