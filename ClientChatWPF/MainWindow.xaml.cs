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
		event Action<List<Message>> EventAddMessage;
		event Action<User> EventUpUserStatus;



		List<TextChat> TextChats { get; set; }
		static User User { get; set; }
		static NetworkStream Stream { get; set; }
		BinaryFormatter formatter = new BinaryFormatter();
		public MainWindow()
		{
			Reg reg = new Reg();
			reg.ShowDialog();
			
			User = reg.User;
			Stream = reg.Stream;

			if (reg.User == null)
			{ Close(); return; }

			InitializeComponent();

			LoadedDataInWindow();

			this.Loaded += new RoutedEventHandler(LoadInfoServer);
		}

		private void LoadedDataInWindow()
		{
			Server = User.ServerUser.ToList()[0].Server;

			ListUserOnline.ItemsSource = UsersOnline = Server.ServerUser.Select(x => x.User).Where(x => x.Status == Status.Online).ToList();
			ListUserOffline.ItemsSource = UsersOffline = Server.ServerUser.Select(x => x.User).Where(x => x.Status == Status.Offline).ToList();

			listTextChat.ItemsSource = Server.TextChat.Select(x => x.Name);

			listUserMessage.ItemsSource = Messages = new List<Message>();
		}

		List<Message> Messages { get; set; }
		List<User> UsersOnline { get; set; }
		List<User> UsersOffline { get; set; }
		public Server Server { get; private set; }

		private void LoadInfoServer(object sender, RoutedEventArgs e)
		{
			this.EventAddMessage += new Action<List<Message>>(AddMessageInListbox);
			this.EventUpUserStatus += new Action<User>(UpUserStatusInListBoxs);

			Thread thr = new Thread(new ThreadStart(TakeMessageOfServer));
			thr.IsBackground = true;
			thr.Start();
		}

		private void TakeMessageOfServer()
		{
			Messages = Server.TextChat.ToList()[0].Message.ToList();
			EventAddMessage(Messages);
			do
			{

				Object ob = GetMessageSerialize();

				switch (ob)
				{
					case (Message):
						Server.TextChat.ToList()[0].Message.Add((Message)ob);
						EventAddMessage(Server.TextChat.ToList()[0].Message.ToList());
						break;
					case (ClassesForServerClent.Class.User):
						EventUpUserStatus((User)ob);
						break;
					default:
						break;
				}
			} while (true);
		}

		private void AddMessageInListbox(List<Message> obj)
		{
			listUserMessage.Dispatcher
				.Invoke(new Action(
					() =>
						listUserMessage.ItemsSource = obj
					));
		}


		private void UpUserStatusInListBoxs(User obj)
		{
			ListUserOnline.Dispatcher
				.Invoke
				(
					new Action
					(
						() =>
						{
							if (obj.Status == Status.Offline)
							{
								UsersOnline.RemoveAll(x => x.ID == obj.ID);
								var a = new List<User>();
								a.AddRange(UsersOnline);
								ListUserOnline.ItemsSource = a;
							}
							else
							{
								UsersOnline.Add(obj);
								var a = new List<User>();
								a.AddRange(UsersOnline);
								ListUserOnline.ItemsSource = a;
							}
						}
					)
				);
			ListUserOffline.Dispatcher
				.Invoke
				(
					new Action
					(
						() =>
						{
							if (obj.Status == Status.Online)
							{
								UsersOffline.RemoveAll(x => x.ID == obj.ID);
								var a = new List<User>();
								a.AddRange(UsersOffline);
								ListUserOffline.ItemsSource = a;
							}
							else
							{
								UsersOffline.Add(obj);
								var a = new List<User>();
								a.AddRange(UsersOffline);
								ListUserOffline.ItemsSource = a;
							}
						}
					)
				);
		}


		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(MessagePop.Text))
				return;
			SendMessageSerialize(new Message()
			{
				IDUser = User.ID,
				IDTextChat = 1,
				Text = MessagePop.Text,
				Date = DateTime.Now,
			});
		}
		
		private void SendMessageSerialize(Message message)
			=> formatter.Serialize(Stream, message);
		private Object GetMessageSerialize()
			=> formatter.Deserialize(Stream);
	}
}
