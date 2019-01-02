using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Casablanca.Models {
    public enum LeaveStatus {
        APPROVED,
        PENDING_APPROVAL,
        REFUSED
    }

    public class Leave {
        [Key]
        public int Id { get; set; }
        public string EventName;
        public string Status;
        public string Color;
        public DateTime EventTime;

        public Leave(string eventName, LeaveStatus status, DateTime eventTime) {
            this.EventName = eventName;
            this.EventTime = eventTime;

            if (status == LeaveStatus.APPROVED) {
                this.Color = "green";
                this.Status = "Congé accepté";
            }
            else {
                this.Color = "orange";
                this.Status = "Demande de congé";
            }
        }
    }
}