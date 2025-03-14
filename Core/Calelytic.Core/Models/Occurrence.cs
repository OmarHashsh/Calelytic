using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Enums;

namespace Calelytic.Core.Models
{
    public class Occurrence
    {
        public OccurrenceFrequency Frequency { get; set; }
        public int Interval { get; set; }
        public List<DayOfWeek> DaysOfTheWeek { get; }
        public DateTime? OccurrenceStartDate {  get; set; } // example usage: I want to start going to the doctor every 3 months in 2026.
        public DateTime? OccurrenceEndDate { get; set; } // sets up a date for the end of the occurrence. if we use the example from one line above, we can say: I want to start visiting the doctor once every quarter only for 2026. I don't want it to extend to 2027...
        public string? CustomPattern { get; set; }

        public Occurrence()
        {
            DaysOfTheWeek = new List<DayOfWeek>();
        }
    }
}
