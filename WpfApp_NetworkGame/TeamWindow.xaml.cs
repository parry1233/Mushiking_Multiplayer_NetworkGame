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
	/// TeamWindow.xaml 的互動邏輯
	/// </summary>
	public partial class TeamWindow : Window
	{
		private string[] info;
		private Dictionary<string, string> name_ID;
		private string[] lineUp;
		private List<ComboBox> comboBoxes;
		public TeamWindow(string[] info_In)
		{
			InitializeComponent();
			this.info = new string[9];
			this.name_ID = new Dictionary<string, string>();
			this.lineUp = new string[5];
			this.info = info_In;
			this.lineUp[0] = this.info[6].Split('/')[0];
			this.lineUp[1] = this.info[6].Split('/')[1];
			this.lineUp[2] = this.info[6].Split('/')[2];
			this.lineUp[3] = this.info[6].Split('/')[3];
			this.lineUp[4] = this.info[6].Split('/')[4];
			this.comboBoxes = new List<ComboBox>();
			this.comboBoxes.Add(list1);
			this.comboBoxes.Add(list2);
			this.comboBoxes.Add(list3);
			this.comboBoxes.Add(list4);
			this.comboBoxes.Add(list5);
			loadmyCards();
			loadList();
		}

		private void loadmyCards()
		{
			xmlRun cardLoad = new xmlRun();
			cardLoad.LoadCard();

			string[] myCards_All = this.info[4].Split('/');
			foreach(string card in myCards_All)
			{
				this.name_ID.Add(cardLoad.getCardInfo(card)[2]+cardLoad.getCardInfo(card)[1], card);
			}
		}

		private void loadList()
		{
			//lineup
			card1.Source = new BitmapImage(new Uri(@"CardImg/Ver1/" + this.lineUp[0] + ".png", UriKind.Relative));
			card2.Source = new BitmapImage(new Uri(@"CardImg/Ver1/" + this.lineUp[1] + ".png", UriKind.Relative));
			card3.Source = new BitmapImage(new Uri(@"CardImg/Ver1/" + this.lineUp[2] + ".png", UriKind.Relative));
			card4.Source = new BitmapImage(new Uri(@"CardImg/Ver1/" + this.lineUp[3] + ".png", UriKind.Relative));
			card5.Source = new BitmapImage(new Uri(@"CardImg/Ver1/" + this.lineUp[4] + ".png", UriKind.Relative));

			//subs
			foreach(ComboBox list in this.comboBoxes)
			{
				list.Items.Clear();
				foreach(KeyValuePair<string,string> card in this.name_ID)
				{
					if(!card.Value.Equals(this.lineUp[0]) && !card.Value.Equals(this.lineUp[1]) && !card.Value.Equals(this.lineUp[2]) 
						&& !card.Value.Equals(this.lineUp[3]) && !card.Value.Equals(this.lineUp[4]))
					{
						list.Items.Add(card.Key);
					}
				}
			}
		}

		private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if((sender as ComboBox).SelectedIndex>=0)
			{
				if ((sender as ComboBox).Name.Equals("list1"))
				{
					this.lineUp[0] = this.name_ID[list1.SelectedItem.ToString()];
				}
				else if ((sender as ComboBox).Name.Equals("list2"))
				{
					this.lineUp[1] = this.name_ID[list2.SelectedItem.ToString()];
				}
				else if ((sender as ComboBox).Name.Equals("list3"))
				{
					this.lineUp[2] = this.name_ID[list3.SelectedItem.ToString()];
				}
				else if ((sender as ComboBox).Name.Equals("list4"))
				{
					this.lineUp[3] = this.name_ID[list4.SelectedItem.ToString()];
				}
				else if ((sender as ComboBox).Name.Equals("list5"))
				{
					this.lineUp[4] = this.name_ID[list5.SelectedItem.ToString()];
				}

				loadList();
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string edit = this.lineUp[0] + "/" + this.lineUp[1] + "/" + this.lineUp[2] + "/" + this.lineUp[3] + "/" + this.lineUp[4];
			database db = new database();
			db.dbEditTeam(this.info[0], edit);

			db.dbLogIn(this.info[0], this.info[1]);
			this.Close();
		}
	}
}
