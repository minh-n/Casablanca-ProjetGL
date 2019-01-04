using System;
using System.Collections.Generic;
using Casablanca.Models.ExpenseReports;

namespace Casablanca.Models.Database {
    interface IDal : IDisposable
    {
        // Collaborators
        List<Collaborator> GetCollaborators();
        Collaborator GetCollaborator(int id);

        // Missions 
        Mission GetMission(int id);

        // ExpenseReports
        List<ExpenseReport> GetExpenseReports();
        ExpenseReport GetExpenseReport(int id);
    }
}
