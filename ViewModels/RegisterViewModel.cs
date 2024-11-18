using Firebase.Auth;
using IMP.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace IMP.ViewModels
{
    internal class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly string webApiKey = "AIzaSyDNtwI02aWPPvuGGK22Hm8LskD6soyIpZY";
        private INavigation _navigation;
        private string email;
        private string password;
        private string repeatPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        public string RepeatPassword
        {
            get => repeatPassword;
            set
            {
                repeatPassword = value;
                RaisePropertyChanged(nameof(RepeatPassword));
            }
        }

        public ICommand RegisterUser { get; }

        public RegisterViewModel(INavigation navigation)
        {
            _navigation = navigation;
            RegisterUser = new Command(RegisterUserTappedAsync);
        }

        private async void RegisterUserTappedAsync(object obj)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RepeatPassword))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Please enter email and both password fields.", "OK");
                return;
            }

            if (Password != RepeatPassword)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Passwords do not match.", "OK");
                return;
            }

            try
            {
                // Logowanie: Rozpoczęcie procesu rejestracji
                Console.WriteLine($"[DEBUG] Attempting to register user with email: {Email}");

                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);

                string userId = auth.User.LocalId;

                // Logowanie: Firebase Authentication zakończone
                Console.WriteLine($"[DEBUG] User registered in Firebase Authentication with ID: {userId}");

                // Dodanie użytkownika do Firestore
                var firestoreService = new FirestoreService();
                await firestoreService.AddUserAsync(userId, Email);

                // Logowanie: Użytkownik dodany do Firestore
                Console.WriteLine($"[DEBUG] User added to Firestore with ID: {userId}");

                await App.Current.MainPage.DisplayAlert("Alert", "User Registered successfully", "OK");
                await _navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
