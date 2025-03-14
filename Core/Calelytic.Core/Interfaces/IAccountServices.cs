using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.DTOs;
using Calelytic.Core.Models;

namespace Calelytic.Core.Interfaces
{
        public interface IAccountServices
        {
            // Authentication
            Task<Account> RegisterAsync(AccountDTO accountDto);
            Task<Account> LoginAsync(string email, string password);
            Task<Account> LoginWithGoogleAsync(string googleToken);

            // Profile Management
            Task UpdateDisplayNameAsync(int accountId, string displayName);
            Task UpdateTimeZoneAsync(int accountId, string timeZone);
            Task DeleteAccountAsync(int accountId);

            // Social Features
            Task AddFriendAsync(int accountId, int friendAccountId);
            Task RemoveFriendAsync(int accountId, int friendAccountId);
            Task<List<Friend>> GetFriendsListAsync(int accountId);
        }
    
}
