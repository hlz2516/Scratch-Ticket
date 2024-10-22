using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using ScratchTicket.Helpers;
using ScratchTicket.ORM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
            vm = App.Container.Resolve<LoginViewModel>();
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

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }

    public partial class LoginViewModel : ObservableObject
    {
        private readonly ILogger logger;

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

        public LoginViewModel(ILogger _logger)
        {
            logger = _logger;
            Accounts = new ObservableCollection<ObservableUserInfo>();
            //从数据库读取所有用户
            using (var dc = new MyDbContext())
            {
                try
                {
                    var accounts = dc.UserInfos.ToList();
                    foreach (var item in accounts)
                    {
                        Accounts.Add(new ObservableUserInfo(item));
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
            //从配置加载用户名和密码
            Account = Config.Default.LastLoginAccout;
            Password = Config.Default.LastLoginPwd;
            //命令绑定
            GenerateCommand = new RelayCommand(Generate);
            LoginCommand = new RelayCommand(Login);
        }

        public IRelayCommand GenerateCommand { get; set; }
        private void Generate()
        {
            //在数据库新增一条随机生成的用户名和密码为123456的记录
            var userInfo = new UserInfo
            {
                Account = Guid.NewGuid().ToString().Substring(0,12),
                Name = ChineseWordsGenerator.GenerateChineseWord(5),
                Password = "123456"
            };
            using (var dc = new MyDbContext())
            {
                try
                {
                    dc.UserInfos.Add(userInfo);
                    dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
            Account = userInfo.Account;
            Password = userInfo.Password;
        }
        public IRelayCommand LoginCommand { get; set; }
        private void Login()
        {
            using (var dc = new MyDbContext())
            {
                try
                {
                    var user = dc.UserInfos.FirstOrDefault(x => x.Account == Account && x.Password == Password);
                    if (user != null)
                    {
                        UserSession.LoginedUser = user;
                        App.Container.Resolve<MainWindow>().Show();
                        App.Container.Resolve<Login>().Hide();
                        //将用户登录信息存入配置
                        Config.Default.LastLoginAccout = user.Account;
                        Config.Default.LastLoginPwd = user.Password;
                        Config.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码不正确，默认密码123456", "提示");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }
    }
}
