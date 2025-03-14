using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calelytic.Core.Interfaces
{
    internal interface INotificationServices
    {
        Task SendReminderAsync(int eventId);
        Task NotifyFriendRequestAsync(int accountId, int friendAccountId);
        
    }
}
