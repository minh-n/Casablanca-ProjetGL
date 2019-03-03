using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Casablanca.Models.ExpenseReports;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using Casablanca.Models.Leaves;
using System.Collections.Concurrent;

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
        private ConcurrentBag<Notification> Notifications { get; set; }

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

			#region Create services

			Services = new List<Service> {
                new Service("Informatique d'entreprise"),	//0
                new Service("Direction"),					//1
                new Service("Compta"),						//2
                new Service("RH"),							//3
                new Service("Commerce, Coloriages et Consultation"),		//4
                new Service("Gestion des flux animaliers"),	//5
				new Service("Positionnement stratégique digitalisé")	//6
			};

			#endregion

			#region Create collaborators
			Collaborators = new List<Collaborator> {
                new Collaborator("Morgan", "FEURTE", "mo", EncodeMD5("go")),
                new Collaborator("Minh", "NGUYEN", "cds", EncodeMD5("cds")),
                new Collaborator("Adrien", "LAVILLONNIERE", "adm", EncodeMD5("adm")),
                new Collaborator("Jeffrey", "GONCALVES", "j", EncodeMD5("g")),
                new Collaborator("Yao", "SHI", "cpt", EncodeMD5("cpt")),
                new Collaborator("Arthur", "BINELLI", "a", EncodeMD5("b")),
                new Collaborator("Thibal", "WITCZAK", "t", EncodeMD5("w")),
                new Collaborator("Floriab", "LE PALLEC", "f", EncodeMD5("lp")),
                new Collaborator("Oubar", "MAYAKI", "o", EncodeMD5("m")),
                new Collaborator("Nathon", "BONNARD", "n", EncodeMD5("b")),
				new Collaborator("Momo", "BELDI", "momo", EncodeMD5("b")),
				new Collaborator("Mathias", "BAZON", "mat", EncodeMD5("baz")),
				new Collaborator("Albon", "DESCOTTES", "alb", EncodeMD5("d")),
				new Collaborator("Arnaud", "GAY-BAUER", "ar", EncodeMD5("no")),
				new Collaborator("Coronton", "MONSCOUR", "coco", EncodeMD5("mons")),
				new Collaborator("Olodie", "LOM", "lom", EncodeMD5("olo")),
				new Collaborator("Kevon", "LOCOSTE", "kvn", EncodeMD5("kvn")) 

			};
			#endregion 

			#region Add coll to services

			// Add coll to services

			AddToService(2, 0); //mow le cds compta
			AddToService(1, 1); //minh le cds PDG
            AddToService(1, 2); //dadri le user + admin

            AddToService(0, 3); //jaff le CDS normal
            AddToService(2, 4); //yao le compta normal


            AddToService(3, 6); //thib le RH normal
            AddToService(3, 7); //flo le RH normal

            AddToService(4, 8); //oubar le cds consultation
			AddToService(4, 9); //nathon le user normal

			AddToService(3, 10); //momo le CDS RH

			AddToService(5, 11); //mathias le cds animaux
			AddToService(5, 5); //arthur  user animaux

			AddToService(4, 12); //alban user consultation
			AddToService(0, 13); //arno user info

			AddToService(0, 14); //corentin user direction

			AddToService(2, 15); //elodie user compta

			AddToService(6, 16); //kvn cds Positionnement stratégique digitalisé

			#endregion

			#region Add admin roles

			// Add admin role to Adrien
			Collaborators[2].Role = Roles.ADMIN;

			//Add CHIEF roles to other people
			Collaborators[3].Role = Roles.CHIEF; //Jeffrey is now CDS info
			Collaborators[1].Role = Roles.CHIEF; //Minh is now CDS Direction (PDG)
			Collaborators[0].Role = Roles.CHIEF; //Morgan is now CDS Compta
			Collaborators[11].Role = Roles.CHIEF; //Mathias is now CDS Animaux
			Collaborators[8].Role = Roles.CHIEF; //Oubar is now CDS Consultation
			Collaborators[16].Role = Roles.CHIEF; //KVN is now CDS Digital
			Collaborators[10].Role = Roles.CHIEF; //Momo is DRH

            #endregion

            #region Initialize notifications
            Notifications = new ConcurrentBag<Notification>()
            {
                new Notification(Collaborators[2], Collaborators[2], NotificationType.INFORMATION, NotificationResult.VALIDATION),
                new Notification(Collaborators[1], Collaborators[2], NotificationType.INFORMATION, NotificationResult.REFUSAL),
                new Notification(Collaborators[3], Collaborators[1], NotificationType.INFORMATION, NotificationResult.VALIDATION),
                new Notification(Collaborators[4], Collaborators[4], NotificationType.INFORMATION, NotificationResult.VALIDATION),
                new Notification(Collaborators[5], Collaborators[5], NotificationType.INFORMATION, NotificationResult.VALIDATION)
            };

            #endregion

            #region Create some missions
            Missions = new List<Mission> {
				new Mission("Mission A", DateTime.Today, new DateTime(2019, 5, 1), MissionStatus.IN_PROGRESS),
				new Mission("Mission B", new DateTime(2019, 2, 9), new DateTime(2019, 3, 1), MissionStatus.PLANNED),
				new Mission("Mission C", new DateTime(2018, 12, 25), new DateTime(2018, 12, 26), MissionStatus.CANCELED),
				new Mission("Mission D", new DateTime(2018, 2, 25), new DateTime(2018, 2, 26), MissionStatus.COMPLETED),
				new Mission("Mission E", new DateTime(2019, 2, 6), new DateTime(2019, 3, 1), MissionStatus.PLANNED),
				new Mission("Mission F", new DateTime(2019, 1, 9), new DateTime(2019, 11, 1), MissionStatus.PLANNED),
				new Mission("Mission G", new DateTime(2019, 1, 2), new DateTime(2019, 1, 4), MissionStatus.IN_PROGRESS),
				new Mission("Mission H", new DateTime(2019, 2, 11), MissionStatus.IN_PROGRESS)

			};
			#endregion

			#region Adding every lists above to the database

			foreach (Service s in Services) {
                Db.Services.Add(s);
            }

            foreach (Collaborator c in Collaborators) {
                Db.Collaborators.Add(c);
            }

            foreach (Mission m in Missions) {
                Db.Missions.Add(m);
            }

            foreach (Notification n in Notifications)
            {
                Db.Notifications.Add(n);
            }

            Db.SaveChanges();
			#endregion

			#region Assign missions to collaborators and setting chiefId

			GetCollaborator("Arthur", "BINELLI").Missions.Add(GetMission("Mission A"));
            GetCollaborator("Arthur", "BINELLI").Missions.Add(GetMission("Mission C"));
            GetCollaborator("Arthur", "BINELLI").Missions.Add(GetMission("Mission E"));

			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission A"));
			GetCollaborator("Jeffrey", "GONCALVES").Missions.Add(GetMission("Mission F"));
			
			GetCollaborator("Nathon", "BONNARD").Missions.Add(GetMission("Mission B"));
			GetCollaborator("Nathon", "BONNARD").Missions.Add(GetMission("Mission E"));
			GetCollaborator("Nathon", "BONNARD").Missions.Add(GetMission("Mission F"));

			GetCollaborator("Floriab", "LE PALLEC").Missions.Add(GetMission("Mission D"));
			GetCollaborator("Floriab", "LE PALLEC").Missions.Add(GetMission("Mission H"));

			//Adrien is coll2, Adrien gets all the missions because he is admin
			//GetCollaborator("Adrien", "LAVILLONNIERE").Missions = Missions;
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission A"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission B"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission C"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission D"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission E"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission F"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission G"));
			GetCollaborator("Adrien", "LAVILLONNIERE").Missions.Add(GetMission("Mission H"));

			GetCollaborator("Morgan", "FEURTE").Missions.Add(GetMission("Mission A"));
			GetCollaborator("Morgan", "FEURTE").Missions.Add(GetMission("Mission B"));
			GetCollaborator("Morgan", "FEURTE").Missions.Add(GetMission("Mission C"));


			// Setting chief ID for each mission
			Missions[0].ChiefId = GetCollaborator("Morgan", "FEURTE").Id;

			Missions[1].ChiefId = GetCollaborator("Minh", "NGUYEN").Id;

			Missions[2].ChiefId = GetCollaborator("Jeffrey", "GONCALVES").Id;

			Missions[3].ChiefId = GetCollaborator("Oubar", "MAYAKI").Id;
			Missions[4].ChiefId = GetCollaborator("Oubar", "MAYAKI").Id;
			Missions[5].ChiefId = GetCollaborator("Oubar", "MAYAKI").Id;

			Missions[6].ChiefId = GetCollaborator("Mathias", "BAZON").Id;

			Missions[7].ChiefId = GetCollaborator("Momo", "BELDI").Id;

			Db.SaveChanges();

			#endregion

			#region Create expense reports 

			ExpenseReports = new List<ExpenseReport>() {
                new ExpenseReport(GetCollaborator("Arthur", "BINELLI"), Month.DECEMBER, 2018, ExpenseReportStatus.PENDING_APPROVAL_2),
				new ExpenseReport(GetCollaborator("Floriab", "LE PALLEC"), Month.DECEMBER, 2018, ExpenseReportStatus.PENDING_APPROVAL_1),
				new ExpenseReport(GetCollaborator("Oubar", "MAYAKI"), Month.JANUARY, 2019, ExpenseReportStatus.UNSENT),
            };

			Db.SaveChanges();

            // by Yao
            ExpenseLines = new List<ExpenseLine>() {
                new ExpenseLine(GetMission("Mission A"), LineType.HOTEL, "Minh NGUYEN", "T Tower", 1000.0f, new DateTime(2019, 3, 4), "ProjetGL-recueil des besoins.pdf"), // by Yao, "trumptower.pdf"
                new ExpenseLine(GetMission("Mission B"), LineType.RESTAURANT, "Jeffrey GONCALVES", "Tower restaurant", 8000.0f, new DateTime(2019, 1, 2), "Cours Polytech - Paris XI -Atelier GL Cours1 v5.pdf"), // by Yao, "restau1.pdf"
                new ExpenseLine(GetMission("Mission C"), LineType.TAXI, "Jeffrey GONCALVES", "Jafar Taxi", 15.98f, new DateTime(2019, 3, 4), ""), //by Yao, "taxig7.pdf"
                new ExpenseLine(GetMission("Mission E"), LineType.OTHER, "Jeffrey GONCALVES", "Jafar Other", 80010.0f, new DateTime(2019, 1, 2), "Cours Polytech - Paris XI -Atelier GL COURS RAPPELS V4.pdf") //by Yao, "russia.pdf"
            };

			// Lignes de l'ER 1 (arthur)
			foreach (ExpenseLine el in ExpenseLines) {ExpenseReports[0].AddLine(el);}

			// Ligne de l'ER 2 (flo) 
			ExpenseReports[1].AddLine(new ExpenseLine(GetMission(6), LineType.RESTAURANT, "Pepperoni Pizza", 10.0f, new DateTime(2019, 1, 7), "pizza.pdf")); // by Yao, "pizza.pdf"
			ExpenseReports[1].AddLine(new ExpenseLine(GetMission(2), LineType.HOTEL, "Pepperoni Florian", 10.0f, new DateTime(2019, 1, 8), "hotelflo.pdf")); // by Yao, "hotelflo.pdf"

			// Lignes de l'ER 4 (jeffrey). Jeffrey n'a aucune mission à effectuer actuellement
			//ExpenseReports[3].AddLine(new ExpenseLine(GetMission(1), LineType.RESTAURANT, "Jeffrey GONCALVES", "Simon Burger", 10.0f, new DateTime(2019, 1, 5), "trumpburger.pdf"));
			//ExpenseReports[3].AddLine(new ExpenseLine(GetMission(3), LineType.HOTEL, "Minh NGUYEN", "Jafar Hotel", 10.0f, new DateTime(2019, 1, 5), "hotel.pdf"));

			//foreach (ExpenseReport er in ExpenseReports) {
			//    Db.ExpenseReports.Add(er);
			//}

			Db.SaveChanges();
			#endregion

			#region Create leaves

			List<Leave> leaves = new List<Leave>()
			{
				new Leave(LeaveStatus.PENDING_APPROVAL_1, LeaveType.OTHER, GetCollaborator("Morgan", "FEURTE"), new DateTime(2019, 2, 05), new DateTime(2019, 2, 15), "Matin", "Matin"),
				new Leave(LeaveStatus.REFUSED, LeaveType.PAID, GetCollaborator("Oubar", "MAYAKI"), new DateTime(2019, 2, 3), new DateTime(2019, 2, 6), "Matin", "Matin"),
				//new Leave(LeaveStatus.APPROVED, LeaveType.PAID, GetCollaborator("Oubar", "MAYAKI"), new DateTime(2019, 1, 4), new DateTime(2019, 1, 12)),

				new Leave(LeaveStatus.PENDING_APPROVAL_1, LeaveType.PAID, GetCollaborator("Floriab", "LE PALLEC"), new DateTime(2019, 2, 9), new DateTime(2019, 2, 11), "Matin", "Matin"),

				new Leave(LeaveStatus.APPROVED, LeaveType.PAID, GetCollaborator("Nathon", "BONNARD"), new DateTime(2019, 2, 27), new DateTime(2019, 3, 1), "Matin", "Après-midi"),


				new Leave(LeaveStatus.PENDING_APPROVAL_1, LeaveType.RTT, GetCollaborator("Nathon", "BONNARD"), new DateTime(2019, 3, 19), new DateTime(2019, 3, 21), "Après-midi", "Après-midi"),

				//new Leave(LeaveStatus.PENDING_APPROVAL_2, LeaveType.RTT, GetCollaborator("Momo", "BELDI"), new DateTime(2019, 2, 1), new DateTime(2019, 2, 8)),
				//new Leave(LeaveStatus.APPROVED, LeaveType.PAID, GetCollaborator("Thibal", "WITCZAK"), new DateTime(2019, 1, 05), new DateTime(2019, 2, 15)),
				new Leave(LeaveStatus.PENDING_APPROVAL_1, LeaveType.PAID, GetCollaborator("Coronton", "MONSCOUR"), new DateTime(2019, 2, 14), new DateTime(2019, 2, 25), "Après-midi", "Après-midi"),

				new Leave(LeaveStatus.REFUSED, LeaveType.RTT, GetCollaborator("Thibal", "WITCZAK"), new DateTime(2018, 12, 05), new DateTime(2019, 1, 15), "Matin", "Matin"),
				new Leave(LeaveStatus.APPROVED, LeaveType.PAID, GetCollaborator("Minh", "NGUYEN"), new DateTime(2019, 3, 4), new DateTime(2019, 9, 18), "Après-midi", "Matin") 

			};
			
			foreach (Leave l in leaves)
			{
				Db.Leaves.Add(l);
			}

			GetCollaborator("Thibal", "WITCZAK").NbPaid = 5;
			GetCollaborator("Thibal", "WITCZAK").NbRTT = 10;

			GetCollaborator("Oubar", "MAYAKI").NbPaid = 15;
			GetCollaborator("Oubar", "MAYAKI").NbRTT = 18;

			GetCollaborator("Nathon", "BONNARD").NbPaid = 5;
			GetCollaborator("Nathon", "BONNARD").NbRTT = 5;

			GetCollaborator("Floriab", "LE PALLEC").NbPaid = 11;
			GetCollaborator("Floriab", "LE PALLEC").NbRTT = 9;

			GetCollaborator("Minh", "NGUYEN").NbPaid = 100;
			GetCollaborator("Minh", "NGUYEN").NbRTT = 900;


			Db.SaveChanges();

			#endregion
		}

		// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

		/*
		 * ------------------------------------------------------------
		 * Getters, setters and helpers--------------------------------
		 * ------------------------------------------------------------
		 */

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

		public void RemoveCollaborator(int id)
		{
			Db.Collaborators.Remove(GetCollaborator(id));
			Db.SaveChanges();

		}


		// Missions

		public void AddMission(Mission miss)
		{
			Db.Missions.Add(miss);
			Db.SaveChanges();
		}

		public Mission GetMission(int id) {
            return Db.Missions.SingleOrDefault(m => m.Id == id);
        }

        public Mission GetMission(string name) {
            return Db.Missions.SingleOrDefault(m => m.Name == name);
        }

		public List<Mission> GetMissions()
		{
			return Db.Missions.ToList();
		}

		public int CreateMission(int id)
		{
			Mission tempMiss = new Mission { ChiefId = id, StartDate = DateTime.Now, EndDate = DateTime.Now, Status = MissionStatus.PLANNED};
			Db.Missions.Add(tempMiss);
			Db.SaveChanges();
			return tempMiss.Id;
		}

		// ExpenseReports
		public List<ExpenseReport> GetExpenseReports() {

            List<ExpenseReport> expenseReports = new List<ExpenseReport>();
            foreach (ExpenseReport er in Db.ExpenseReports.ToList())
            {
                if (!er.IsAdvance)
                    expenseReports.Add(er);
            }

            return expenseReports;
        }

		public ExpenseReport GetExpenseReport(int id) {
            return Db.ExpenseReports.SingleOrDefault(e => e.Id == id);
        }

        public int CreateExpenseReport(Collaborator coll, Month month, int year, bool isAdvance) {
			ExpenseReport tempER = new ExpenseReport(coll, month, year, isAdvance);
			Db.ExpenseReports.Add(tempER);
            Db.SaveChanges();
			return tempER.Id;
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

		// Leaves
		public List<Leave> GetLeaves()
		{
			return Db.Leaves.ToList();
		}

		public Leave GetLeave(int id)
		{
			return Db.Leaves.SingleOrDefault(s => s.Id == id);
		}

		public void CreateLeave(Leave temp)
		{

			Db.Leaves.Add(temp);
		
			Db.SaveChanges();
		}

        // Notifications
        public List<Notification> GetNotifications()
        {
            return Db.Notifications.ToList();
        }

        public Notification GetNotifications(int id)
        {
            return Db.Notifications.SingleOrDefault(c => c.Id == id);
        }

        public List<Notification> GetNotifications(Collaborator receiver)
        {
            List<Notification> r = new List<Notification>();
            foreach (Notification n in Db.Notifications)
            {
                if(n.Receiver != null)
                {
                    if (n.Receiver.Id == receiver.Id)
                        r.Add(n);
                }                
            }
            return r;
        }

        public void AddNotification(Notification not)
        {
            Db.Notifications.Add(not);
            Db.SaveChanges();
        }

        public void DeleteNotification(Notification not)
        {
            Db.Notifications.Remove(not);
            Db.SaveChanges();
        }

        public void DeleteNotification(List<Notification> notifs)
        {
            foreach (Notification not in notifs)
            {
                Db.Notifications.Remove(not);
            }

            Db.SaveChanges();
        }



        //Advances
        public int CreateAdvance(Collaborator coll, bool isAdvance)
        {
            ExpenseReport tempER = new ExpenseReport(coll, Month.NONE,1990, isAdvance);
            Db.ExpenseReports.Add(tempER);
            Db.SaveChanges();
            return tempER.Id;
        }

        public List<ExpenseReport> GetAdvances()
        {
            List<ExpenseReport> advances = new List<ExpenseReport>();
            foreach (ExpenseReport er in Db.ExpenseReports.ToList())
            {
                if (er.IsAdvance)
                    advances.Add(er);
            }

            return advances;
        }

        public ExpenseReport GetAdvance(int id)
        {
            return Db.ExpenseReports.SingleOrDefault(e => (e.Id == id && e.IsAdvance));
        }

        public void TransferFromAdvanceToEr(int id)
        {
            List<ExpenseReport> advances = GetAdvances();
            ExpenseReport expenseReport = GetExpenseReport(id);

            foreach (ExpenseReport er in advances)
            {
                foreach (ExpenseLine el in er.ExpenseLines.ToList())
                {
                    if (el.Validated /*&& el.Mission.Status == MissionStatus.COMPLETED*/) //To check
                    {
                        expenseReport.Collaborator.AdvanceCost += el.Cost;
                        expenseReport.AddLine(el);
                        er.RemoveLine(el);
                    }
                }

                if (er.ExpenseLines.Count == 0)
                {
                    Db.ExpenseReports.Remove(er);
                    Db.SaveChanges();
                }
            }

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