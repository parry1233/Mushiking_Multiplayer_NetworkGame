using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp_NetworkGame
{
	/// <summary>
	/// LobbyWindow.xaml 的互動邏輯
	/// </summary>
	public partial class LobbyWindow : Window
	{
		private Server server;
		private string[] info;
		public LobbyWindow()
		{
			InitializeComponent();
			this.info = new string[9];
			server = new Server();
			server.OpenServer();
			HostInfo.Content = "[Host Device Name]\r\n"+this.server.getHostName() + "\r\n" + "[Host IPV4 address]\r\n"+this.server.getIP();
		}

		public void getInfo(string[] infoIn)
		{
			this.info = infoIn;
			yourInfo.Content = "[你的個人資訊]\r\n用戶名稱: " + this.info[7].Split('/')[0]
				+ "\r\n用戶戰績: " + this.info[5].Split('/')[0] + "勝 " + this.info[5].Split('/')[1] + "敗 " + this.info[5].Split('/')[2] + "和"
				+ "\r\n用戶稱號: " + this.info[3] + "\r\n用戶餘額: " + this.info[8];
		}

		private void SendPermit(object sender, RoutedEventArgs e)
		{
			if(JoinClientList.SelectedIndex>-1)
			{
				this.server.serverSend(this.server.directSocketFind(JoinClientList.SelectedItem.ToString()), "PLAY/" + this.info[7].Split('/')[0]
					+ "/" + this.info[6].Split('/')[0] + "/" + this.info[6].Split('/')[1] + "/" + this.info[6].Split('/')[2] + "/" + this.info[6].Split('/')[3]
					+ "/" + this.info[6].Split('/')[4]);
			}
		}

		public void AddClient(string remoteEndPoint)
		{
			JoinClientList.Items.Add(remoteEndPoint);
		}

		public void deleteClient(string remoteEndPoint)
		{
			JoinClientList.Items.Remove(remoteEndPoint);
		}

		public string[] getInfo()
		{
			return this.info;
		}
	}
}
