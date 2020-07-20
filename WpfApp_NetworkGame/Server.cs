using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp_NetworkGame
{
	class Server
	{
		private Socket socketListen;
		private Socket socketConnect;
		private string RemoteEndPoint;
		Dictionary<string, Socket> dicClient = new Dictionary<string, Socket>();
		private string[] info;

		private string hostName;
		private IPAddress localIP;
		private Socket oppositePeer;

		public Server()
		{
			this.hostName = Dns.GetHostName();
			this.localIP = Dns.GetHostAddresses(this.hostName)[1];
			this.info = new string[8];
		}

		public string getHostName()
		{
			return this.hostName;
		}

		public string getIP()
		{
			return this.localIP.ToString();
		}

		public void OpenServer()
		{
			try
			{
				TcpClient tcpClient = new TcpClient();
				IPEndPoint ipe = new IPEndPoint(localIP, 6666);
				this.socketListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				this.socketListen.Bind(ipe);
				this.socketListen.Listen(10);

				serverConnect(this.socketListen);
			}
			catch(Exception e)
			{
				MessageBox.Show("OpenServer_ERR: " + e.Message, "ERR");
			}
		}

		public void serverConnect(Socket socket)
		{
			try
			{
				socket.BeginAccept(asyncResult =>
				{
					socketConnect = socket.EndAccept(asyncResult);
					RemoteEndPoint = socketConnect.RemoteEndPoint.ToString();
					dicClient.Add(RemoteEndPoint, socketConnect);
					App.Current.Dispatcher.Invoke((Action)(() =>
					{
						foreach (Window win in App.Current.Windows)
						{
							if (win.GetType() == typeof(LobbyWindow))
							{
								(win as LobbyWindow).AddClient(RemoteEndPoint);
							}
						}
						serverReceive(socketConnect);
						serverConnect(socketListen);
					}));
				}, null);
			}
			catch(Exception e)
			{
				MessageBox.Show("serverConnect_ERR: " + e.Message, "Server ERR");
			}
		}

		public void serverSend(Socket client, string message)
		{
			if (client == null || message == string.Empty) return;
			//資料轉碼
			byte[] data = Encoding.UTF8.GetBytes(message);
			try
			{
				//開始傳送訊息
				client.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
				{
					//完成訊息傳送
					int length = client.EndSend(asyncResult);
				}, null);
			}
			catch (Exception e)
			{
				//傳送失敗，將該客戶端資訊刪除
				string deleteClient = client.RemoteEndPoint.ToString();
				dicClient.Remove(deleteClient);
				App.Current.Dispatcher.Invoke((Action)(() =>
				{
					foreach (Window win in App.Current.Windows)
					{
						if (win.GetType() == typeof(LobbyWindow))
						{
							(win as LobbyWindow).deleteClient(deleteClient);
						}
					}
				}));
				MessageBox.Show("serverSend_ERR: " + e.Message, "Server Send ERR");
			}
		}

		public void serverReceive(Socket socket)
		{
			byte[] data = new byte[1024];
			try
			{
				socket.BeginReceive(data, 0, data.Length, SocketFlags.None,
				asyncResult =>
				{
					try
					{
						int length = socket.EndReceive(asyncResult);
						string rcv = "";
						string[] instruction = { "" };
						char[] delimiterChars = { '/' };
						rcv = Encoding.UTF8.GetString(data);
						int string_count = rcv.IndexOf('\0');
						if (string_count >= 0)
						{
							rcv = rcv.Substring(0, string_count);
						}
						instruction = rcv.Split(delimiterChars);

						if (instruction[0].Equals("ONLINE"))
						{
							serverSend(socket, "WELCOME");
						}
						else if(instruction[0].Equals("LEAVE"))
						{
							serverSend(socket, "LEAVE_PERMIT");
							dicClient.Remove(socket.RemoteEndPoint.ToString());
							App.Current.Dispatcher.Invoke((Action)(() =>
							{
								foreach (Window win in App.Current.Windows)
								{
									if (win.GetType() == typeof(LobbyWindow))
									{
										(win as LobbyWindow).deleteClient(socket.RemoteEndPoint.ToString());
									}
								}
							}));
						}
						else if(instruction[0].Equals("PLAY_COMMIT"))
						{
							this.oppositePeer = socket;
							App.Current.Dispatcher.Invoke((Action)(() =>
							{
								string[] yourInfo = new string[7];
								foreach (Window win in App.Current.Windows)
								{
									if (win.GetType() == typeof(LobbyWindow))
									{
										yourInfo[0] = (win as LobbyWindow).getInfo()[7].Split('/')[0];//name
										yourInfo[1] = (win as LobbyWindow).getInfo()[0];//id
										yourInfo[2] = (win as LobbyWindow).getInfo()[6].Split('/')[0];//card1
										yourInfo[3] = (win as LobbyWindow).getInfo()[6].Split('/')[1];//card2
										yourInfo[4] = (win as LobbyWindow).getInfo()[6].Split('/')[2];//card3
										yourInfo[5] = (win as LobbyWindow).getInfo()[6].Split('/')[3];//card4
										yourInfo[6] = (win as LobbyWindow).getInfo()[6].Split('/')[4];//card5
									}
								}
								GameWindow gameWindow = new GameWindow(true);
								gameWindow.setYours(yourInfo[0], yourInfo[1], yourInfo[2], yourInfo[3], yourInfo[4], yourInfo[5], yourInfo[6]);
								gameWindow.setEnemy(instruction[1], instruction[2], instruction[3], instruction[4], instruction[5], instruction[6]);
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
						else if(instruction[0].Equals("CHOOSE"))
						{
							App.Current.Dispatcher.Invoke((Action)(() =>
							{
								foreach (Window win in App.Current.Windows)
								{
									if (win.GetType() == typeof(GameWindow))
									{
										(win as GameWindow).setIMG(instruction[1]);
										(win as GameWindow).EnemyChoose(instruction[1]);
									}
								}
							}));
						}
						else
						{
							MessageBox.Show(socket.RemoteEndPoint.ToString() + ": " + rcv);
						}
					}
					catch (Exception)
					{
						serverReceive(socket);
					}

					serverReceive(socket);
				}, null);

			}
			catch (Exception e)
			{
				string deleteClient = socket.RemoteEndPoint.ToString();
				dicClient.Remove(deleteClient);
				App.Current.Dispatcher.Invoke((Action)(() =>
				{
					foreach (Window win in App.Current.Windows)
					{
						if (win.GetType() == typeof(LobbyWindow))
						{
							(win as LobbyWindow).deleteClient(deleteClient);
						}
					}
				}));
			}
		}

		public Socket directSocketFind(string remoteEndPoint)
		{
			return this.dicClient[remoteEndPoint];
		}

		private void my_MouseDown(object sender, MouseButtonEventArgs e)
		{
			string send = "";
			foreach (Window win in App.Current.Windows)
			{
				if (win.GetType() == typeof(GameWindow))
				{
					if((sender as Image).Name.Equals("my1"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card1);
						(win as GameWindow).check1.Visibility = Visibility.Visible;
						(win as GameWindow).my1.Opacity = 0.5;
						(win as GameWindow).imgCheck1 = false;
						send = (win as GameWindow).my_card1;
					}
					else if((sender as Image).Name.Equals("my2"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card2);
						(win as GameWindow).check2.Visibility = Visibility.Visible;
						(win as GameWindow).my2.Opacity = 0.5;
						(win as GameWindow).imgCheck2 = false;
						send = (win as GameWindow).my_card2;
					}
					else if((sender as Image).Name.Equals("my3"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card3);
						(win as GameWindow).check3.Visibility = Visibility.Visible;
						(win as GameWindow).my3.Opacity = 0.5;
						(win as GameWindow).imgCheck3 = false;
						send = (win as GameWindow).my_card3;
					}
					else if((sender as Image).Name.Equals("my4"))
					{
						(win as GameWindow).youChoose((win as GameWindow).my_card4);
						(win as GameWindow).check4.Visibility = Visibility.Visible;
						(win as GameWindow).my4.Opacity = 0.5;
						(win as GameWindow).imgCheck4 = false;
						send = (win as GameWindow).my_card4;
					}
					else if((sender as Image).Name.Equals("my5"))
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

			serverSend(this.oppositePeer,"CHOOSE/" + send);
		}
	}
}
