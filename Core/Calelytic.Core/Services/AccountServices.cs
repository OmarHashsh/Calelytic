using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Calelytic.Core.DTOs;
using Calelytic.Core.Models;
using Microsoft.EntityFrameworkCore;
using Calelytic.Core.Exceptions;
using System.Security.Principal;


namespace Calelytic.Core.Services
{
    public class AccountServices
    {
        private readonly ApplicationDbContext _context;

        public AccountServices(ApplicationDbContext context)
        {
            _context = context;
        }



        // Does account exist?
        public async Task<bool> DoesAccountExist(Guid accountId)
        {
            bool exists = await _context.Accounts.AnyAsync(a => a.Id == accountId);
            return exists;
        }

        // Create account
        public async Task<Account> CreateAccount(AccountDTO dto)
        {
            var account = new Account
            {
                Id = dto.Id,
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow

            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }


        // Fetch data of some account and return to caller.
        // *1 *2
        public async Task<Guid> FetchAccDataByEmail(string email)
        {
            var accountId = await _context.Accounts
                .Where(a => a.Email == email)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            if (accountId == Guid.Empty)
                throw new NotFoundException("Account not found.");
            return accountId;
        }
        // Modify account
        // *1
        public async Task ModifyAccount(Guid id, AccountDTO newData)
        {
            var dataInDB = await _context.Accounts.FindAsync(id);
            if (dataInDB == null)
            {
                throw new NotFoundException("Account not found.");
            }

            var nonEditableProps = new HashSet<string>
            {
                "Id", "CreatedAt", "CreatedBy",
                "FriendsList", // managed via FriendService
                "Calendars",   // managed via CalendarService
                "Events",      // managed via EventService
                "DeletedEvents"// internal
            };
            var changes = new List<(string Field, object? OldVal, object? NewVal)>();
            foreach (var prop in typeof(AccountDTO).GetProperties())
            {
                if (nonEditableProps.Contains(prop.Name)) continue;

                var newValue = prop.GetValue(newData);
                var existingValue = prop.GetValue(dataInDB);

                if (!Equals(newValue, existingValue))
                {
                    changes.Add((prop.Name, existingValue, newValue));
                    prop.SetValue(dataInDB, newValue);
                }
            }
            await _context.SaveChangesAsync();
        }

        // Soft Delete an account ( Gives a chance for someone to undo this action but permanently deletes the account after 30 days)
        // *1
        public async Task SoftDeleteByGuid(Guid Id)     // replace the value passed to Task with the appropriate value for my purpose
        {
            var account = await _context.Accounts.FindAsync(Id);
            if (account == null)
                throw new NotFoundException("Account not found.");
            account.IsDeleted = true;
            account.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
    // Hard delete an account
    // *1
    // I believe that this should be added as a routine for the server where everyday at 00:00 the server checks the logs for all
    // deletion operations that happened 30 days (or any other period) before DateTime.UtcNow and implement a hard delete method
    // in that routine. When you read this comment, tell me how that could take place.




    /* EDGE CASES:
        1* Account for the case where the passed ID doesn't belong to any account.
        2* Account for the case where the method doesn't need the class-wide AccountInstance. 
    */

}
