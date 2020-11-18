using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace ClientChatWPF
{
	/// <summary>
	/// Логика взаимодействия для Reg.xaml
	/// </summary>
	public partial class Reg : Window
	{
		public User User;
		public Reg()
		{
			InitializeComponent();
		}
		/// private const string host = "192.168.0.100";
		private const string host = "127.1.0.1";
		private const int port = 22222;
		public TcpClient Client { get; set; }
		public NetworkStream Stream { get; set; }

		private void ClickButton(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(Name.Text) || String.IsNullOrWhiteSpace(Password.Text))
			{ ErrorText.Text = "Некоторые поля оказались незаполненными!"; return; }

			try
			{
				User = new User() 
				{
					RealName = Name.Text,
					Password = Password.Text,
					ActionForServer = ActionForServer.AddDB,
				};

				try
				{
					if(Client is null)
					{
						Client = new TcpClient();
						Client.Connect(host, port);
						Stream = Client.GetStream();
					}
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(Stream, User);
					Object ob = formatter.Deserialize(Stream);

					if (ob is User user)
						User = user;
					
					else if (ob is Exception exception)
						throw new Exception(exception.Message);

					else
						throw new Exception("An unknown error occurred!");

					Close();
				}
				catch(SocketException ex)
                {
					Client = null;
					ErrorText.Text = "There is no Internet connection or the server is currently unavailable!";
					User = null;
				}
				catch (Exception ex)
				{
					ErrorText.Text = ex.Message;
					User = null;
				}
			}
			catch (Exception ex)
			{
				ErrorText.Text = ex.Message;
				User = null;
			}
		}
		private void ClickReg(object sender, RoutedEventArgs e)
		{
			try
			{
				try
				{
					if (Client is null)
					{
						Client = new TcpClient();
						Client.Connect(host, port);
						Stream = Client.GetStream();
					}
				}
				catch(SocketException)
				{
					Client = null;
					ErrorText.Text = "There is no Internet connection or the server is currently unavailable!";
					return;
				}
                Reg2 reg2 = new Reg2
                {
                    Stream = Stream
                };
				reg2.ShowDialog();

				if (reg2.User is null)
					return;

				Name.Text = reg2.User.RealName;
				Password.Text = reg2.User.Password;
				ErrorText.Text = "";
			}
			catch (Exception) { }
			finally
			{
			}
		}
	}
}
