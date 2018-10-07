using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public class CollaboratorList
    {
        public IList<Collaborator> collaborateurs { get; set; }

        public CollaboratorList()
        {
            collaborateurs = new List<Collaborator>();
        }
    }
}