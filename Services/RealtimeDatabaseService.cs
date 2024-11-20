using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IMP.Models;

namespace IMP.Services
{
    public class RealtimeDatabaseService
    {
        private readonly string _databaseUrl = "https://impdb-557fa-default-rtdb.europe-west1.firebasedatabase.app/";
        private readonly HttpClient _httpClient;

        public RealtimeDatabaseService()
        {
            _httpClient = new HttpClient();
        }

        // Dodanie użytkownika do bazy danych
        public async Task AddUserAsync(string userId, string email)
        {
            try
            {
                var user = new
                {
                    Email = email,
                    CreatedAt = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_databaseUrl}/users/{userId}.json", content);
                response.EnsureSuccessStatusCode();

                Console.WriteLine($"[INFO] User {userId} added to Realtime Database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error adding user: {ex.Message}");
                throw;
            }
        }

        // Dodanie sekcji dla użytkownika
        public async Task AddSection(string userId, Section section)
        {
            try
            {
                var json = JsonSerializer.Serialize(section);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_databaseUrl}/sections/{userId}.json", content);
                response.EnsureSuccessStatusCode();

                Console.WriteLine($"[INFO] Section added for user {userId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error adding section: {ex.Message}");
                throw;
            }
        }

        // Pobranie wszystkich sekcji użytkownika
        public async Task<List<Section>> GetSections(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_databaseUrl}/sections/{userId}.json");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json) || json == "null")
                {
                    return new List<Section>();
                }

                var sections = JsonSerializer.Deserialize<Dictionary<string, Section>>(json);
                return sections?.Values.ToList() ?? new List<Section>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error fetching sections: {ex.Message}");
                throw;
            }
        }

        // Usunięcie sekcji użytkownika
        public async Task DeleteSection(string userId, string sectionId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_databaseUrl}/sections/{userId}/{sectionId}.json");
                response.EnsureSuccessStatusCode();

                Console.WriteLine($"[INFO] Section {sectionId} deleted for user {userId}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error deleting section: {ex.Message}");
                throw;
            }
        }
    }
}
