using Firebase.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using IMP.Models;
using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IMP.Services
{
    public class FirestoreService
    {
        private FirestoreDb _db;

        public FirestoreService()
        {
            InitializeFirestore().Wait();
        }

        private async Task InitializeFirestore()
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("impdb-557fa-firebase-adminsdk-7gw7l-be2ef53692.json");
            GoogleCredential credential;
            using (var reader = new StreamReader(stream))
            {
                credential = GoogleCredential.FromStream(reader.BaseStream);
            }

            var firestoreClient = new FirestoreClientBuilder
            {
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();

            _db = FirestoreDb.Create("impdb-557fa", firestoreClient);
        }

        public async Task AddUserAsync(string userId, string email)
        {
            try
            {
                var userRef = _db.Collection("users").Document(userId);
                var user = new
                {
                    Email = email,
                    CreatedAt = Timestamp.GetCurrentTimestamp()
                };

                // Loguj, że rozpoczynasz operację
                Console.WriteLine($"[DEBUG] Attempting to add user with ID: {userId} to Firestore.");

                // Sprawdź, czy użytkownik już istnieje
                var snapshot = await userRef.GetSnapshotAsync();
                if (!snapshot.Exists)
                {
                    await userRef.SetAsync(user);
                    Console.WriteLine($"[DEBUG] User with ID: {userId} successfully added to Firestore.");
                }
                else
                {
                    Console.WriteLine($"[DEBUG] User with ID: {userId} already exists in Firestore.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to add user to Firestore: {ex.Message}");
                throw;
            }
        }

        public async Task AddSectionAsync(Section section)
        {
            var sectionsRef = _db.Collection("users").Document(section.UserId).Collection("sections");
            await sectionsRef.Document(section.Id).SetAsync(section);
        }

        public async Task<List<Section>> GetSectionsAsync(string userId)
        {
            var sectionsRef = _db.Collection("users").Document(userId).Collection("sections");
            var snapshot = await sectionsRef.GetSnapshotAsync();

            var sections = new List<Section>();
            foreach (var document in snapshot.Documents)
            {
                var section = document.ConvertTo<Section>();
                sections.Add(section);
            }
            return sections;
        }

        public async Task TestFirestoreConnection()
        {
            try
            {
                var testRef = _db.Collection("test").Document("testDoc");
                await testRef.SetAsync(new { Test = "Connection Successful" });
                Console.WriteLine("Connection to Firestore successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to Firestore: " + ex.Message);
            }
        }

    }
}
