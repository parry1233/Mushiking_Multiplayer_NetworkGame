using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WpfApp_NetworkGame
{
	class database
	{
		static string dbHost = "127.0.0.1";
		static string dbPort = "3306";
		static string dbUser = "parry1233";
		static string dbPass = "parry1233";
		/*static string dbHost = "ccuigo.mysql.database.azure.com";
		static string dbPort = "3306";
		static string dbUser = "parry1233@ccuigo";
		static string dbPass = "Parry1000033";*/
		static string dbName = "mushiking";
		static string conn_info = "server=" + dbHost + ";port=" + dbPort + ";user=" + dbUser + ";password=" + dbPass + ";database=" + dbName + ";charset=utf8;oldguids=true;";
		public database()
		{

		}

		public void dbConnect()
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string tryCMD = "SELECT * FROM user";
					MySqlCommand cmd = new MySqlCommand(tryCMD, conn);
					MySqlDataReader dataRead = cmd.ExecuteReader();

					if (dataRead.HasRows)
					{
						
					}
					else
					{

					}
				}
			}
			catch (MySql.Data.MySqlClient.MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						//LogTextBox.Text += "[ SERVER ] Unpredicted incident occured. Fail to connect to database.\r\n";
						break;
					case 1042:
						//LogTextBox.Text += "[ SERVER ] IP error. Please check again.\r\n";
						break;
					case 1045:
						//LogTextBox.Text += "[ SERVER ] User account or password error. Please check again\r\n";
						break;
				}
			}
		}

		public void dbLogIn(string id, string pw)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					bool check_loginSuccess = false;
					conn.Open();
					string logInCMD = "SELECT * FROM user WHERE User_ID = binary @ID_in";
					MySqlCommand cmd = new MySqlCommand(logInCMD, conn);
					cmd.Parameters.AddWithValue("@ID_in", id);
					MySqlDataReader dataRead = cmd.ExecuteReader();

					if (dataRead.HasRows)
					{
						while (dataRead.Read())
						{
							if (dataRead["User_PW"].Equals(pw))
							{
								check_loginSuccess = true;

								string[] userData = new string[9];
								userData[0] = dataRead["User_ID"].ToString();
								userData[1] = dataRead["User_PW"].ToString();
								userData[2] = dataRead["User_Grade"].ToString();
								userData[3] = dataRead["User_Title"].ToString();
								userData[4] = dataRead["User_CardData"].ToString();
								userData[5] = dataRead["User_Record"].ToString();
								userData[6] = dataRead["User_Team"].ToString();
								userData[7] = dataRead["User_Info"].ToString();
								userData[8] = dataRead["User_Coin"].ToString();

								MainWindow mainWindow = new MainWindow();
								mainWindow.userData(userData);
								mainWindow.Show();

								foreach (Window win in App.Current.Windows)
								{
									if (win.GetType() != typeof(MainWindow))
									{
										win.Close();
									}
								}
							}
							else
							{
								MessageBox.Show("登入失敗!密碼錯誤!");
								foreach (Window win in App.Current.Windows)
								{
									if (win.GetType() == typeof(LogInWindow))
									{
										(win as LogInWindow).logInBTN.IsEnabled = true;
									}
								}
							}
						}
					}
					else
					{
						MessageBox.Show("登入失敗!不存在此帳號!");
						foreach (Window win in App.Current.Windows)
						{
							if (win.GetType() == typeof(LogInWindow))
							{
								(win as LogInWindow).logInBTN.IsEnabled = true;
							}
						}
					}
				}
			}
			catch (MySql.Data.MySqlClient.MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("[DB] Unpredicted incident occured. Fail to connect to database");
						break;
					case 1042:
						MessageBox.Show("[DB] IP error. Please check again.");
						break;
					case 1045:
						MessageBox.Show("[DB] db log in falied( User account or password error). Please check again");
						break;
				}
			}
		}

		public void dbEditCard(string id,string edit)
		{
			try
			{
				string currentCard = "";
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string logInCMD = "SELECT * FROM user WHERE User_ID = binary @ID_in";
					MySqlCommand cmd = new MySqlCommand(logInCMD, conn);
					cmd.Parameters.AddWithValue("@ID_in", id);
					MySqlDataReader dataRead = cmd.ExecuteReader();

					if (dataRead.HasRows)
					{
						string[] userData = new string[8];
						while (dataRead.Read())
						{
							currentCard = dataRead["User_CardData"].ToString();
						}
					}
					else
					{
						MessageBox.Show("讀取失敗!不存在此帳號!");
						foreach (Window win in App.Current.Windows)
						{
							if (win.GetType() != typeof(LogInWindow))
							{
								(win as LogInWindow).logInBTN.IsEnabled = true;
							}
						}
					}
				}
				currentCard += "/" + edit;
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string edit_CMD = "UPDATE user SET User_CardData = @Card_update WHERE User_ID = binary @ID";
					MySqlCommand cmd = new MySqlCommand(edit_CMD, conn);
					cmd.Parameters.AddWithValue("@Card_update", currentCard);
					cmd.Parameters.AddWithValue("@ID", id);
					cmd.ExecuteNonQuery();
				}
			}
			catch (MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("[DB] Unpredicted incident occured. Fail to connect to database.");
						break;
					case 1042:
						MessageBox.Show("[DB] IP error. Please check again.");
						break;
					case 1045:
						MessageBox.Show("[DB] db User account or password error. Please check again");
						break;
					case 1062:
						MessageBox.Show("[DB] primary key already existed. Please check again");
						break;
					case 1366:
						MessageBox.Show("[DB] Incorrect value while INSERT, cannot insert to MySQL");
						break;
				}
			}
		}

		public void dbEditTeam(string id, string edit)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string edit_CMD = "UPDATE user SET User_Team = @Team_update WHERE User_ID = binary @ID";
					MySqlCommand cmd = new MySqlCommand(edit_CMD, conn);
					cmd.Parameters.AddWithValue("@Team_update", edit);
					cmd.Parameters.AddWithValue("@ID", id);
					cmd.ExecuteNonQuery();
				}
			}
			catch (MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("[DB] Unpredicted incident occured. Fail to connect to database.");
						break;
					case 1042:
						MessageBox.Show("[DB] IP error. Please check again.");
						break;
					case 1045:
						MessageBox.Show("[DB] db User account or password error. Please check again");
						break;
					case 1062:
						MessageBox.Show("[DB] primary key already existed. Please check again");
						break;
					case 1366:
						MessageBox.Show("[DB] Incorrect value while INSERT, cannot insert to MySQL");
						break;
				}
			}
		}

		public void dbEditRecord(string id, int edit)
		{
			try
			{
				string currentRecord = "";
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string logInCMD = "SELECT * FROM user WHERE User_ID = binary @ID_in";
					MySqlCommand cmd = new MySqlCommand(logInCMD, conn);
					cmd.Parameters.AddWithValue("@ID_in", id);
					MySqlDataReader dataRead = cmd.ExecuteReader();

					if (dataRead.HasRows)
					{
						string[] userData = new string[8];
						while (dataRead.Read())
						{
							currentRecord = dataRead["User_Record"].ToString();
						}
					}
					else
					{
						MessageBox.Show("讀取失敗!不存在此帳號!");
						foreach (Window win in App.Current.Windows)
						{
							if (win.GetType() != typeof(LogInWindow))
							{
								(win as LogInWindow).logInBTN.IsEnabled = true;
							}
						}
					}
				}
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string[] WinLoseTie = currentRecord.Split('/');
					if (edit == 1)
					{
						//win
						WinLoseTie[0] = (Convert.ToInt32(WinLoseTie[0]) + 1).ToString();
					}
					else if (edit == 0)
					{
						//tie
						WinLoseTie[2] = (Convert.ToInt32(WinLoseTie[2]) + 1).ToString();
					}
					else if (edit == -1)
					{
						//lose
						WinLoseTie[1] = (Convert.ToInt32(WinLoseTie[1]) + 1).ToString();
					}
					string edit_CMD = "UPDATE user SET User_Record = @record_update WHERE User_ID = binary @ID";
					MySqlCommand cmd = new MySqlCommand(edit_CMD, conn);
					cmd.Parameters.AddWithValue("@record_update", WinLoseTie[0]+"/"+WinLoseTie[1]+"/"+WinLoseTie[2]);
					cmd.Parameters.AddWithValue("@ID", id);
					cmd.ExecuteNonQuery();
				}
			}
			catch (MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("[DB] Unpredicted incident occured. Fail to connect to database.");
						break;
					case 1042:
						MessageBox.Show("[DB] IP error. Please check again.");
						break;
					case 1045:
						MessageBox.Show("[DB] db User account or password error. Please check again");
						break;
					case 1062:
						MessageBox.Show("[DB] primary key already existed. Please check again");
						break;
					case 1366:
						MessageBox.Show("[DB] Incorrect value while INSERT, cannot insert to MySQL");
						break;
				}
			}
		}

		public void dbEditCoin(string id, int num)
		{
			try
			{
				int currentCoin = 0;
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string logInCMD = "SELECT * FROM user WHERE User_ID = binary @ID_in";
					MySqlCommand cmd = new MySqlCommand(logInCMD, conn);
					cmd.Parameters.AddWithValue("@ID_in", id);
					MySqlDataReader dataRead = cmd.ExecuteReader();

					if (dataRead.HasRows)
					{
						string[] userData = new string[8];
						while (dataRead.Read())
						{
							currentCoin = Convert.ToInt32(dataRead["User_Coin"].ToString());
						}
					}
					else
					{
						MessageBox.Show("讀取失敗!不存在此帳號!");
						foreach (Window win in App.Current.Windows)
						{
							if (win.GetType() != typeof(LogInWindow))
							{
								(win as LogInWindow).logInBTN.IsEnabled = true;
							}
						}
					}
				}
				currentCoin += num;
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string edit_CMD = "UPDATE user SET User_Coin = @Coin_update WHERE User_ID = binary @ID";
					MySqlCommand cmd = new MySqlCommand(edit_CMD, conn);
					cmd.Parameters.AddWithValue("@Coin_update", currentCoin.ToString());
					cmd.Parameters.AddWithValue("@ID", id);
					cmd.ExecuteNonQuery();
				}
			}
			catch (MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("[DB] Unpredicted incident occured. Fail to connect to database.");
						break;
					case 1042:
						MessageBox.Show("[DB] IP error. Please check again.");
						break;
					case 1045:
						MessageBox.Show("[DB] db User account or password error. Please check again");
						break;
					case 1062:
						MessageBox.Show("[DB] primary key already existed. Please check again");
						break;
					case 1366:
						MessageBox.Show("[DB] Incorrect value while INSERT, cannot insert to MySQL");
						break;
				}
			}
		}

		public void dbEditInfo(string id, string edit)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string edit_CMD = "UPDATE user SET User_Info = @Info_update WHERE User_ID = binary @ID";
					MySqlCommand cmd = new MySqlCommand(edit_CMD, conn);
					cmd.Parameters.AddWithValue("@Info_update", edit);
					cmd.Parameters.AddWithValue("@ID", id);
					cmd.ExecuteNonQuery();
				}
			}
			catch (MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("[DB] Unpredicted incident occured. Fail to connect to database.");
						break;
					case 1042:
						MessageBox.Show("[DB] IP error. Please check again.");
						break;
					case 1045:
						MessageBox.Show("[DB] db User account or password error. Please check again");
						break;
					case 1062:
						MessageBox.Show("[DB] primary key already existed. Please check again");
						break;
					case 1366:
						MessageBox.Show("[DB] Incorrect value while INSERT, cannot insert to MySQL");
						break;
				}
			}
		}

		public string[] dbGetInfo(string id)
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(conn_info))
				{
					conn.Open();
					string logInCMD = "SELECT * FROM user WHERE User_ID = binary @ID_in";
					MySqlCommand cmd = new MySqlCommand(logInCMD, conn);
					cmd.Parameters.AddWithValue("@ID_in", id);
					MySqlDataReader dataRead = cmd.ExecuteReader();

					if (dataRead.HasRows)
					{
						string[] userData = new string[8];
						while (dataRead.Read())
						{
							userData[0] = dataRead["User_ID"].ToString();
							userData[1] = dataRead["User_PW"].ToString();
							userData[2] = dataRead["User_Grade"].ToString();
							userData[3] = dataRead["User_Title"].ToString();
							userData[4] = dataRead["User_CardData"].ToString();
							userData[5] = dataRead["User_Record"].ToString();
							userData[6] = dataRead["User_Team"].ToString();
							userData[7] = dataRead["User_Info"].ToString();
						}
						return userData;
					}
					else
					{
						MessageBox.Show("讀取失敗!不存在此帳號!");
						foreach (Window win in App.Current.Windows)
						{
							if (win.GetType() != typeof(LogInWindow))
							{
								(win as LogInWindow).logInBTN.IsEnabled = true;
							}
						}
						return new string[8];
					}
				}
			}
			catch (MySql.Data.MySqlClient.MySqlException ex)
			{
				switch (ex.Number)
				{
					case 0:
						MessageBox.Show("[DB] Unpredicted incident occured. Fail to connect to database");
						break;
					case 1042:
						MessageBox.Show("[DB] IP error. Please check again.");
						break;
					case 1045:
						MessageBox.Show("[DB] db log in falied( User account or password error). Please check again");
						break;
				}
				return new string[8];
			}
		}
	}
}
