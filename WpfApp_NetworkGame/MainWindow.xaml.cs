using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_NetworkGame
{
	/// <summary>
	/// MainWindow.xaml 的互動邏輯
	/// </summary>
	public partial class MainWindow : Window
	{
		private string[] userInfo;
		public MainWindow()
		{
			InitializeComponent();
			this.userInfo = new string[9];
		}

		public void userData(string[] userInfoIn)
		{
			this.userInfo = userInfoIn;
			string[] collectCard = this.userInfo[4].Split('/');
			string userName = this.userInfo[7].Split('/')[0];
			UserInfoLabel.Content = "守護者 "+userName+"，歡迎回來!\r\n\r\n已收集的甲蟲: "+collectCard.Length+"隻\r\n剩餘代幣: "+this.userInfo[8]+"M幣";

			LeaderImg.Source = new BitmapImage(new Uri(@"CardImg/Ver1/" + this.userInfo[6].Split('/')[0] + ".png", UriKind.Relative));
		}

		private void CreateHost(object sender, RoutedEventArgs e)
		{
			LobbyWindow lobbyWindow = new LobbyWindow();
			lobbyWindow.getInfo(this.userInfo);
			lobbyWindow.Show();
			this.Close();
		}

		private void JoinRoom(object sender, RoutedEventArgs e)
		{
			IPConfirmWindow iPConfirmWindow = new IPConfirmWindow();
			iPConfirmWindow.getInfo(this.userInfo);
			iPConfirmWindow.Show();
			this.Close();
		}

		private void ToShop(object sender, RoutedEventArgs e)
		{
			MessageBoxResult popup = MessageBox.Show("探索一隻甲蟲將耗費 1000 M幣，確定繼續?", "Shop", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if(popup == MessageBoxResult.Yes)
			{
				if (Convert.ToInt32(this.userInfo[8]) >= 1000)
				{
					ShopWindow shopWindow = new ShopWindow(this.userInfo);
					shopWindow.Show();
					this.Close();
				}
				else
				{
					MessageBox.Show("糟糕!看來錢花完了!\r\n玩幾場遊戲再來探索新的甲蟲吧!");
				}
			}
		}

		private void ToTeam(object sender, RoutedEventArgs e)
		{
			TeamWindow teamWindow = new TeamWindow(this.userInfo);
			teamWindow.Show();
			this.Close();
		}
	}
}
