using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.DTOs;
using Calelytic.Core.Models;

namespace Calelytic.Core.Interfaces
{
    internal interface IEventServices
    {
        // Event Management
        Task<Event> CreateEventAsync(int calendarId, EventDTO eventDto);
        Task UpdateEventAsync(int eventId, EventDTO eventDto);
        Task DeleteEventAsync(int eventId);

        // Recurring Events
        Task<List<Event>> GetRecurringEventsAsync(int eventId);

        // Reminders
        Task SetReminderAsync(int eventId, TimeSpan reminderTime);
    }
}
