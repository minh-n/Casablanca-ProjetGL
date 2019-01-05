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
        private List<ExpenseLine> ExpenseLines { get; set; }

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
                new Collaborator("Morgan", "FEURTE", "morgan", EncodeMD5("f")),
                new Collaborator("Minh", "NGUYEN", "cds", EncodeMD5("cds")),
                new Collaborator("Adrien", "LAVILLONNIERE", "a", EncodeMD5("t")),
                new Collaborator("Jeffrey", "GONCALVES"),
                new Collaborator("Yao", "SHI", "cpt", EncodeMD5("cpt")),
                new Collaborator("Arthur", "BINELLI"),
                new Collaborator("Thibal", "WITCZAK", "rh", EncodeMD5("rh")),
                new Collaborator("Floriab", "LE PALLEC"),
                new Collaborator("Oubar", "MAYAKI"),
                new Collaborator("Nathon", "BONNARD")
            };

            // Add coll to services
            AddToService(1, 1); //minh le cds PDG
            AddToService(1, 2); //dadri le user + admin
            AddToService(0, 3); //jaff le CDS normal
            AddToService(2, 4); //yao le compta
            AddToService(2, 5); 
            AddToService(3, 6); //thib le RH
            AddToService(5, 7);
            AddToService(4, 8);
            AddToService(2, 0); //mow le cds compta

            // Add admin role to Adrien
            Collaborators[2].Role = Roles.ADMIN;
			Collaborators[3].Role = Roles.CHIEF;
			Collaborators[1].Role = Roles.CHIEF;
			Collaborators[0].Role = Roles.CHIEF;

			// Create missions
			Missions = new List<Mission> {
                new Mission("Voyage vers Tipperary", DateTime.Today, new DateTime(2019, 5, 1), MissionStatus.IN_PROGRESS, Services[0]),
                new Mission("Voyage vers Agartha", new DateTime(2019, 2, 9), new DateTime(2019, 3, 1), MissionStatus.PLANNED, Services[0]),
                new Mission("Voyage vers l'au-delà", new DateTime(2018, 12, 25), new DateTime(2018, 12, 26), MissionStatus.CANCELED, Services[1]),
                new Mission("Voyage voyage", new DateTime(2019, 2, 25), new DateTime(2019, 2, 26), MissionStatus.COMPLETED, Services[0]),
                new Mission("Voyage vers Mars", new DateTime(2019, 2, 6), new DateTime(2019, 3, 1), MissionStatus.PLANNED, Services[0]),
                new Mission("Voyage vers le Mur", new DateTime(2019, 1, 9), new DateTime(2019, 11, 1), MissionStatus.PLANNED, Services[0]),
                new Mission("Voyage à Fuji-san", new DateTime(2019, 1, 2), new DateTime(2019, 1, 4), MissionStatus.IN_PROGRESS, Services[0])
            };

            // Assign missions to collaborators

            Collaborators[1].Missions.Add(Missions[0]);
            Collaborators[1].Missions.Add(Missions[1]);
            Collaborators[1].Missions.Add(Missions[2]);
            Collaborators[1].Missions.Add(Missions[3]);
            Collaborators[1].Missions.Add(Missions[5]);

			//Adrien is coll2, Adrien gets all the missions because he is admin
			Collaborators[2].Missions = Missions;

			//a enlever, ca c'est pour voir
			Collaborators[3].Missions = Missions;
			Collaborators[4].Missions = Missions;


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

            // Create some expense reports 
            //TODO bizarre : j'ajoute les expense au coll3, alors que c'est le coll 1 qui a des missions. 
            // et ça marche. Par contre si j'enlevais une mission au coll1, alors y a nullpointer.
            ExpenseReports = new List<ExpenseReport>() {
                new ExpenseReport(GetCollaborator("Arthur", "BINELLI"), Month.DECEMBER, 2018, ExpenseReportStatus.PENDING_APPROVAL_2),
				new ExpenseReport(GetCollaborator("Floriab", "LE PALLEc"), Month.DECEMBER, 2018, ExpenseReportStatus.PENDING_APPROVAL_1),
				new ExpenseReport(GetCollaborator("Oubar", "MAYAKI"), Month.JANUARY, 2019, ExpenseReportStatus.UNSENT),
                new ExpenseReport(GetCollaborator("Jeffrey", "GONCALVES"), Month.FEBRUARY, 2018, ExpenseReportStatus.PENDING_APPROVAL_2),
                new ExpenseReport(GetCollaborator("Jeffrey", "GONCALVES"), Month.NOVEMBER, 2018, ExpenseReportStatus.APPROVED)
            };

			//Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(4), Month.FEBRUARY, 2019));
			//Db.ExpenseReports.Add(new ExpenseReport(GetCollaborator(5), Month.JANUARY, 2018));

			Db.SaveChanges();

            ExpenseLines = new List<ExpenseLine>() {
                new ExpenseLine(GetMission(1), LineType.HOTEL, GetCollaborator("Minh", "NGUYEN"), "Trump Tower", 1000.0f, new DateTime(2019, 3, 4), "trumptower.pdf"),
                new ExpenseLine(GetMission(2), LineType.RESTAURANT, GetCollaborator("Jeffrey", "GONCALVES"), "Trump Tower restaurant", 8000.0f, new DateTime(2019, 1, 2), "trumptower.pdf"),
                new ExpenseLine(GetMission(3), LineType.TAXI, GetCollaborator("Jeffrey", "GONCALVES"), "Trump Taxi", 15.98f, new DateTime(2019, 3, 4), "trumptower.pdf"),
                new ExpenseLine(GetMission(5), LineType.OTHER, GetCollaborator("Jeffrey", "GONCALVES"), "Trump Other", 80010.0f, new DateTime(2019, 1, 2), "russia.pdf")
            };

			//lignes du frais 1 (arthur)
			foreach (ExpenseLine el in ExpenseLines) {ExpenseReports[0].AddLine(el);}

			//ligne d'oummar
			ExpenseReports[1].AddLine(new ExpenseLine(GetMission(6), LineType.RESTAURANT, "Pepperoni Pizza", 10.0f, new DateTime(2019, 1, 7), "pizza.pdf"));

			//frais 4 (jeffrey). Jeffrey n'a aucune mission à effectuer actuellement
			ExpenseReports[3].AddLine(new ExpenseLine(GetMission(1), LineType.RESTAURANT, GetCollaborator("Jeffrey", "GONCALVES"), "Trump Burger", 10.0f, new DateTime(2019, 1, 5), "trumpburger.pdf"));
			ExpenseReports[3].AddLine(new ExpenseLine(GetMission(3), LineType.HOTEL, GetCollaborator("Minh", "NGUYEN"), "Jafar Hotel", 10.0f, new DateTime(2019, 1, 5), "hotel.pdf"));

			//foreach (ExpenseReport er in ExpenseReports) {
			//    Db.ExpenseReports.Add(er);
			//}

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

        public Collaborator GetCollaborator(string idString) {
            if (int.TryParse(idString, out int id))
                return GetCollaborator(id);
            return null;
        }

        public Collaborator GetCollaborator(string firstname, string lastname) {
            return Db.Collaborators.SingleOrDefault(c => c.FirstName == firstname && c.LastName == lastname);
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