using IMP.Models;
using IMP.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

namespace IMP.ViewModels
{
    public class SectionsViewModel
    {
        private readonly RealtimeDatabaseService _databaseService;
        public List<Section> Sections { get; set; }

        public SectionsViewModel()
        {
            _databaseService = new RealtimeDatabaseService();
            Sections = new List<Section>();
        }

        // Metoda do pobierania sekcji dla zalogowanego użytkownika
        public async Task GetSections(string userId)
        {
            Sections = await _databaseService.GetSections(userId);
        }

        // Metoda do dodawania sekcji dla zalogowanego użytkownika
        public async Task AddSection(string userId, Section section)
        {
            await _databaseService.AddSection(userId, section);
            Sections.Add(section);  // Dodaj sekcję do listy
        }

        // Metoda do usuwania sekcji
        public async Task DeleteSection(string userId, string sectionId)
        {
            await _databaseService.DeleteSection(userId, sectionId);
            var sectionToRemove = Sections.FirstOrDefault(s => s.Id == sectionId);
            if (sectionToRemove != null)
            {
                Sections.Remove(sectionToRemove);  // Usuń sekcję z listy
            }
        }
    }
}
