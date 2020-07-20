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
using System.Windows.Shapes;

namespace WpfApp_NetworkGame
{
	/// <summary>
	/// IPConfirmWindow.xaml 的互動邏輯
	/// </summary>
	public partial class IPConfirmWindow : Window
	{
		private Client client;
		private string[] info;
		public IPConfirmWindow()
		{
			InitializeComponent();
			this.info = new string[9];
			this.client = new Client();
			Conditionlabel.Content = "";
			LeaveIP.IsEnabled = false;
		}

		public void getInfo(string[] infoIn)
		{
			this.info = infoIn;
			yourInfo.Content = "[你的個人資訊]\r\n用戶名稱: " + this.info[7].Split('/')[0]
				+ "\r\n用戶戰績: " + this.info[5].Split('/')[0] + "勝 " + this.info[5].Split('/')[1] + "敗 " + this.info[5].Split('/')[2] + "和"
				+ "\r\n用戶隊長: " + this.info[3] + "\r\n用戶餘額: " + this.info[8];
		}

		private void JoinIP_Click(object sender, RoutedEventArgs e)
		{
			this.client.clientConnect(ipIN.Text);
		}

		public void wait()
		{
			Conditionlabel.Content = "已向" + ipIN.Text + "發起挑戰，等待回復...";
			JoinIP.IsEnabled = false;
			ipIN.IsEnabled = false;
			LeaveIP.IsEnabled = true;
		}

		private void LeaveIP_Click(object sender, RoutedEventArgs e)
		{
			this.client.clientSend("LEAVE");
		}

		public void leave()
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.userData(this.info);
			mainWindow.Show();
			this.Close();
		}

		public string[] getInfo()
		{
			return this.info;
		}
	}
}
