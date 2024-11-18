using IMP.Models;
using IMP.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace IMP.ViewModels
{
    public class SectionsViewModel : BaseViewModel
    {
        private readonly FirestoreService _firestoreService;
        private readonly string _userEmail;

        public ObservableCollection<Section> Sections { get; set; }
        public string SectionName { get; set; }
        public string StartTime { get; set; }
        public string Duration { get; set; }
        public List<string> SelectedDays { get; set; } = new List<string>();

        public ICommand AddSectionCommand { get; }

        // Konstruktor bez argumentów (opcjonalny, ale przydatny dla XAML)
        public SectionsViewModel()
        {
            _firestoreService = new FirestoreService();
            Sections = new ObservableCollection<Section>();
            AddSectionCommand = new Command(async () => await AddSection());

            LoadSections();
        }

        // Konstruktor przyjmujący email użytkownika
        public SectionsViewModel(string userEmail) : this() // Wywołanie domyślnego konstruktora
        {
            _userEmail = userEmail;
        }

        private async void LoadSections()
        {
            var userId = "userId123"; // Pobierz UID użytkownika na podstawie _userEmail (jeśli to konieczne)
            var sections = await _firestoreService.GetSectionsAsync(userId);

            foreach (var section in sections)
            {
                Sections.Add(section);
            }
        }

        private async Task AddSection()
        {
            if (string.IsNullOrEmpty(SectionName) || string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(Duration))
            {
                Console.WriteLine("All fields are required.");
                return;
            }

            var section = new Section
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "userId123", // Pobierz UID zalogowanego użytkownika
                Name = SectionName,
                StartTime = StartTime,
                Duration = Duration,
                SelectedDays = SelectedDays
            };

            await _firestoreService.AddSectionAsync(section);

            Sections.Add(section);
            Console.WriteLine("Section added.");
        }
    }
}
