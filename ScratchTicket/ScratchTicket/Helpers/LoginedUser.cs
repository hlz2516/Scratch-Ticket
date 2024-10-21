using ScratchTicket.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchTicket.Helpers
{
    public static class UserSession
    {
        public static UserInfo LoginedUser { get; set; }
    }
}
