using Google.Cloud.Firestore;
using System.Collections.Generic;

namespace IMP.Models
{
    [FirestoreData]
    public class Section
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string UserId { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string StartTime { get; set; }

        [FirestoreProperty]
        public string Duration { get; set; }

        [FirestoreProperty]
        public List<string> SelectedDays { get; set; }
    }
}
