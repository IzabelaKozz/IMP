using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Firebase.Auth;

namespace IMP.ViewModels
{
    public class HomeViewModel : BindableObject
    {
        private readonly INavigation _navigation;
        private readonly string _userEmail; // Dodajemy userId

        public ICommand NavigateToSectionsCommand { get; }

        public HomeViewModel(INavigation navigation, string userEmail)
        {
            _navigation = navigation;
            _userEmail = userEmail; // Przypisujemy email
            NavigateToSectionsCommand = new Command(async () => await NavigateToSections());
        }


        private async Task NavigateToSections()
        {
            await _navigation.PushAsync(new SectionsPage(_userEmail)); // Przekazujemy email do SectionsPage
        }

    }
}
