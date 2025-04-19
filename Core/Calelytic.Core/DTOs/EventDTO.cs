using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Enums;
using Calelytic.Core.Models;

namespace Calelytic.Core.DTOs
{
    public class EventDTO
    {
        public int EventId { get; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public EventType? Type { get; set; } = EventType.OneTime;
        public Occurrence? Recurrence { get; set; } = null;
        public DateTime DateOfEvent { get; set; }
        public TimeSpan? Duration { get; set; }
        public List<Reminder>? Reminders { get; set; } = new();
        public List<Guid>? Participants { get; set; } = new();
        public Guid UpdatedBy { get; set; }
        public bool HasReminder { get; set; } = false;
        public bool IsAllDay { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
