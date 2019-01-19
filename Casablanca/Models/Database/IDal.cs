using System;
using System.Collections.Generic;
using Casablanca.Models.ExpenseReports;

namespace Casablanca.Models.Database {
    interface IDal : IDisposable
    {
        // Collaborators
        List<Collaborator> GetCollaborators();
        Collaborator GetCollaborator(int id);
        Collaborator GetCollaborator(string idString);
        Collaborator GetCollaborator(string firstname, string lastname);
        void CreateCollaborator(string firstname, string lastname, string login, string password);

        // Missions 
        Mission GetMission(int id);
		Mission GetMission(string name);
		List<Mission> GetMissions();

		// ExpenseReports
		List<ExpenseReport> GetExpenseReports();
        ExpenseReport GetExpenseReport(int id);
		Collaborator Login(string login, string password);
        void CreateExpenseReport(Collaborator coll, Month month, int year);
        void ClearExpenseLines(ExpenseReport er);

        // Services
        List<Service> GetServices();
		Service GetService(int id);
		void AddToService(int serviceId, int collId);

		// Admin
		void SetCollaboratorAccount(int collId, string login, string pass);

        // Helper
        string EncodeMD5(string password);
        void SaveChanges();
		//bool CheckChiefValidator(Collaborator chief, Mission mission);
		//can't implement because static
	}
}
