using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Models;
using Calelytic.Core.DTOs;
using Calelytic.Core.Interfaces;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Calelytic.Core.Enums;

namespace Calelytic.Core.Services
{
    public class EventServices
    {
        private readonly ApplicationDbContext _context;
        public EventServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DoesEventExist(int id)
        {
            bool exists = await _context.Event.AnyAsync(a => a.Id == id);
            return exists;
        }

        public async Task<int> CreateEvent(Guid UserId, EventDTO dto)
        {
            //You should implement a privilege check in the future
            var newEvent = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Type = dto.Type,
                Recurrence = dto.Recurrence,
                DateOfEvent = dto.DateOfEvent,
                Duration = dto.Duration,
                IsAllDay = dto.IsAllDay,
                Reminders = dto.Reminders?.ToList(),
                Participants = dto.Participants?.ToList(),
                UpdatedBy = UserId

            };
            await _context.SaveChangesAsync();
            return newEvent.Id;
        }
        public async Task<EventDTO> FetchEventDataById(int EventId)
        {
            var requestedEvent = await _context.Event
                .Where(e => e.EventId == EventId)
                .Select(e => e.EventId == EventId)
                .FirstOrDefault();

            return requestedEvent == null ? null : requestedEvent;
        }

        public async Task<bool> ModifyEventDateTime(int EventId, DateTime RequestedDateTime)
        {
            var eventToModify = await _context.Event.firstOrDefault(e => e.EventId == EventId);
            if (eventToModify != null)
            {
                eventToModify.DateOfEvent = RequestedDateTime;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //make sure to only pass non-repeating values or values that aren't already present in the DB.
        public async Task<bool> AddEventParticipants(EventDTO dto, List<string> Emails, IAccountServices account)
        {
            var existingEvent = await _context.Event
                .FirstOrDefaultAsync(e => e.EventId == dto.EventId);

            if (existingEvent == null)
                return false;

            foreach (var email in Emails)
            {
                var participant = await account.FetchAccDataByEmail(email);
                existingEvent.Participants.Add(participant);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveEventParticipants(EventDTO dto, List<string> Emails, IAccountServices account)
        {
            var existingEvent = await _context.Event
                .FirstOrDefaultAsync(e => e.EventId == dto.EventId);

            if (existingEvent == null)
                return false;
            foreach (var email in Emails)
            {
                var participant = await account.FetchAccDataByEmail(email);
                existingEvent.Participants.Remove(participant);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ModifyEventDescription(EventDTO dto, string newDescription)
        {
            var existingEvent = await _context.Event
                .FirstOrDefaultAsync(e => e.EventId == dto.EventId);
            if (existingEvent == null) return false;

            existingEvent.Description = newDescription;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ModifyEventOccurrence(EventDTO dto)
        {
            var existingEvent = await _context.Events.Include(e => e.Occurrence).FirstOrDefaultAsync(e => e.EventId == dto.EventId);

            // Short-circuit if event or its recurrence doesn't exist
            if (existingEvent?.Occurrence == null || dto.Recurrence == null)
                return false;

            // Map only updatable properties (excluding DaysOfTheWeek)
            var source = dto.Recurrence;
            var target = existingEvent.Occurrence;

            target.Frequency = source.Frequency;
            target.Interval = source.Interval;
            target.OccurrenceStartDate = source.OccurrenceStartDate;
            target.OccurrenceEndDate = source.OccurrenceEndDate;
            target.CustomPattern = source.CustomPattern;
            target.CustomPatternStartingDate = source.CustomPatternStartingDate;

            // Explicitly mark as modified (optional but clear)
            _context.Entry(target).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ModifyType (EventDTO dto, EventType NewType)
        {
            var existingEvent = await _context.Event
                .FirstOrDefaultAsync(e => e.EventId == dto.EventId);
            if (existingEvent == null) return false;

            existingEvent.EventType = NewType;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ModifyTitle(EventDTO dto, string NewTitle)
        {
            var existingEvent= await _context.Event.FirstOrDefaultAsync(e => e.EventId == dto.EventId);
            if (existingEvent == null) return false;
            existingEvent.Title = NewTitle;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ModifyDuration(EventDTO dto, TimeSpan NewTime)
        {
            var existingEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventId == dto.EventId);
            if (existingEvent == null) return false;
            existingEvent.Duration = NewTime;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReinstateEvent(EventDTO dto)
        {
            var existingEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventId == dto.EventId);
            if (existingEvent == null) return false;
            existingEvent.IsDeleted = 0;
            return true; 
        }

        public async Task<bool> DeleteEvent(EventDTO dto)
        {
            var ToBeDeleted = await _context.Event.FindAsync(dto.EventId);
            if (ToBeDeleted == null) return false;

            _context.Event.Remove(dto.EventId);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddReminder( ReminderDTO dto )
        {
            var checkForValidEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventId == dto.EventId);
            if (checkForValidEvent == null) return false;

            var newReminder = new Reminder
            {
                TimeBeforeEvent = dto.TimeBeforeEvent,
                Method = dto.Method,
                EventId = dto.EventId,
            };

            checkForValidEvent.Reminders.Add(newReminder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveReminder( ReminderDTO dto )
        {
            var checkForValidEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventId == dto.EventId);
            if (checkForValidEvent == null) return false;

            var newReminder = new Reminder
            {
                TimeBeforeEvent = dto.TimeBeforeEvent,
                Method = dto.Method,
                EventId = dto.EventId,
            };

            checkForValidEvent.Reminders.Remove(newReminder);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
