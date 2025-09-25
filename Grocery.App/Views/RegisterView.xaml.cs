using Grocery.App.ViewModels;
using Grocery.Core.Interfaces.Services;

namespace Grocery.App.Views;

public partial class RegisterView : ContentPage
{
    public RegisterView(IClientService clientService)
    {
        InitializeComponent();
        BindingContext = new RegisterViewModel(clientService);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
