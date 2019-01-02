using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Casablanca.Models.Database
{
    public class Dal : IDal
    {
        private DatabaseContext db { get; set; }

        public Dal()
        {
            db = new DatabaseContext();
        }

        public void InitializeDatabase() {
            // Create collaborators
            db.Collaborators.Add(new Collaborator("Morgan", "FEURTE", "Informatique"));
            db.Collaborators.Add(new Collaborator("Minh", "NGUYEN", "Management"));
            db.Collaborators.Add(new Collaborator("Adrien", "LAVILLONNIERE", "Informatique"));
            db.Collaborators.Add(new Collaborator("Jeffrey", "GONCALVES", "Informatique"));
            db.Collaborators.Add(new Collaborator("Yao", "SHI", "Informatique"));

            // Create missions
            db.Missions.Add(new Mission("Voyage vers Tipperary", 0, DateTime.Today, new DateTime(2019, 5, 1), MissionStatus.IN_PROGRESS));
            db.Missions.Add(new Mission("Voyage vers Agartha", 1, new DateTime(2019, 2, 9), new DateTime(2019, 3, 1), MissionStatus.PLANNED));
            db.Missions.Add(new Mission("Voyage vers l'au-delà", 2, new DateTime(2018, 12, 25), new DateTime(2018, 12, 26), MissionStatus.CANCELED));
            db.Missions.Add(new Mission("Voyage voyage", 3, new DateTime(2019, 2, 25), new DateTime(2019, 2, 26), MissionStatus.COMPLETED));
            db.Missions.Add(new Mission("Voyage vers Mars", 4, new DateTime(2019, 2, 6), new DateTime(2019, 3, 1), MissionStatus.PLANNED));
            db.Missions.Add(new Mission("Voyage vers le Mur", 5, new DateTime(2019, 1, 9), new DateTime(2019, 11, 1), MissionStatus.PLANNED));
            db.Missions.Add(new Mission("Voyage à Fuji-san", 6, new DateTime(2019, 1, 2), new DateTime(2019, 1, 4), MissionStatus.IN_PROGRESS));

            db.SaveChanges();

            // Assign missions to collaborators
            GetCollaborator(1).Missions.Add(GetMission(1));
            GetCollaborator(1).Missions.Add(GetMission(2));
            GetCollaborator(1).Missions.Add(GetMission(3));
            GetCollaborator(2).Missions.Add(GetMission(6));
            GetCollaborator(0).Missions.Add(GetMission(0));

            db.SaveChanges();
        }

        // Collaborators
        public List<Collaborator> GetCollaborators()
        {
            return db.Collaborators.ToList();
        }

        public Collaborator GetCollaborator(int id) {
            //Action<Collaborator> lambda = x => x.Id.Equals(id);
            return db.Collaborators.Find(id);
        }

        // Missions
        public Mission GetMission(int id) {
            Action<Mission> lambda = x => x.Id.Equals(id);
            return db.Missions.Find(lambda);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}