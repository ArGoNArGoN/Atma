using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



namespace ClientChatWPF
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		event Action<List<Message>> MyEvent;
		List<TextChat> TextChats { get; set; }
		static User User { get; set; }
		private const string host = "127.0.0.1";
		private const int port = 8888;
		static TcpClient Client { get; set; }
		static NetworkStream Stream { get; set; }
		BinaryFormatter formatter = new BinaryFormatter();

		public MainWindow()
		{
			Reg reg = new Reg();
			reg.ShowDialog();
			User = reg.User;

			InitializeComponent();

			ConnectServer();

			listTextChat.ItemsSource = TextChats.Select(x => x.ID.ToString() + " "+ x.Name.ToString());
			Messages = TextChats[0].Message.ToList();
			listUserMessage.ItemsSource = Messages;

			this.Loaded += new RoutedEventHandler(Form1_Load);
		}

		List<Message> Messages { get; set; }

        private void Form1_Load(object sender, RoutedEventArgs e)
        {
			this.MyEvent += new Action<List<Message>>(Form1_MyEvent);
			Thread thr = new Thread(new ThreadStart(GetMessage));
			thr.IsBackground = true;
			thr.Start();
		}

        private void GetMessage()
		{
			do
			{
				TextChats[0].Message.Add(GetMessageSerialize());
				MyEvent(TextChats[0].Message.ToList());
			} while (true);
		}

        private void Form1_MyEvent(List<Message> obj)
		{
			listUserMessage.Dispatcher
				.Invoke(new Action(
					() =>
						listUserMessage.ItemsSource = obj
					));
		}

        private void ConnectServer()
		{
			Client = new TcpClient();

			try
			{
				Client.Connect(host, port);
				Stream = Client.GetStream();
				formatter.Serialize(Stream, User);

				TextChats = ((TextChat[])formatter.Deserialize(Stream)).ToList();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(MessagePop.Text))
				return;
			SendMessageSerialize(new Message() { IDUser = User.ID, IDTextChat = 1, Text = MessagePop.Text, Date = DateTime.Now });
		}

		private void SendMessageSerialize(Message message)
			=> formatter.Serialize(Stream, message);
		private Message GetMessageSerialize()
			=> (Message)formatter.Deserialize(Stream);
	}
}
