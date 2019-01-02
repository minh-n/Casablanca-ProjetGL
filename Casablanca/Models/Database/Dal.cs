using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models.Database
{
    public class Dal : IDal
    {
        private DatabaseContext db { get; set; }

        public Dal()
        {
            db = new DatabaseContext();
        }

        public void InitializeDatabase() {
            CreateCollaborator("Morgan", "FEURTE");
            CreateCollaborator("Minh", "NGUYEN");
            CreateCollaborator("Adrien", "LAVILLONNIERE");
            CreateCollaborator("Jeffrey", "GONCALVES");
            CreateCollaborator("Yao", "SHI");
        }

        public List<Collaborator> GetCollaborators()
        {
            return db.Collaborators.ToList();
        }

        public void CreateCollaborator(string firstname, string lastname)
        {
            db.Collaborators.Add(new Collaborator(firstname, lastname));
            db.SaveChanges();
        }

        public void ResetCollaborators()
        {
            db.Collaborators.RemoveRange(db.Collaborators);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}