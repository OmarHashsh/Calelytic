using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calelytic.Core.Interfaces
{
    internal interface ISyncServices
    {

        Task SyncEventsWithGoogleAsync(int calendarId, string googleToken);
        Task RefreshGoogleTokenAsync(string refreshToken);
        
    }
}
