using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Models;

namespace Calelytic.Core.Interfaces
{
    internal interface ICalendarServices
    {
            // Calendar Management
            Task<Calendar> CreateCalendarAsync(int accountId, string calendarName);
            Task DeleteCalendarAsync(int calendarId);
            Task ShareCalendarAsync(int calendarId, int friendAccountId);

            // Event Management
            Task<List<Event>> GetCalendarEventsAsync(int calendarId);
            Task SyncWithGoogleCalendarAsync(int calendarId, string googleToken);
    
    }
}
