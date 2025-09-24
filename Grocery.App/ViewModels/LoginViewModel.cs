using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class LoginViewModel(
        IAuthService authService,
        GlobalViewModel global
    ) : BaseViewModel
    {
        private readonly IAuthService _authService = authService;
        private readonly GlobalViewModel _global = global;

        [ObservableProperty]
        private string email = "user3@mail.com";

        [ObservableProperty]
        private string password = "user3";

        [ObservableProperty]
        private string loginMessage = string.Empty;

        [RelayCommand]
        private void Login()
        {
            Client? authenticatedClient = _authService.Login(Email, Password);

            if (authenticatedClient != null)
            {
                string clientName = authenticatedClient.Name ?? "gebruiker";
                LoginMessage = $"Welkom {clientName}!";

                _global.Client = authenticatedClient;
                Application.Current!.MainPage = new AppShell();
            }
            else
            {
                LoginMessage = "Ongeldige inloggegevens.";
            }
        }
    }
}

