using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models {

    public class Service {
        [Key]
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public List<Collaborator> CollList { get; set; }
        public Collaborator Chief { get; set; }

        public Service(string serviceName) {
            ServiceName = serviceName;
            CollList = new List<Collaborator>();
            Chief = null;
        }


        public Service() {
        }
    }
}