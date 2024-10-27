using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using ScratchTicket.Controls;
using ScratchTicket.Helpers;
using ScratchTicket.ORM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ScratchTicket
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.Container.Resolve<MainWindowViewModel>();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }

    public class MainWindowViewModel : ObservableObject
    {
        private readonly ILogger logger;
        private double capital;
        public double Capital
        {
            get { return capital; }
            set 
            {
                SetProperty(ref capital, value); 
            }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }


        public ObservableCollection<ObservableCardBundle> Purchased { get; private set; }
        public ObservableCollection<ObservableCardBundle> Goods { get; private set; }

        public MainWindowViewModel()
        {
            Goods = new ObservableCollection<ObservableCardBundle>();
            Purchased = new ObservableCollection<ObservableCardBundle>();
            //绑定命令
            PurchaseCommand = new RelayCommand<object>(PurchaseCard);
            OpenBlindBoxCommand = new RelayCommand<object>(OpenBlindBox);
            var loginedUser = UserSession.LoginedUser;
            UserName = loginedUser.Name;
            Capital = loginedUser.Capital;
            using (var dc = new MyDbContext())
            {
                try
                {
                    //查找当前用户的资产和所有的卡包，卡包赋给bundles
                    var cardBundles = dc.PurchasedCardBundles.Where(x => x.AccountID == loginedUser.Account)
                        .Join(dc.CardBundles, p => p.CardBundleID, c => c.Guid, (p, c) => c);
                    foreach (var card in cardBundles)
                    {
                        var obCard = new ObservableCardBundle(card);
                        obCard.OpenBlindBoxCommand = new RelayCommand<object>(OpenBlindBox);
                        Purchased.Add(obCard);
                    }
                    //顺便查找所有商品
                    var goods = dc.CardBundles.Where(x=>x.Guid.StartsWith("SAMPLE")).OrderBy(x=>x.Price).ToList();
                    if (goods.Count == 0) //如果没有，则自动生成
                    {
                        goods.Add(new CardBundle
                        {
                            Guid = "SAMPLE" + Guid.NewGuid().ToString(),  //所有模板商品的GUID前缀为SAMPLE，用户购买到的都是具有真实GUID的
                            CardType = CardBundleType.Normal,
                            Price = 20,
                            CardsCount = 8,
                            Background = "/Resources/cardback1.jpg"
                        });
                        goods.Add(new CardBundle
                        {
                            Guid = "SAMPLE" + Guid.NewGuid().ToString(),
                            CardType = CardBundleType.Rare,
                            Price = 50,
                            CardsCount = 8,
                            Background = "/Resources/cardback2.jpg"
                        });
                        goods.Add(new CardBundle
                        {
                            Guid = "SAMPLE" + Guid.NewGuid().ToString(),
                            CardType = CardBundleType.Legend,
                            Price = 100,
                            CardsCount = 8,
                            Background = "/Resources/cardback3.jpg"
                        });
                        dc.CardBundles.AddRange(goods);
                        dc.SaveChanges();
                    }
                    foreach (var good in goods)
                    {
                        var g = new ObservableCardBundle(good);
                        g.PurchaseCommand = new RelayCommand<object>(PurchaseCard);
                        Goods.Add(g);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        public MainWindowViewModel(ILogger _logger):this()
        {
            logger = _logger;
        }

        public IRelayCommand PurchaseCommand { get;private set; }
        private void PurchaseCard(object obj)
        {
            CardHolder cardHolder = obj as CardHolder;
            //判断个人资产是否足够购买
            if (Capital - cardHolder.Price < 0.01)
            {
                MessageBox.Show("您没有足够的现金购买该卡包，请选择别的卡包或者切换账号！", "提示");
                return;
            }
            Capital -= cardHolder.Price;
            //App.Container.Resolve<MainWindow>().StartAssetChangeAnimation(Variable);

            using (var dc = new MyDbContext())
            {
                //新建要购买的卡包
                var cardBundle = new CardBundle
                {
                    Guid = Guid.NewGuid().ToString(),
                    CardType = cardHolder.BundleType,
                    Price = cardHolder.Price,
                    CardsCount = 8,
                    Background = cardHolder.Source.ToString(),
                };
                //添加到卡包表和已购卡包表
                dc.CardBundles.Add(cardBundle);
                dc.PurchasedCardBundles.Add(new PurchasedCardBundle
                {
                    AccountID = UserSession.LoginedUser.Account,
                    CardBundleID = cardBundle.Guid
                });
                //更新用户的资产
                UserSession.UpdateUserAsset(Capital);
                dc.SaveChanges();
                //添加到已购卡包集合（UI）
                var obCard = new ObservableCardBundle(cardBundle);
                obCard.OpenBlindBoxCommand = new RelayCommand<object>(OpenBlindBox);
                Purchased.Add(obCard);
            }
        }
    
        public IRelayCommand OpenBlindBoxCommand { get; private set; }
        private void OpenBlindBox(object obj)
        {
            CardHolder cardHolder = obj as CardHolder;
            System.Diagnostics.Debug.WriteLine(cardHolder.Guid);
        }
    }
}
