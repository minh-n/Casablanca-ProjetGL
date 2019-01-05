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
        private DatabaseContext Db { get; set; }

        private List<Service> Services { get; set; }
        private List<Collaborator> Collaborators { get; set; }
        private List<Mission> Missions { get; set; }
        private List<ExpenseReport> ExpenseReports { get; set; }

        public Dal()
        {
            Db = new DatabaseContext();
        }

        public void InitializeDatabase() {

            // Create services
            Services = new List<Service> {
                new Service("Informatique"),
                new Service("Direction"),
                new Service("Compta"),
                new Service("RH"),
                new Service("Plomberie"),
                new Service("Matériaux")
            };

			// Create collaborators
			Collaborators = new List<Collaborator> {
				new Collaborator("Morgan", "FEURTE"),
				new Collaborator("Minh", "NGUYEN"),
				new Collaborator("Adrien", "LAVILLONNIERE", "Dadri", EncodeMD5("test")),
                new Collaborator("Jeffrey", "GONCALVES"),
                new Collaborator("Yao", "SHI"),
                new Collaborator("Arthur", "BINELLI"),
                new Collaborator("Thibal", "WITCZAK"),
                new Collaborator("Florab", "LE PALLEC"),
                new Collaborator("Oubar", "MAYAKI"),
                new Collaborator("Nathon", "BONNARD")
            };

            // Add coll to services
            AddToService(1, 1);
			AddToService(1, 2);
			AddToService(1, 3);
			AddToService(2, 4);
			AddToService(2, 5);
			AddToService(3, 6);
			AddToService(5, 7);
			AddToService(4, 8);
			AddToService(0, 9);
			AddToService(0, 0);

            // Create missions
            Missions = new List<Mission> {
                new Mission("Voyage vers Tipperary", DateTime.Today, new DateTime(2019, 5, 1), MissionStatus.IN_PROGRESS, null),
                new Mission("Voyage vers Agartha", new DateTime(2019, 2, 9), new DateTime(2019, 3, 1), MissionStatus.PLANNED, null),
                new Mission("Voyage vers l'au-delà", new DateTime(2018, 12, 25), new DateTime(2018, 12, 26), MissionStatus.CANCELED, null),
                new Mission("Voyage voyage", new DateTime(2019, 2, 25), new DateTime(2019, 2, 26), MissionStatus.COMPLETED, null),
                new Mission("Voyage vers Mars", new DateTime(2019, 2, 6), new DateTime(2019, 3, 1), MissionStatus.PLANNED, null),
                new Mission("Voyage vers le Mur", new DateTime(2019, 1, 9), new DateTime(2019, 11, 1), MissionStatus.PLANNED, null),
                new Mission("Voyage à Fuji-san", new DateTime(2019, 1, 2), new DateTime(2019, 1, 4), MissionStatus.IN_PROGRESS, null)
            };

			// Assign missions to collaborators

			Collaborators[1].Missions.Add(Missions[0]);
			Collaborators[1].Missions.Add(Missions[1]);
			Collaborators[1].Missions.Add(Missions[2]);
			Collaborators[1].Missions.Add(Missions[3]);
			Collaborators[1].Missions.Add(Missions[5]);
			
			

			/*
			 * Adding every lists to the database
			 */
			foreach (Service s in Services) {
                Db.Services.Add(s);
            }

            foreach (Collaborator c in Collaborators) {
                Db.Collaborators.Add(c);
            }

            foreach (Mission m in Missions) {
                Db.Missions.Add(m);
            }

            Db.SaveChanges();

			// Create some expense reports TODO DO AS LIST
			//Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(5), Month.DECEMBER, 2018));
			//Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(5), Month.JANUARY, 2019));
            //Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(5), Month.DECEMBER, 2018));

			//TODO bizarre : j'ajoute les expense au coll3, alors que c'est le coll 1 qui a des missions. 
			// et ça marche. Par contre si j'enlevais une mission au coll1, alors y a nullpointer.

			Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(3), Month.DECEMBER, 2018));
			Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(3), Month.JANUARY, 2019));
			Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(3), Month.FEBRUARY, 2018));
			Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(3), Month.NOVEMBER, 2018));

			//Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(4), Month.FEBRUARY, 2019));
			//Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(5), Month.JANUARY, 2018));

			Db.SaveChanges();

            ExpenseLine el1 = new ExpenseLine(GetMission(1), LineType.HOTEL, "Trump Tower", 1000.0f, new DateTime(2019, 3, 4), "trumptower.pdf");
            ExpenseLine el2 = new ExpenseLine(GetMission(2), LineType.RESTAURANT, "Trump Tower restaurant", 8000.0f, new DateTime(2019, 1, 2), "trumptower.pdf");
			ExpenseLine e21 = new ExpenseLine(GetMission(3), LineType.TAXI, "Trump Taxi", 15.98f, new DateTime(2019, 3, 4), "trumptower.pdf");
			ExpenseLine e31 = new ExpenseLine(GetMission(5), LineType.RESTAURANT, "Trump Tower restaurant", 80010.0f, new DateTime(2019, 1, 2), "trumptower.pdf");

			GetExpenseReport(2).AddLine(el1);
            GetExpenseReport(2).AddLine(el2);
			GetExpenseReport(2).AddLine(e21);
			GetExpenseReport(2).AddLine(e31);

			Db.SaveChanges();
        }

        // Collaborators
        public List<Collaborator> GetCollaborators()
        {
            return Db.Collaborators.ToList();
        }

        public Collaborator GetCollaborator(int id) {
            return Db.Collaborators.SingleOrDefault(c => c.Id == id);
        }

        // Missions
        public Mission GetMission(int id) {
            return Db.Missions.SingleOrDefault(m => m.Id == id);
        }

        public List<Mission> GetCollaboratorMissions(int collId) {
            return GetCollaborator(collId).Missions;
        }

        // ExpenseReports
        public List<ExpenseReport> GetExpenseReports() {
            return Db.ExpenseReports.ToList();
        }

		public ExpenseReport GetExpenseReport(int id) {
            return Db.ExpenseReports.SingleOrDefault(e => e.Id == id);
        }

		// Login
		public Collaborator Login(string name, string pass)
		{
			string passEncoded = EncodeMD5(pass);
			return Db.Collaborators.FirstOrDefault(u => u.Login == name && u.Password == passEncoded);
		}

		private string EncodeMD5(string pass)
        {
            string passSalt = "ChevalDeMetal" + pass + "Casablanca";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(passSalt)));
        }

		// Services
		public List<Service> GetServices()
		{
			return Db.Services.ToList();
		}

		public Service GetService(int id)
		{
			return Db.Services.SingleOrDefault(s => s.Id == id);
		}

		public void AddToService(int serviceId, int collId)
		{
            //GetService(serviceId).CollList.Add(GetCollaborator(collId));
			//GetCollaborator(collId).Service = GetService(serviceId);
            Services[serviceId].CollList.Add(Collaborators[collId]);
            Collaborators[collId].Service = Services[serviceId];
        }
		
		// Admin
		public void SetCollaboratorAccount(int collId, string login, string pass)
		{
			GetCollaborator(collId).Login = login;
			GetCollaborator(collId).Password = EncodeMD5(pass);

			Db.SaveChanges(); //Very important to save
		}





		public void Dispose()
        {
            Db.Dispose();
        }
    }
}