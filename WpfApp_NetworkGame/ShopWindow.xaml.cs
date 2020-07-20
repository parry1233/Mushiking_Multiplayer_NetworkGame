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
	/// ShopWindow.xaml 的互動邏輯
	/// </summary>
	public partial class ShopWindow : Window
	{
		private string cardID;
		private string[] info;
		public ShopWindow(string[] info_in)
		{
			InitializeComponent();
			Load();
			this.info = new string[8];
			this.info = info_in;
		}

		private void Load()
		{
			xmlRun cards = new xmlRun();
			cards.LoadCard();
			this.cardID = cards.getRandom();
			string uri = "CardImg/Ver1/" + this.cardID + ".png";

			CardImg.Source = new BitmapImage(new Uri(@uri, UriKind.Relative));
		}

		private void RecordCard(object sender, MouseButtonEventArgs e)
		{
			bool checkExist = false;
			string[] cards = this.info[4].Split('/');
			foreach(string card in cards)
			{
				if(card.Equals(this.cardID))
				{
					checkExist = true;
					break;
				}
			}
			database db = new database();
			if(!checkExist)
			{
				db.dbEditCard(this.info[0], cardID);
				db.dbEditCoin(this.info[0], -1000);
			}
			else
			{
				db.dbEditCoin(this.info[0], -500);
			}

			db.dbLogIn(this.info[0], this.info[1]);
			CardImg.IsEnabled = false;
			this.Close();
		}
	}
}
