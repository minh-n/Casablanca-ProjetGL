using Casablanca.Models.ExpenseReports;
using System.Data.Entity;

namespace Casablanca.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<ExpenseReport> ExpenseReports { get; set; }
		public DbSet<Service> Services { get; set; }

	}
}