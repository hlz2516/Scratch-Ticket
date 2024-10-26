using CommunityToolkit.Mvvm.ComponentModel;
using NLog;
using ScratchTicket.Controls;
using ScratchTicket.Helpers;
using ScratchTicket.ORM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Autofac;
using CommunityToolkit.Mvvm.Input;

namespace ScratchTicket
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard storyboard1;
        public event Action<double> AssetChanged;

        public MainWindow()
        {
            InitializeComponent();
            AssetChangeAnimationInit();
            //AssetChanged += MainWindow_AssetChanged;
            this.DataContext = App.Container.Resolve<MainWindowViewModel>();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void AssetChangeAnimationInit()
        {
            storyboard1 = new Storyboard();
            var duration = TimeSpan.FromMilliseconds(250);
            DoubleAnimation doubleAnimation1 = new DoubleAnimation();
            doubleAnimation1.Duration = new Duration(duration);
            storyboard1.Children.Add(doubleAnimation1);

            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            storyboard1.Children.Add(doubleAnimation2);
            doubleAnimation2.BeginTime = duration;
            doubleAnimation2.Duration = duration;

            Storyboard.SetTarget(doubleAnimation1, txtVar);
            Storyboard.SetTargetProperty(doubleAnimation1, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTarget(doubleAnimation2, txtVar);
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath("(Canvas.Top)"));

            DoubleAnimation doubleAnimation3 = new DoubleAnimation();
            doubleAnimation3.From = 0; // 开始位置
            doubleAnimation3.To = 1; // 结束位置
            doubleAnimation3.BeginTime = TimeSpan.FromMilliseconds(0);
            doubleAnimation3.Duration = new Duration(duration);
            storyboard1.Children.Add(doubleAnimation3);

            DoubleAnimation doubleAnimation4 = new DoubleAnimation();
            doubleAnimation4.From = 1; // 开始位置
            doubleAnimation4.To = 0; // 结束位置
            doubleAnimation4.BeginTime = duration;
            doubleAnimation4.Duration = duration;
            storyboard1.Children.Add(doubleAnimation4);

            Storyboard.SetTarget(doubleAnimation3, cvs1);
            Storyboard.SetTargetProperty(doubleAnimation3, new PropertyPath("Opacity"));
            Storyboard.SetTarget(doubleAnimation4, cvs1);
            Storyboard.SetTargetProperty(doubleAnimation4, new PropertyPath("Opacity"));
        }

        private bool isSubscribed = false;
        private EventHandler eventHandler = null;
        public void StartAssetChangeAnimation(double variable)
        {
            DoubleAnimation doubleAnimation1 = storyboard1.Children[0] as DoubleAnimation;
            DoubleAnimation doubleAnimation2 = storyboard1.Children[1] as DoubleAnimation;

            if (eventHandler != null)
            {
                doubleAnimation1.Completed -= eventHandler;
            }
            eventHandler = (sender, eve) =>
            {
                AssetChanged?.Invoke(variable);
            };
            doubleAnimation1.Completed += eventHandler;

            if (variable > 0)
            {
                txtVar.Foreground = new SolidColorBrush(Colors.LightGreen);
                doubleAnimation1.From = cvs1.ActualHeight; // 开始位置
                doubleAnimation1.To = (cvs1.ActualHeight - txtVar.ActualHeight) / 2; // 结束位置
            }
            else if (variable < 0)
            {
                txtVar.Foreground = new SolidColorBrush(Colors.Red);
                doubleAnimation1.From = -txtVar.ActualHeight; // 开始位置
                doubleAnimation1.To = (cvs1.ActualHeight - txtVar.ActualHeight) / 2; // 结束位置
            }

            if (variable > 0)
            {
                doubleAnimation2.From = (cvs1.ActualHeight - txtVar.ActualHeight) / 2; // 开始位置
                doubleAnimation2.To = -txtVar.ActualHeight; // 结束位置
            }
            else if (variable < 0)
            {
                doubleAnimation2.From = (cvs1.ActualHeight - txtVar.ActualHeight) / 2; // 开始位置
                doubleAnimation2.To = cvs1.ActualHeight; // 结束位置
            }

            storyboard1.Begin();
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
                if (Math.Abs(capital -value) > 0.01)
                {
                    Variable = value - capital;
                }
                SetProperty(ref capital, value); 
            }
        }

        private double variable;
        public double Variable
        {
            get => variable;
            set => SetProperty(ref variable, value);
        }

        public ObservableCollection<ObservableCardBundle> Purchased { get; private set; }
        public ObservableCollection<ObservableCardBundle> Goods { get; private set; }

        public MainWindowViewModel()
        {
            Goods = new ObservableCollection<ObservableCardBundle>();
            Purchased = new ObservableCollection<ObservableCardBundle>();
            //绑定命令
            PurchaseCommand = new RelayCommand<object>(PurchaseCard);
            var loginedUser = UserSession.LoginedUser;
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
                        Purchased.Add(new ObservableCardBundle(card));
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
            App.Container.Resolve<MainWindow>().StartAssetChangeAnimation(Variable);

            using (var dc = new MyDbContext())
            {
                //新建要购买的卡包
                var cardBundle = new CardBundle
                {
                    Guid = Guid.NewGuid().ToString(),
                    CardType = cardHolder.BundleType,
                    Price = cardHolder.Price,
                    CardsCount = 8,
                    Background = cardHolder.Source.ToString()
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
                Purchased.Add(new ObservableCardBundle(cardBundle));
            }
        }
    }
}
