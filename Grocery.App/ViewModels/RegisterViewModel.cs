using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    // partial voor WinRT/analyzer compatibiliteit
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IClientService _clientService;

        // initialiseer velden zodat ze niet-null zijn (CS8618)
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel(IClientService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));

            RegisterCommand = new Command(async () => await OnRegisterAsync());
        }

        private async Task OnRegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                await ShowAlertAsync("Fout", "Vul alle velden in.");
                return;
            }

            try
            {
                var newClient = new Client(0, Name, Email, Password);

                _clientService.Register(newClient);

                await ShowAlertAsync("Succes", "Account aangemaakt!");

                var nav = Application.Current?.MainPage?.Navigation;
                if (nav != null)
                {
                    await nav.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Fout", ex.Message);
            }
        }

        private static Task ShowAlertAsync(string title, string message)
        {
            var main = Application.Current?.MainPage;
            if (main != null)
                return main.DisplayAlert(title, message, "OK");

            System.Diagnostics.Debug.WriteLine($"{title}: {message}");
            return Task.CompletedTask;
        }
    }
}
