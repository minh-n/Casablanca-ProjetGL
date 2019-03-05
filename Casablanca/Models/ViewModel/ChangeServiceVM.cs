using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models.ViewModel
{
    public class ChangeServiceVM
    {
        public Collaborator Collaborator { get; set; }
        public List<Service> Services { get; set; }

        public ChangeServiceVM(Collaborator collaborator, List<Service> services)
        {
            Collaborator = collaborator;
            Services = services;
        }

        public ChangeServiceVM()
        {

        }
    }
}