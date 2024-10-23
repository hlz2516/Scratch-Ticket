using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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

    public partial class ObservableCardBundle:ObservableObject
    {
        private readonly CardBundle cardBundle;
        public ObservableCardBundle(CardBundle _cardBundle)
        {
            cardBundle = _cardBundle;
        }

        public int ID
        {
            get => cardBundle.ID;
            set => SetProperty(cardBundle.ID, value, cardBundle,(u,p)=>u.ID = p);
        }

        public CardBundleType CardType
        {
            get => cardBundle.CardType;
            set => SetProperty(cardBundle.CardType, value, cardBundle, (u, p) => u.CardType = p);
        }

        public int TotalCount
        {
            get => cardBundle.TotalCount;
            set => SetProperty(cardBundle.TotalCount, value, cardBundle, (u, p) => u.TotalCount = p);
        }

        public string Background
        {
            get => cardBundle.Background; 
            set => SetProperty(cardBundle.Background, value, cardBundle, (u, p) => u.Background = p);
        }
    }
}
