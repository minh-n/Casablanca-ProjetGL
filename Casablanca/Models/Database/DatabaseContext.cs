using Casablanca.Models.ExpenseReports;
using Casablanca.Models.Leaves;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Casablanca.Models {
    public class DatabaseContext : DbContext
    {
        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<ExpenseReport> ExpenseReports { get; set; }
		public DbSet<Service> Services { get; set; }
        public DbSet<ExpenseLine> ExpenseLines { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}