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
	/// LogInWindow.xaml 的互動邏輯
	/// </summary>
	public partial class LogInWindow : Window
	{
		private database db;
		public LogInWindow()
		{
			InitializeComponent();
			this.db = new database();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(IDtxt.Text.Length>0&&PWtxt.Text.Length>0)
			{
				logInBTN.IsEnabled = false;
				this.db.dbLogIn(IDtxt.Text, PWtxt.Text);
			}
			else
			{
				MessageBox.Show("帳號與密碼可能空白，請檢察!");
			}
		}
	}
}
