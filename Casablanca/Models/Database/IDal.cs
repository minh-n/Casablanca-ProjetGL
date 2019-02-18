using System;
using System.Collections.Generic;
using Casablanca.Models.ExpenseReports;
using Casablanca.Models.Leaves;

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
		void AddMission(Mission miss);
		Mission GetMission(int id);
		Mission GetMission(string name);
		List<Mission> GetMissions();
		int CreateMission(int id);

		// ExpenseReports
		List<ExpenseReport> GetExpenseReports();
        ExpenseReport GetExpenseReport(int id);
		Collaborator Login(string login, string password);
        int CreateExpenseReport(Collaborator coll, Month month, int year, bool isAdvance);
        void ClearExpenseLines(ExpenseReport er);

        // Advances
        int CreateAdvance(Collaborator coll, Month month, int year, bool isAdvance);
        List<ExpenseReport> GetAdvances();
        ExpenseReport GetAdvance(int id);
        void TransferFromAdvanceToEr(int id);

        // Services
        List<Service> GetServices();
		Service GetService(int id);
		void AddToService(int serviceId, int collId);

		// Admin
		void SetCollaboratorAccount(int collId, string login, string pass);

		// Leave
		List<Leave> GetLeaves();
		Leave GetLeave(int id);
		void CreateLeave(Leave temp);

		// Helper
		string EncodeMD5(string password);
        void SaveChanges();
		//bool CheckChiefValidator(Collaborator chief, Mission mission);
		//can't implement because static
	}
}
