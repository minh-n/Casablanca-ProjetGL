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
		List<Mission> GetCollaboratorMissions(int collId);

		// ExpenseReports
		List<ExpenseReport> GetExpenseReports();
        ExpenseReport GetExpenseReport(int id);
		Collaborator Login(string login, string password);

		// Services
		List<Service> GetServices();
		Service GetService(int id);
		void AddToService(int serviceId, int collId);

		// Admin
		void SetCollaboratorAccount(int collId, string login, string pass);

        // Helper
        string EncodeMD5(string password);
    }
}
