using Grocery.App.ViewModels;
using Grocery.Core.Interfaces.Services;

namespace Grocery.App.Views;

public partial class LoginView : ContentPage
{
    private readonly IClientService _clientService;

    public LoginView(LoginViewModel viewModel, IClientService clientService)
    {
        InitializeComponent(); 
        BindingContext = viewModel;
        _clientService = clientService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterView(_clientService));
    }
}
