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

        public static void UpdateUserAsset(double capital)
        {
            using (var dc = new MyDbContext())
            {
                var user = dc.UserInfos.FirstOrDefault(x => x.Account == LoginedUser.Account);
                if (user != null)
                {
                    user.Capital = capital;
                    dc.SaveChanges();
                }
            }
        }
    }
}
