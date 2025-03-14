using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calelytic.Core.Enum_s;

namespace Calelytic.Core.Models
{
    public class Friend
    {
        public int FriendsAccountId { get; set; }
        public string FriendsAccountName { get; set; }
        public string FriendsEmail { get; set; }
        public FriendStatus FriendStatus { get; set; }
    }
}
