using IMP.ViewModels;
using Microsoft.Maui.Controls;

namespace IMP
{
    public partial class SectionsPage : ContentPage
    {
        private readonly string _userId;

        public SectionsPage(string userId)
        {
            InitializeComponent();
            _userId = userId;

            // Przekazujemy userId do ViewModelu
            BindingContext = new SectionsViewModel();
        
        }
    }
}
