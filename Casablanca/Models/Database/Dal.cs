using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Casablanca.Models.ExpenseReports;

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
            db.Missions.Add(new Mission("Voyage vers Tipperary", DateTime.Today, new DateTime(2019, 5, 1), MissionStatus.IN_PROGRESS, null));
            db.Missions.Add(new Mission("Voyage vers Agartha", new DateTime(2019, 2, 9), new DateTime(2019, 3, 1), MissionStatus.PLANNED, null));
            db.Missions.Add(new Mission("Voyage vers l'au-delà", new DateTime(2018, 12, 25), new DateTime(2018, 12, 26), MissionStatus.CANCELED, null));
            db.Missions.Add(new Mission("Voyage voyage", new DateTime(2019, 2, 25), new DateTime(2019, 2, 26), MissionStatus.COMPLETED, null));
            db.Missions.Add(new Mission("Voyage vers Mars", new DateTime(2019, 2, 6), new DateTime(2019, 3, 1), MissionStatus.PLANNED, null));
            db.Missions.Add(new Mission("Voyage vers le Mur", new DateTime(2019, 1, 9), new DateTime(2019, 11, 1), MissionStatus.PLANNED, null));
            db.Missions.Add(new Mission("Voyage à Fuji-san", new DateTime(2019, 1, 2), new DateTime(2019, 1, 4), MissionStatus.IN_PROGRESS, null));

            db.SaveChanges();

            // Assign missions to collaborators
            GetCollaborator(2).Missions.Add(GetMission(1));
            GetCollaborator(2).Missions.Add(GetMission(2));
            GetCollaborator(2).Missions.Add(GetMission(3));
            GetCollaborator(3).Missions.Add(GetMission(6));
            GetCollaborator(1).Missions.Add(GetMission(7));
            GetCollaborator(1).Missions.Add(GetMission(4));
            GetCollaborator(4).Missions.Add(GetMission(5));

            // Create some expense reports
            db.ExpenseReports.Add(new ExpenseReport(Month.JANUARY, 2019));
            db.ExpenseReports.Add(new ExpenseReport(Month.DECEMBER, 2018));

            ExpenseLine el1 = new ExpenseLine(GetMission(1), LineType.HOTEL, "Trump tower", 1000.0f, new DateTime(), "trumptower.pdf");
            ExpenseLine el2 = new ExpenseLine(GetMission(1), LineType.HOTEL, "Trump tower", 1000.0f, new DateTime(), "trumptower.pdf");
            GetExpenseReport(1).AddLine(el1);
            GetExpenseReport(1).AddLine(el2);

            db.SaveChanges();
        }

        // Collaborators
        public List<Collaborator> GetCollaborators()
        {
            return db.Collaborators.ToList();
        }

        public Collaborator GetCollaborator(int id) {
            return db.Collaborators.Find(id);
        }

        // Missions
        public Mission GetMission(int id) {
            return db.Missions.Find(id);
        }

        // ExpenseReports
        public List<ExpenseReport> GetExpenseReports() {
            return db.ExpenseReports.ToList();
        }

        public ExpenseReport GetExpenseReport(int id) {
            return db.ExpenseReports.Find(id);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}