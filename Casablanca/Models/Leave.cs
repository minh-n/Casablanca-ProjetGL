using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public enum LeaveStatus
    {
        APPROVED,
        PENDING
    }

    public class Leave
    {
        public int id { get; set; }
        public string eventName;
        public string status;
        public string color;
        public DateTime eventTime;

        public Leave(string eventName, LeaveStatus status, DateTime eventTime)
        {
            this.eventName = eventName;
            this.eventTime = eventTime;

            if (status == LeaveStatus.APPROVED)
            {
                this.color = "green";
                this.status = "Congé accepté";
            }
            else
            {
                this.color = "orange";
                this.status = "Demande de congé";
            }
        }
    }
}