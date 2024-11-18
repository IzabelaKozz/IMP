using IMP.ViewModels;
using Microsoft.Maui.Controls;

namespace IMP
{
    public partial class SectionsPage : ContentPage
    {
        private readonly string _userEmail;

        public SectionsPage(string userEmail)
        {
            InitializeComponent();
            _userEmail = userEmail;

            // Przekazujemy email do ViewModelu
            BindingContext = new SectionsViewModel(_userEmail);
        }
    }
}
