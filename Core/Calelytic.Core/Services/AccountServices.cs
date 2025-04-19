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
            bool exists = await _context.Account.AnyAsync(a => a.Id == accountId);
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

            _context.Account.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }


        // Fetch data of some account and return to caller.
        // *1 *2
        public async Task<Guid?> FetchAccDataByEmail(string email)
        {
            var accountId = await _context.Account
                .Where(a => a.Email == email)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();
            return accountId == Guid.Empty ? null : accountId;
        }
        // Modify account
        // *1
        public async Task<bool> ModifyAccount(Guid id, AccountDTO newData)
        {
            var dataInDB = await _context.Account.FindAsync(id);
            if (dataInDB == null)
                return false;

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
            return true;
        }

        // Soft Delete an account ( Gives a chance for someone to undo this action but permanently deletes the account after 30 days)
        // *1
        public async Task<bool> SoftDeleteByGuid(Guid Id)
        {
            var account = await _context.Account.FindAsync(Id);
            if (account == null)
                return false;

            account.IsDeleted = true;
            account.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true; 
        }
    }
    // Hard delete an account
    // *1
    // I believe that this should be added as a routine for the server where everyday at 00:00 the server checks the logs for all
    // deletion operations that happened 30 days (or any other period) before DateTime.UtcNow and implement a hard delete method
    // in that routine. When you read this comment, tell me how that could take place.

}
