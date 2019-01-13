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

		/*
		 * ------------------------------------------------------------
		 * Initalize the DB with test data-----------------------------
		 * ------------------------------------------------------------
		 */
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
                new Collaborator("Jeffrey", "GONCALVES", "j", EncodeMD5("g")),
                new Collaborator("Yao", "SHI", "cpt", EncodeMD5("cpt")),
                new Collaborator("Arthur", "BINELLI", "ar", EncodeMD5("b")),
                new Collaborator("Thibal", "WITCZAK", "rh", EncodeMD5("rh")),
                new Collaborator("Floriab", "LE PALLEC", "f", EncodeMD5("l")),
                new Collaborator("Oubar", "MAYAKI", "o", EncodeMD5("m")),
                new Collaborator("Nathon", "BONNARD", "n", EncodeMD5("b"))
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
				new Mission("Mission A", DateTime.Today, new DateTime(2019, 5, 1), MissionStatus.IN_PROGRESS),
				new Mission("Mission B", new DateTime(2019, 2, 9), new DateTime(2019, 3, 1), MissionStatus.PLANNED),
				new Mission("Mission C", new DateTime(2018, 12, 25), new DateTime(2018, 12, 26), MissionStatus.CANCELED),
				new Mission("Mission D", new DateTime(2019, 2, 25), new DateTime(2019, 2, 26), MissionStatus.COMPLETED),
				new Mission("Mission E", new DateTime(2019, 2, 6), new DateTime(2019, 3, 1), MissionStatus.PLANNED),
				new Mission("Mission F", new DateTime(2019, 1, 9), new DateTime(2019, 11, 1), MissionStatus.PLANNED),
				new Mission("Mission G", new DateTime(2019, 1, 2), new DateTime(2019, 1, 4), MissionStatus.IN_PROGRESS)
			};


			/*
			 * Adding every lists above to the database
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

			// Assign missions to collaborators
			GetCollaborator("Arthur", "BINELLI").Missions.Add(GetMission("Mission A"));
            GetCollaborator("Arthur", "BINELLI").Missions.Add(GetMission("Mission B"));
            GetCollaborator("Arthur", "BINELLI").Missions.Add(GetMission("Mission C"));
            GetCollaborator("Arthur", "BINELLI").Missions.Add(GetMission("Mission E"));

			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission A"));
			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission B"));
			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission C"));
			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission D"));
			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission E"));
			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission F"));
			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission G"));

			//Adrien is coll2, Adrien gets all the missions because he is admin
			//GetCollaborator("Adrien", "LAVILLONNIERE").Missions = Missions;
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission A"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission B"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission C"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission D"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission E"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission F"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission G"));

			Missions[0].ChiefId = GetCollaborator("Morgan", "FEURTE").Id;
			Missions[1].ChiefId = GetCollaborator("Morgan", "FEURTE").Id;
			Missions[2].ChiefId = GetCollaborator("Morgan", "FEURTE").Id;
			Missions[3].ChiefId = GetCollaborator("Minh", "NGUYEN").Id;
			Missions[4].ChiefId = GetCollaborator("Minh", "NGUYEN").Id;
			Missions[5].ChiefId = GetCollaborator("Minh", "NGUYEN").Id;
			Missions[6].ChiefId = GetCollaborator("Minh", "NGUYEN").Id;

			Db.SaveChanges();

            // Create some expense reports 
            //TODO bizarre : j'ajoute les expense au coll3, alors que c'est le coll 1 qui a des missions. 
            // et ça marche. Par contre si j'enlevais une mission au coll1, alors y a nullpointer.
			// TODO : je crois que ça a été résolu.
            ExpenseReports = new List<ExpenseReport>() {
                new ExpenseReport(GetCollaborator("Arthur", "BINELLI"), Month.DECEMBER, 2018, ExpenseReportStatus.PENDING_APPROVAL_2),
				new ExpenseReport(GetCollaborator("Floriab", "LE PALLEC"), Month.DECEMBER, 2018, ExpenseReportStatus.PENDING_APPROVAL_1),
				new ExpenseReport(GetCollaborator("Oubar", "MAYAKI"), Month.JANUARY, 2019, ExpenseReportStatus.UNSENT),
                new ExpenseReport(GetCollaborator("Jeffrey", "GONCALVES"), Month.FEBRUARY, 2018, ExpenseReportStatus.PENDING_APPROVAL_2),
                new ExpenseReport(GetCollaborator("Jeffrey", "GONCALVES"), Month.NOVEMBER, 2018, ExpenseReportStatus.UNSENT)
            };

			Db.SaveChanges();

            ExpenseLines = new List<ExpenseLine>() {
                new ExpenseLine(GetMission("Mission A"), LineType.HOTEL, GetCollaborator("Minh", "NGUYEN"), "T Tower", 1000.0f, new DateTime(2019, 3, 4), "trumptower.pdf"),
                new ExpenseLine(GetMission("Mission B"), LineType.RESTAURANT, GetCollaborator("Jeffrey", "GONCALVES"), "Tower restaurant", 8000.0f, new DateTime(2019, 1, 2), "restau1.pdf"),
                new ExpenseLine(GetMission("Mission C"), LineType.TAXI, GetCollaborator("Jeffrey", "GONCALVES"), "Jafar Taxi", 15.98f, new DateTime(2019, 3, 4), "taxig7.pdf"),
                new ExpenseLine(GetMission("Mission E"), LineType.OTHER, GetCollaborator("Jeffrey", "GONCALVES"), "Jafar Other", 80010.0f, new DateTime(2019, 1, 2), "russia.pdf")
            };

			//lignes du frais 1 (arthur)
			foreach (ExpenseLine el in ExpenseLines) {ExpenseReports[0].AddLine(el);}

			//ligne de flo ?
			ExpenseReports[1].AddLine(new ExpenseLine(GetMission(6), LineType.RESTAURANT, "Pepperoni Pizza", 10.0f, new DateTime(2019, 1, 7), "pizza.pdf"));
			ExpenseReports[1].AddLine(new ExpenseLine(GetMission(2), LineType.HOTEL, "Pepperoni Florian", 10.0f, new DateTime(2019, 1, 8), "hotelflo.pdf"));

			//frais 4 (jeffrey). Jeffrey n'a aucune mission à effectuer actuellement
			ExpenseReports[3].AddLine(new ExpenseLine(GetMission(1), LineType.RESTAURANT, GetCollaborator("Jeffrey", "GONCALVES"), "Simon Burger", 10.0f, new DateTime(2019, 1, 5), "trumpburger.pdf"));
			ExpenseReports[3].AddLine(new ExpenseLine(GetMission(3), LineType.HOTEL, GetCollaborator("Minh", "NGUYEN"), "Jafar Hotel", 10.0f, new DateTime(2019, 1, 5), "hotel.pdf"));

			//foreach (ExpenseReport er in ExpenseReports) {
			//    Db.ExpenseReports.Add(er);
			//}

			Db.SaveChanges();
        }

		// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

		/*
		 * ------------------------------------------------------------
		 * Getters, setters and helpers--------------------------------
		 * ------------------------------------------------------------
		 */

		// Check if a collaborator is a ChiefValidator of a given Mission
		// Donc faut vérifier que la mission a un service,
		// que le service a une liste de personnes et que la liste de personne contient un cds

		public static bool CheckChiefValidator(Collaborator chief, Mission mission)
		{
			//inutile?
			return false;

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

        public void CreateCollaborator(string firstname, string lastname, string login, string password) {
            Db.Collaborators.Add(new Collaborator(firstname, lastname, login, password));
            Db.SaveChanges();
        }

        // Missions
        public Mission GetMission(int id) {
            return Db.Missions.SingleOrDefault(m => m.Id == id);
        }

        public Mission GetMission(string name) {
            return Db.Missions.SingleOrDefault(m => m.Name == name);
        }

        // ExpenseReports
        public List<ExpenseReport> GetExpenseReports() {
            return Db.ExpenseReports.ToList();
        }

		public ExpenseReport GetExpenseReport(int id) {
            return Db.ExpenseReports.SingleOrDefault(e => e.Id == id);
        }

        public void CreateExpenseReport(Collaborator coll, Month month, int year) {
            Db.ExpenseReports.Add(new ExpenseReport(coll, month, year));
            Db.SaveChanges();
        }

        public void ClearExpenseLines(ExpenseReport er) {
            List<ExpenseLine> ELs = new List<ExpenseLine>();

            // Remove EL from list (iterate backward to be safe)
            for (int i = er.ExpenseLines.Count - 1; i >= 0; i--) {
                ELs.Add(er.ExpenseLines[i]);
                er.RemoveLine(er.ExpenseLines[i]);
            }

            // Destroy all ELs
            for (int i = ELs.Count - 1; i >= 0; i--) {
                ExpenseLine toRemove = ELs[i];
                //ExpenseLine toRemove = Db.ExpenseLines.SingleOrDefault(el => el.Id == ELs[i].Id);
                if (toRemove != null)
                    Db.ExpenseLines.Remove(toRemove);
            }
            Db.SaveChanges();
        }

        // Login
        public Collaborator Login(string name, string pass)
		{
			string passEncoded = EncodeMD5(pass);
			return Db.Collaborators.FirstOrDefault(u => u.Login == name && u.Password == passEncoded);
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

        // Helper
        public string EncodeMD5(string pass) {
            string passSalt = "ChevalDeMetal" + pass + "Casablanca";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(passSalt)));
        }

        public void SaveChanges() {
            Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}