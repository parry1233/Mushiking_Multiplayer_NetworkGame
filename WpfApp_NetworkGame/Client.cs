using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp_NetworkGame
{
	class Client
	{
		private Socket clientSocket;
		private string[] info;
		private Socket oppositePeer;
		public Client()
		{
			this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.info = new string[8];
		}

		public void clientConnect(string ip)
		{
			try
			{
				//埠及IP
				IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), int.Parse("10666"));
				//建立套接字
				//this.client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				//開始連線到伺服器
				this.clientSocket.BeginConnect(ipe, asyncResult =>
				{
					try
					{
						this.clientSocket.EndConnect(asyncResult);
						//向伺服器傳送訊息
						clientSend("ONLINE");
						//接受訊息
						clientReceive(this.clientSocket);
					}
					catch (SocketException e)
					{
						MessageBox.Show("客戶端錯誤回報: " + e.Message, "SocketERR");
					}
				}, null);

			}
			catch (SocketException e)
			{
				MessageBox.Show("客戶端錯誤回報01: " + e.Message, "SocketERR");
			}
			catch (Exception e)
			{
				MessageBox.Show("客戶端錯誤回報02: " + e.Message, "ERR");
			}
		}

		public void clientSend(string msg)
		{
			if (this.clientSocket == null || msg == string.Empty)
				return;

			byte[] data = Encoding.UTF8.GetBytes(msg);
			try
			{
				this.clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
				{
					//完成傳送訊息
					int length = this.clientSocket.EndSend(asyncResult);
				}, null);
			}
			catch(Exception e)
			{
				MessageBox.Show("clientSend_ERR: " + e.Message, "ERR");
			}
		}

		public void clientReceive(Socket socket)
		{
			byte[] data = new byte[1024];
			try
			{
				string rcv = "";
				socket.BeginReceive(data, 0, data.Length, SocketFlags.None,
					asyncResult =>
					{

						try
						{
							char[] delimiterChars = { '/' };


							int length = socket.EndReceive(asyncResult);
							rcv = Encoding.UTF8.GetString(data);
							int i = rcv.IndexOf('\0');
							if (i >= 0)
							{
								rcv = rcv.Substring(0, i);
							}

							string[] instruction = rcv.Split(delimiterChars);
							App.Current.Dispatcher.Invoke((Action)(() =>
							{
								if(instruction[0].Equals("WELCOME"))
								{
									foreach (Window win in App.Current.Windows)
									{
										if (win.GetType() == typeof(IPConfirmWindow))
										{
											(win as IPConfirmWindow).wait();
										}
									}
								}
								else if(instruction[0].Equals("LEAVE_PERMIT"))
								{
									foreach (Window win in App.Current.Windows)
									{
										if (win.GetType() == typeof(IPConfirmWindow))
										{
											(win as IPConfirmWindow).leave();
										}
									}
								}
								else if(instruction[0].Equals("PLAY"))
								{
									this.oppositePeer = socket;
									string[] yourInfo = new string[7];
									foreach (Window win in App.Current.Windows)
									{
										if (win.GetType() == typeof(IPConfirmWindow))
										{
											yourInfo[0] = (win as IPConfirmWindow).getInfo()[7].Split('/')[0];//name
											yourInfo[1] = (win as IPConfirmWindow).getInfo()[0];//id
											yourInfo[2] = (win as IPConfirmWindow).getInfo()[6].Split('/')[0];//card1
											yourInfo[3] = (win as IPConfirmWindow).getInfo()[6].Split('/')[1];//card2
											yourInfo[4] = (win as IPConfirmWindow).getInfo()[6].Split('/')[2];//card3
											yourInfo[5] = (win as IPConfirmWindow).getInfo()[6].Split('/')[3];//card4
											yourInfo[6] = (win as IPConfirmWindow).getInfo()[6].Split('/')[4];//card5
										}
									}
									clientSend("PLAY_COMMIT/"+yourInfo[0]+"/"+yourInfo[2]+"/"+yourInfo[3]+"/"+yourInfo[4]+"/"+yourInfo[5]+"/"+yourInfo[6]);
									App.Current.Dispatcher.Invoke((Action)(() =>
									{
										GameWindow gameWindow = new GameWindow(false);
										gameWindow.setYours(yourInfo[0],yourInfo[1],yourInfo[2],yourInfo[3],yourInfo[4],yourInfo[5], yourInfo[6]);
										gameWindow.setEnemy(instruction[1],instruction[2],instruction[3],instruction[4],instruction[5],instruction[6]);
										gameWindow.my1.MouseDown += my_MouseDown;
										gameWindow.my2.MouseDown += my_MouseDown;
										gameWindow.my3.MouseDown += my_MouseDown;
										gameWindow.my4.MouseDown += my_MouseDown;
										gameWindow.my5.MouseDown += my_MouseDown;
										gameWindow.Show();
										foreach (Window win in App.Current.Windows)
										{
											if (win.GetType() != typeof(GameWindow))
											{
												win.Close();
											}
										}
									}));
								}
								else if (instruction[0].Equals("CHOOSE"))
								{
									App.Current.Dispatcher.Invoke((Action)(() =>
									{
										foreach (Window win in App.Current.Windows)
										{
											if (win.GetType() == typeof(GameWindow))
											{
												(win as GameWindow).storeChoose(instruction[1]);
												(win as GameWindow).EnemyChoose(instruction[1]);
											}
										}
									}));
								}
								else
								{
									MessageBox.Show(rcv);
								}
							}));
						}
						catch(Exception e)
						{
							MessageBox.Show("RCV_async_ERR: " + e.Message, "ASYNC_RCV_ERR");
						}

						clientReceive(socket);
					}, null);
			}
			catch(Exception e)
			{
				MessageBox.Show("clientReceiveERR: " + e.Message, "RCV_ERR");
			}
		}

		private void my_MouseDown(object sender, MouseButtonEventArgs e)
		{
			string send = "";
			foreach (Window win in App.Current.Windows)
			{
				if (win.GetType() == typeof(GameWindow))
				{
					if ((sender as Image).Name.Equals("my1"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card1);
						(win as GameWindow).check1.Visibility = Visibility.Visible;
						(win as GameWindow).my1.Opacity = 0.5;
						(win as GameWindow).imgCheck1 = false;
						send = (win as GameWindow).my_card1;
					}
					else if ((sender as Image).Name.Equals("my2"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card2);
						(win as GameWindow).check2.Visibility = Visibility.Visible;
						(win as GameWindow).my2.Opacity = 0.5;
						(win as GameWindow).imgCheck2 = false;
						send = (win as GameWindow).my_card2;
					}
					else if ((sender as Image).Name.Equals("my3"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card3);
						(win as GameWindow).check3.Visibility = Visibility.Visible;
						(win as GameWindow).my3.Opacity = 0.5;
						(win as GameWindow).imgCheck3 = false;
						send = (win as GameWindow).my_card3;
					}
					else if ((sender as Image).Name.Equals("my4"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card4);
						(win as GameWindow).check4.Visibility = Visibility.Visible;
						(win as GameWindow).my4.Opacity = 0.5;
						(win as GameWindow).imgCheck4 = false;
						send = (win as GameWindow).my_card4;
					}
					else if ((sender as Image).Name.Equals("my5"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card5);
						(win as GameWindow).check5.Visibility = Visibility.Visible;
						(win as GameWindow).my5.Opacity = 0.5;
						(win as GameWindow).imgCheck5 = false;
						send = (win as GameWindow).my_card5;
					}
					else
					{
						MessageBox.Show((sender as Image).Name);
					}
				}
			}

			clientSend("CHOOSE/" + send);
		}
	}
}
