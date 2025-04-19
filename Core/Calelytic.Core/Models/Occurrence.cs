using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Enums;

namespace Calelytic.Core.Models
{
    public class Occurrence
    {
        // Primary and Foreign Keys
        public int Id { get; set; }                      // Primary Key
        public int EventId { get; set; }                 // Foreign Key to Event

        // Core Recurrence Rules
        public OccurrenceFrequency Frequency { get; set; }
        public int Interval { get; set; }

        // Bitmask for days of the week recurrence
        public int DaysBitmask { get; set; }             // Binary mask stored in DB

        [NotMapped]
        public List<DayOfWeek> DaysOfTheWeek
        {
            get
            {
                var days = new List<DayOfWeek>();
                for (int i = 0; i < 7; i++)
                {
                    if ((DaysBitmask & (1 << i)) != 0)
                        days.Add((DayOfWeek)i);
                }
                return days;
            }
            set
            {
                DaysBitmask = 0;
                foreach (var day in value)
                {
                    DaysBitmask |= (1 << (int)day);
                }
            }
        }

        // Date boundaries for the occurrence rule
        public DateTime? OccurrenceStartDate { get; set; } = DateTime.Today;
        public DateTime? OccurrenceEndDate { get; set; }

        // Optional custom pattern rules (e.g. "2m+15d")
        public string? CustomPattern { get; set; }
        // I thought I needed another property to figure out when does the occurrence happen and when it ends but I can just use the ones from above.

        public Occurrence() { }
    }
}
