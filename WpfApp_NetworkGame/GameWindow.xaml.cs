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
using System.Windows.Threading;

namespace WpfApp_NetworkGame
{
	/// <summary>
	/// GameWindow.xaml 的互動邏輯
	/// </summary>
	public partial class GameWindow : Window
	{
		//DispatcherTimer youScoreRun = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(10) };
		//DispatcherTimer enScoreRun = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(10) };
		private string yourID="";
		private xmlRun xmlData;
		private bool turn = false;
		private int round = 0;
		private List<string> youLog = new List<string>();
		private List<string> enLog = new List<string>();
		public bool imgCheck1 = true, imgCheck2 = true, imgCheck3 = true, imgCheck4 = true, imgCheck5 = true;
		public string en_card1="", en_card2="", en_card3="", en_card4="", en_card5="";
		public string my_card1 = "", my_card2 = "", my_card3 = "", my_card4 = "", my_card5 = "";
		private string tempChooseID = "";

		public GameWindow(bool turn_check)
		{
			InitializeComponent();
			int initZero = 0;
			enPts.Content = initZero.ToString();
			myPts.Content = initZero.ToString();
			this.turn = turn_check;
			this.xmlData = new xmlRun();
			this.xmlData.LoadCard();
			this.round = 1;
			check1.Visibility = Visibility.Collapsed;
			check2.Visibility = Visibility.Collapsed;
			check3.Visibility = Visibility.Collapsed;
			check4.Visibility = Visibility.Collapsed;
			check5.Visibility = Visibility.Collapsed;
			checkTurn();

			en_choose1.Visibility = Visibility.Collapsed;
			en_choose2.Visibility = Visibility.Collapsed;
			en_choose3.Visibility = Visibility.Collapsed;
			en_choose4.Visibility = Visibility.Collapsed;
			en_choose5.Visibility = Visibility.Collapsed;

		}

		public void setYours(string name,string id,string card1,string card2,string card3,string card4,string card5)
		{
			yourName.Content = name;
			this.yourID = id;
			this.my_card1 = card1;
			this.my_card2 = card2;
			this.my_card3 = card3;
			this.my_card4 = card4;
			this.my_card5 = card5;
			string convert1 = "CardImg/Ver1/" + card1 + ".png";
			string convert2 = "CardImg/Ver1/" + card2 + ".png";
			string convert3 = "CardImg/Ver1/" + card3 + ".png";
			string convert4 = "CardImg/Ver1/" + card4 + ".png";
			string convert5 = "CardImg/Ver1/" + card5 + ".png";
			my1.Source = new BitmapImage(new Uri(@convert1,UriKind.Relative));
			my2.Source = new BitmapImage(new Uri(@convert2, UriKind.Relative));
			my3.Source = new BitmapImage(new Uri(@convert3, UriKind.Relative));
			my4.Source = new BitmapImage(new Uri(@convert4, UriKind.Relative));
			my5.Source = new BitmapImage(new Uri(@convert5, UriKind.Relative));
		}

		public void setEnemy(string name,string card1, string card2, string card3, string card4, string card5)
		{
			enemyName.Content = name;
			this.en_card1 = card1;
			this.en_card2 = card2;
			this.en_card3 = card3;
			this.en_card4 = card4;
			this.en_card5 = card5;

			if(Convert.ToInt32(this.xmlData.getCardInfo(this.en_card1)[2])==180)
			{
				en1.Source = new BitmapImage(new Uri("CardImg/Default/back1.png", UriKind.Relative));
			}
			else if(Convert.ToInt32(this.xmlData.getCardInfo(this.en_card1)[2]) == 210)
			{
				en1.Source = new BitmapImage(new Uri("CardImg/Default/back2.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card1)[2]) == 260)
			{
				en1.Source = new BitmapImage(new Uri("CardImg/Default/back3.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card1)[2]) == 300)
			{
				en1.Source = new BitmapImage(new Uri("CardImg/Default/back4.png", UriKind.Relative));
			}
			//
			if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card2)[2]) == 180)
			{
				en2.Source = new BitmapImage(new Uri("CardImg/Default/back1.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card2)[2]) == 210)
			{
				en2.Source = new BitmapImage(new Uri("CardImg/Default/back2.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card2)[2]) == 260)
			{
				en2.Source = new BitmapImage(new Uri("CardImg/Default/back3.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card2)[2]) == 300)
			{
				en2.Source = new BitmapImage(new Uri("CardImg/Default/back4.png", UriKind.Relative));
			}
			//
			if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card3)[2]) == 180)
			{
				en3.Source = new BitmapImage(new Uri("CardImg/Default/back1.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card3)[2]) == 210)
			{
				en3.Source = new BitmapImage(new Uri("CardImg/Default/back2.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card3)[2]) == 260)
			{
				en3.Source = new BitmapImage(new Uri("CardImg/Default/back3.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card3)[2]) == 300)
			{
				en3.Source = new BitmapImage(new Uri("CardImg/Default/back4.png", UriKind.Relative));
			}
			//
			if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card4)[2]) == 180)
			{
				en4.Source = new BitmapImage(new Uri("CardImg/Default/back1.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card4)[2]) == 210)
			{
				en4.Source = new BitmapImage(new Uri("CardImg/Default/back2.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card4)[2]) == 260)
			{
				en4.Source = new BitmapImage(new Uri("CardImg/Default/back3.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card4)[2]) == 300)
			{
				en4.Source = new BitmapImage(new Uri("CardImg/Default/back4.png", UriKind.Relative));
			}
			//
			if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card5)[2]) == 180)
			{
				en5.Source = new BitmapImage(new Uri("CardImg/Default/back1.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card5)[2]) == 210)
			{
				en5.Source = new BitmapImage(new Uri("CardImg/Default/back2.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card5)[2]) == 260)
			{
				en5.Source = new BitmapImage(new Uri("CardImg/Default/back3.png", UriKind.Relative));
			}
			else if (Convert.ToInt32(this.xmlData.getCardInfo(this.en_card5)[2]) == 300)
			{
				en5.Source = new BitmapImage(new Uri("CardImg/Default/back4.png", UriKind.Relative));
			}
		}

		public void storeChoose(string id)
		{
			this.tempChooseID = id;
			if (this.tempChooseID.Equals(this.en_card1))
			{
				en_choose1.Visibility = Visibility.Visible;
			}
			else if (this.tempChooseID.Equals(this.en_card2))
			{
				en_choose2.Visibility = Visibility.Visible;
			}
			else if (this.tempChooseID.Equals(this.en_card3))
			{
				en_choose3.Visibility = Visibility.Visible;
			}
			else if (this.tempChooseID.Equals(this.en_card4))
			{
				en_choose4.Visibility = Visibility.Visible;
			}
			else if (this.tempChooseID.Equals(this.en_card5))
			{
				en_choose5.Visibility = Visibility.Visible;
			}
		}

		public void showEnCard()
		{
			en_choose1.Visibility = Visibility.Collapsed;
			en_choose2.Visibility = Visibility.Collapsed;
			en_choose3.Visibility = Visibility.Collapsed;
			en_choose4.Visibility = Visibility.Collapsed;
			en_choose5.Visibility = Visibility.Collapsed;

			string uri = "CardImg/Ver1/" + this.tempChooseID + ".png";

			if (this.tempChooseID.Equals(this.en_card1))
			{
				en1.Source = new BitmapImage(new Uri(@uri, UriKind.Relative));
			}
			else if(this.tempChooseID.Equals(this.en_card2))
			{
				en2.Source = new BitmapImage(new Uri(@uri, UriKind.Relative));
			}
			else if (this.tempChooseID.Equals(this.en_card3))
			{
				en3.Source = new BitmapImage(new Uri(@uri, UriKind.Relative));
			}
			else if (this.tempChooseID.Equals(this.en_card4))
			{
				en4.Source = new BitmapImage(new Uri(@uri, UriKind.Relative));
			}
			else if (this.tempChooseID.Equals(this.en_card5))
			{
				en5.Source = new BitmapImage(new Uri(@uri, UriKind.Relative));
			}

			this.tempChooseID = "";
		}

		public void checkTurn()
		{
			if(turn)
			{
				SignalLabel.Content = "你的回合!";
				if(imgCheck1)
				{
					my1.IsEnabled = true;
				}
				if(imgCheck2)
				{
					my2.IsEnabled = true;
				}
				if(imgCheck3)
				{
					my3.IsEnabled = true;

				}
				if(imgCheck4)
				{
					my4.IsEnabled = true;

				}
				if(imgCheck5)
				{
					my5.IsEnabled = true;

				}
			}
			else
			{
				//wait
				SignalLabel.Content = "對手的回合!";
				my1.IsEnabled = false;
				my2.IsEnabled = false;
				my3.IsEnabled = false;
				my4.IsEnabled = false;
				my5.IsEnabled = false;
			}
		}



		public void EnemyChoose(string id)
		{
			this.enLog.Add(id);
			checkTurnMatch();
			CompareCard();
			checkTurn();
		}

		public void youChoose(string id)
		{
			this.youLog.Add(id);
			checkTurnMatch();
			CompareCard();
			checkTurn();
		}

		public void checkTurnMatch()
		{
			if(this.youLog.Count!=this.enLog.Count)
			{
				if(this.youLog.Count>this.enLog.Count)
				{
					this.turn = false;
				}
				else
				{
					this.turn = true;
				}
			}
			else
			{
				if(this.turn)
				{
					this.turn = false;
				}
				else
				{
					this.turn = true;
				}
			}
		}

		public void CompareCard()
		{
			if((this.enLog.Count==this.youLog.Count)&&this.enLog.Count>0&&this.youLog.Count>0)
			{
				showEnCard();
				string[] enemyCard = this.xmlData.getCardInfo(this.enLog[enLog.Count-1]);
				string[] yourCard = this.xmlData.getCardInfo(this.youLog[youLog.Count-1]);
				if (yourCard[3].Equals("Pap"))
				{
					if (enemyCard[3].Equals("Pap"))
					{
						//Tie
						myPts.Content = (Convert.ToInt32(myPts.Content) + Convert.ToInt32(yourCard[2])).ToString();
						enPts.Content = (Convert.ToInt32(enPts.Content) + Convert.ToInt32(enemyCard[2])).ToString();
					}
					else if (enemyCard[3].Equals("Sic"))
					{
						//Lose
						enPts.Content = (Convert.ToInt32(enPts.Content) + Convert.ToInt32(enemyCard[2]) + 50).ToString();
					}
					else if (enemyCard[3].Equals("Roc"))
					{
						//Win
						myPts.Content = (Convert.ToInt32(myPts.Content) + Convert.ToInt32(yourCard[2]) + 50).ToString();
					}
				}
				else if (yourCard[3].Equals("Sic"))
				{
					if (enemyCard[3].Equals("Pap"))
					{
						//Win
						myPts.Content = (Convert.ToInt32(myPts.Content) + Convert.ToInt32(yourCard[2]) + 50).ToString();
					}
					else if (enemyCard[3].Equals("Sic"))
					{
						//Tie
						myPts.Content = (Convert.ToInt32(myPts.Content) + Convert.ToInt32(yourCard[2])).ToString();
						enPts.Content = (Convert.ToInt32(enPts.Content) + Convert.ToInt32(enemyCard[2])).ToString();
					}
					else if (enemyCard[3].Equals("Roc"))
					{
						//Lose
						enPts.Content = (Convert.ToInt32(enPts.Content) + Convert.ToInt32(enemyCard[2]) + 50).ToString();
					}
				}
				else if (yourCard[3].Equals("Roc"))
				{
					if (enemyCard[3].Equals("Pap"))
					{
						//Lose
						enPts.Content = (Convert.ToInt32(enPts.Content) + Convert.ToInt32(enemyCard[2]) + 50).ToString();
					}
					else if (enemyCard[3].Equals("Sic"))
					{
						//Win
						myPts.Content = (Convert.ToInt32(myPts.Content) + Convert.ToInt32(yourCard[2]) + 50).ToString();
					}
					else if (enemyCard[3].Equals("Roc"))
					{
						//Tie
						myPts.Content = (Convert.ToInt32(myPts.Content) + Convert.ToInt32(yourCard[2])).ToString();
						enPts.Content = (Convert.ToInt32(enPts.Content) + Convert.ToInt32(enemyCard[2])).ToString();
					}
				}

				if(this.turn)
				{
					this.turn = false;
				}
				else
				{
					this.turn = true;
				}

				this.round++;
				if (this.round > 5)
				{
					EndGame();
				}
			}
		}


		public void EndGame()
		{
			int you_sum = Convert.ToInt32(myPts.Content);
			int en_sum = Convert.ToInt32(enPts.Content);
			if(you_sum > en_sum)
			{
				MessageBox.Show(yourName.Content+" 恭喜你!獲得了勝利!");
				database db = new database();
				db.dbEditRecord(this.yourID,1);
				db.dbEditCoin(this.yourID, 500);
				System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
				Application.Current.Shutdown();
			}
			else if(you_sum == en_sum)
			{
				MessageBox.Show("這是一場平局!你們不相上下!");
				database db = new database();
				db.dbEditRecord(this.yourID, 0);
				db.dbEditCoin(this.yourID, 300);
				System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
				Application.Current.Shutdown();
			}
			else
			{
				MessageBox.Show("可惡!運氣不好!下次再來!");
				database db = new database();
				db.dbEditRecord(this.yourID, -1);
				db.dbEditCoin(this.yourID, 100);
				System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
				Application.Current.Shutdown();
			}
		}
	}
}
