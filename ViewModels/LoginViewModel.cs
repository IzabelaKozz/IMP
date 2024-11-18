using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Użycie Preferences
using IMP.Services; // Dodaj tę linię na górze pliku

using Google.Cloud.Firestore;

namespace IMP.ViewModels
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        private readonly string webApiKey = "AIzaSyDNtwI02aWPPvuGGK22Hm8LskD6soyIpZY"; // Wstaw swój rzeczywisty klucz API
        private readonly INavigation _navigation;
        private string userName;
        private string userPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RegisterBtn { get; }
        public ICommand LoginBtn { get; }

        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public string UserPassword
        {
            get => userPassword;
            set
            {
                userPassword = value;
                RaisePropertyChanged(nameof(UserPassword));
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            RegisterBtn = new Command(RegisterBtnTappedAsync);
            LoginBtn = new Command(LoginBtnTappedAsync);
        }

        private async void LoginBtnTappedAsync(object obj)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
            try
            {
                // Logowanie użytkownika
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(UserName, UserPassword);

                string userId = auth.User.LocalId;
                string email = auth.User.Email;

                // Zapisanie tokena użytkownika do Preferences
                var serializedToken = JsonConvert.SerializeObject(auth);
                Preferences.Set("FirebaseAuthToken", serializedToken);

                // Dodanie użytkownika do Firestore
                var firestoreService = new FirestoreService();
                await firestoreService.AddUserAsync(userId, email);

                // Przekierowanie do HomePage po udanym logowaniu
                await _navigation.PushAsync(new HomePage(userId));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Logowanie nie powiodło się: " + ex.Message, "OK");
            }
        }



        private async void RegisterBtnTappedAsync(object obj)
        {
            // Przekierowanie do strony rejestracji
            await _navigation.PushAsync(new RegisterPage());
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
