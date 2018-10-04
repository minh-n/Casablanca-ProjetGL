using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public class Collaborateurs
    {
        public Collaborateurs()
        {
            collaborateurs = new List<Collaborateur>();
        }

        public IList<Collaborateur> collaborateurs { get; set; }
    }
}