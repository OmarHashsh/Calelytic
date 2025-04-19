using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Enums;
using Calelytic.Core.Models;

namespace Calelytic.Core.DTOs
{
    public class ReminderDTO
    {
        public int ReminderId { get; }
        public TimeSpan TimeBeforeEvent { get; set; }
        public ReminderMethod Method { get; set; }
        public int EventId { get; set; }
    }
}
