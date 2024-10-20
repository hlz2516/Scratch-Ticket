using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using ScratchTicket.ORM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScratchTicket
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private readonly LoginViewModel vm;
        public Login()
        {
            InitializeComponent();
            vm = new LoginViewModel();
            this.DataContext = vm;
        }

        private void cbbAccount_SelectionChanged(object sender, EventArgs e)
        {
            cbbAccount.DisplayMemberPath = "Account";
        }

        private void cbbAccount_DropDownOpened(object sender, EventArgs e)
        {
            cbbAccount.DisplayMemberPath = "AccountWidthName";
        }
    }

    public partial class LoginViewModel : ObservableObject
    {
        private string account;
        public string Account
        {
            get => account;
            set => SetProperty(ref account, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public ObservableCollection<ObservableUserInfo> Accounts { get; set; }
        public LoginViewModel()
        {
            Accounts = new ObservableCollection<ObservableUserInfo>();
            //从数据库读取所有用户
            using (var dc = new MyDbContext())
            {
                var accounts = dc.UserInfos.ToList();
                foreach (var item in accounts)
                {
                    Accounts.Add(new ObservableUserInfo(item));
                }
            }
        }
    }
}
