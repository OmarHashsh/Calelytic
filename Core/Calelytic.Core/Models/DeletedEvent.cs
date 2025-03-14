using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calelytic.Core.Models
{
     public class DeletedEvent
    {
        public int Id { get; set; } // Unique identifier for deleted event
        public int EventId { get; set; } // Foreign key reference to Event
        public DateTime DeletedDate { get; set; } // Time of deletion (UTC)
        public string? Reason { get; set; } // Optional reason for deletion

        // Navigation property (for Entity Framework or ORM use)
        public Event? EventData { get; set; } // Reference to the original event

        public DeletedEvent(int eventId, string? reason = null)
        {
            EventId = eventId;
            DeletedDate = DateTime.UtcNow;
            Reason = reason;
        }
        public DeletedEvent() { }

    }
}
