using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Enums;

namespace Calelytic.Core.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public TimeSpan TimeBeforeEvent { get; set; }
        public ReminderMethod Method { get; set; }
        public int EventID { get; set; } //Foreign Key reference
        public Event? Event { get; set; }// Navigation Property
    }
}
