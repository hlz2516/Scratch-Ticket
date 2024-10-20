using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchTicket.ORM
{
    /// <summary>
    /// 将UserInfo包装成可观察模型
    /// </summary>
    public partial class ObservableUserInfo: ObservableObject
    {
        private readonly UserInfo userInfo;
        public ObservableUserInfo(UserInfo _userinfo)
        {
            userInfo = _userinfo;
        }
        public string Account
        {
            get => userInfo.Account;
            set => SetProperty(userInfo.Account, value,userInfo,(u,p)=>u.Account = p);
        }
        public string Password
        {
            get => userInfo.Password;
            set => SetProperty(userInfo.Password, value, userInfo, (u, p) => u.Password = p);
        }
        public string Name
        {
            get => userInfo.Name;
            set => SetProperty(userInfo.Name, value, userInfo, (u, p) => u.Name = p);
        }
        public double Capital
        {
            get => userInfo.Capital;
            set => SetProperty(userInfo.Capital, value, userInfo, (u, p) => u.Capital = p);
        }
        public string AccountWidthName
        {
            get { return $"{userInfo.Account}({userInfo.Name})"; }
        }
    }
}
