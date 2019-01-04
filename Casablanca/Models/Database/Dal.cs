using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Casablanca.Models.ExpenseReports;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

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

			// Create services
			db.Services.Add(new Service("Informatique"));
			db.Services.Add(new Service("Direction"));
			db.Services.Add(new Service("Compta"));
			db.Services.Add(new Service("RH"));
			db.Services.Add(new Service("Plomberie"));
			db.Services.Add(new Service("Matériaux"));

			db.SaveChanges();
			
			// Create collaborators
			db.Collaborators.Add(new Collaborator("Morgan", "FEURTE", "salutMorgan", EncodeMD5("test")));
            db.Collaborators.Add(new Collaborator("Minh", "NGUYEN", "greetings", EncodeMD5("test1")));
            db.Collaborators.Add(new Collaborator("Adrien", "LAVILLONNIERE"));
            db.Collaborators.Add(new Collaborator("Jeffrey", "GONCALVES"));
            db.Collaborators.Add(new Collaborator("Yao", "SHI"));
			db.Collaborators.Add(new Collaborator("Arthur", "BINELLI"));
			db.Collaborators.Add(new Collaborator("Thibal", "WITCZAK"));
			db.Collaborators.Add(new Collaborator("Florab", "LE PALLEC"));
			db.Collaborators.Add(new Collaborator("Oubar", "MAYAKI"));
			db.Collaborators.Add(new Collaborator("Nathon", "BONNARD"));

			db.SaveChanges();


			// Add coll to services
			AddToService(1, 1);
			AddToService(1, 2);
			AddToService(1, 3);
			AddToService(2, 4);
			AddToService(2, 5);
			AddToService(3, 6);
			AddToService(5, 7);
			AddToService(4, 8);
			AddToService(6, 9);

			db.SaveChanges();


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
            db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(2), Month.JANUARY, 2019));
            db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(2), Month.DECEMBER, 2018));

            db.SaveChanges();

            ExpenseLine el1 = new ExpenseLine(GetMission(1), LineType.HOTEL, "Trump tower", 1000.0f, new DateTime(2019, 3, 4), "trumptower.pdf");
            ExpenseLine el2 = new ExpenseLine(GetMission(1), LineType.RESTAURANT, "Trump tower restaurant", 8000.0f, new DateTime(2019, 1, 2), "trumptower.pdf");
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
            return db.Collaborators.SingleOrDefault(c => c.Id == id);
        }

        // Missions
        public Mission GetMission(int id) {
            return db.Missions.SingleOrDefault(m => m.Id == id);
        }

        public List<Mission> GetCollaboratorMissions(int collId) {
            return GetCollaborator(collId).Missions;
        }

        // ExpenseReports
        public List<ExpenseReport> GetExpenseReports() {
            return db.ExpenseReports.ToList();
        }

		public ExpenseReport GetExpenseReport(int id) {
            return db.ExpenseReports.SingleOrDefault(e => e.Id == id);
        }

		// Login
		public Collaborator Login(string name, string pass)
		{
			string passEncoded = EncodeMD5(pass);
			return db.Collaborators.FirstOrDefault(u => u.Login == name && u.Password == passEncoded);
		}

		private string EncodeMD5(string pass)
        {
            string passSalt = "ChevalDeMetal" + pass + "Casablanca";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(passSalt)));
        }

		// Services
		public List<Service> GetServices()
		{
			return db.Services.ToList();
		}

		public Service GetService(int id)
		{
			return db.Services.SingleOrDefault(s => s.Id == id);
		}

		public void AddToService(int serviceId, int collId)
		{
			GetService(serviceId).CollList.Add(GetCollaborator(collId));
			GetCollaborator(collId).Service = GetService(serviceId);

		}
		
		public void Dispose()
        {
            db.Dispose();
        }
    }
}