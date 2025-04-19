using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Models;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace Calelytic.Core.DTOs
{
    public class AccountDTO
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string? Email { get; set; }
        public string? GoogleAuthToken { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? TimeZone { get; set; }
        public List<Friend>? FriendsList { get; set; }
        public List<Calendar>? Calendars { get; set; }
        public List<Event>? Events { get; set; }
        public List<DeletedEvent>? DeletedEvents { get; set; }
    }
}
