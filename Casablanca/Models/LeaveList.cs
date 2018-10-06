using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casablanca.Models
{
    public class LeaveList
    {
        public IList<Leave> leaves { get; set; }

        public LeaveList()
        {
            leaves = new List<Leave>();
        }
    }
}