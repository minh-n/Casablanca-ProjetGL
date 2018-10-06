using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public class Collaborateurs
    {
        public IList<Collaborateur> collaborateurs { get; set; }

        public Collaborateurs()
        {
            collaborateurs = new List<Collaborateur>();
        }
    }
}