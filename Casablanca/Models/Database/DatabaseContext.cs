using System.Data.Entity;

namespace Casablanca.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<Leave> Leaves { get; set; }
    }
}