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
        Task<bool> DoesAccountExist(Guid accountId);
        Task<Account> CreateAccount(AccountDTO dto);
        Task<Guid?> FetchAccDataByEmail(string email);
        Task<bool> ModifyAccount(Guid id, AccountDTO newData);
        Task<bool> SoftDeleteByGuid(Guid Id);

    }

}
