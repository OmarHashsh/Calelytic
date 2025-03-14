using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calelytic.Core.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = "User";
        public string? Email { get; set; }// made optional along with the password to allow the user to login with google
        public string? PasswordHash { get; set; } 
        public string? TimeZone { get; set; }
        public List<Friend>? FriendsList { get; set; }
        public List<Calendar>? Calendars { get; set; }
        public List<Event>? Events { get; set; }
        public List<DeletedEvent>? DeletedEvents { get; set; }
        
        public Account()
        {
            Email = "N/a";
            DisplayName = Email;
        }
    }
}
