using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Enums;

namespace Calelytic.Core.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public EventType? Type { get; set; } = EventType.OneTime;
        public DateTime DateOfEvent { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsAllDay { get; set; } = false;
        public bool HasReminder { get; set; } = false;
        public Occurrence? Recurrence { get; set; } = null;
        public List<Reminder> Reminders { get; set; } = new();

        public Event()
        {
            Duration = TimeSpan.FromSeconds(1);
        }
    }

}
