using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calelytic.Core.Models
{
    public class Calendar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Event>? EventsList { get; set; }
        public List<DeletedEvent>? DeletedEventsList { get; set; }
        public Calendar()
        {
            EventsList = new List<Event>();
            DeletedEventsList = new List<DeletedEvent>();
        }
    }
}
