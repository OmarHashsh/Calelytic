using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Enums;

namespace Calelytic.Core.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public EventType? Type { get; set; } = EventType.OneTime;
        public Occurrence? Recurrence { get; set; } = null; // the difference between this var and the one above it is that "Type" specifies whether or not a particular event has recurrence. "Recurrence", on the other hand, specifies how often this event repeats.
        public DateTime DateOfEvent { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsAllDay { get; set; } = false;
        public bool HasReminder { get; set; } = false;
        public List<Reminder> Reminders { get; set; } = new();
        public List<Guid> Participants { get; set; } = new();
        public List<Calendar> Calendars { get; set; } // this list contains the calendars that can access this event. Useful for filtering.
        //Logging Vars
        public DateTime? UpdatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public Event()
        {
            Duration = TimeSpan.FromSeconds(1);
        }
    }

}
