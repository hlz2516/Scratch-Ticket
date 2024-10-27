using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public string Guid { get => cardBundle.Guid; }

        public CardBundleType CardType
        {
            get => cardBundle.CardType;
            set => SetProperty(cardBundle.CardType, value, cardBundle, (u, p) => u.CardType = p);
        }

        public int CardsCount
        {
            get => cardBundle.CardsCount;
            set => SetProperty(cardBundle.CardsCount, value, cardBundle, (u, p) => u.CardsCount = p);
        }

        public double Price
        {
            get => cardBundle.Price;
            set => SetProperty(cardBundle.Price, value, cardBundle, (u, p) => u.Price = p);
        }

        public string Background
        {
            get => cardBundle.Background; 
            set => SetProperty(cardBundle.Background, value, cardBundle, (u, p) => u.Background = p);
        }

        public IRelayCommand PurchaseCommand { get; set; }
        public IRelayCommand OpenBlindBoxCommand { get; set; }
    }
}
