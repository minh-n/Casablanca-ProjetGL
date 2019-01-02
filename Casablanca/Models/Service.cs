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

        public Service(string serviceName, List<Collaborator> collList, Collaborator chief) {
            ServiceName = serviceName;
            CollList = collList;
            Chief = chief;
        }

        public Service() {
        }
    }
}