using IMP.ViewModels;
using Firebase.Auth;
using Google.Cloud.Firestore;


namespace IMP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage(string userEmail)
        {
            InitializeComponent();
            BindingContext = new HomeViewModel(Navigation, userEmail); // Przekazujemy userId
        }
    }
}
