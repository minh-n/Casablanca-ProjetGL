using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models {
    public enum MissionStatus {
        COMPLETED,
        IN_PROGRESS,
        PLANNED,
        CANCELED
    }

    public class Mission {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MissionStatus Status { get; set; }

        public List<Collaborator> CollList { get; set; }

        public Service Service { get; set; }

        public Mission() {

        }

        public Mission(string name, DateTime startDate, DateTime endDate, MissionStatus status, List<Collaborator> collList, Service service) {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            CollList = collList;
            Service = service;
        }

        public Mission(string name, DateTime startDate, DateTime endDate, MissionStatus status, Service service) : this(name) {
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            Service = service;
        }

        public Mission(string name, DateTime startDate, DateTime endDate, MissionStatus status) : this(name) {
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }

        public Mission(string name, Service service) {
            Name = name;
            StartDate = DateTime.Now;
            Status = MissionStatus.IN_PROGRESS;
            Service = service;
        }

        public Mission(string name) {
            Name = name;
            StartDate = DateTime.Now;
            Status = MissionStatus.IN_PROGRESS;
        }
    }
}