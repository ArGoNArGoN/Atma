using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
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

		List<Int32> IndexAndIdTC = new List<Int32>();
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

			if (User.ServerUser.Count() == 0)
				GetServer();

			LoadedDataInWindow();

			this.Loaded += new RoutedEventHandler(LoadInfoServer);
		}

        private void GetServer()
        {
			ServerSearchWindow server = new ServerSearchWindow(User);
            server.ShowDialog();

			if (User.ServerUser.Count() == 0)
			{
				Close();
				Environment.Exit(0);
			}
        }

        private void LoadedDataInWindow()
		{
			Server = User.ServerUser.ToList()[0].Server;

			ListServers.ItemsSource = User.ServerUser.Select(x => x.Server);

			ListUserOnline.ItemsSource = UsersOnline = Server.ServerUser.Select(x => x.User).Where(x => x.Status == Status.Online).ToList();
			ListUserOffline.ItemsSource = UsersOffline = Server.ServerUser.Select(x => x.User).Where(x => x.Status == Status.Offline).ToList();

			ListTextChat.ItemsSource = Server.TextChat;
            foreach (var item in Server.TextChat) 
				IndexAndIdTC.Add(item.ID);

			ListTextChat.SelectedIndex = 0;
			ListServers.SelectedIndex = 0;
			ListUserMessage.ItemsSource = Messages = new List<Message>();
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
						User.ServerUser.Select(x => x.Server).ToList().ForEach(x1 => x1.TextChat.FirstOrDefault(x => x.ID == ((Message)ob).IDTextChat)?.Message.Add((Message)ob));
						var textChat = Server.TextChat.FirstOrDefault(x => x.ID == ((Message)ob).IDTextChat);
						if (textChat == null)
							break;

						EventAddMessage(textChat.Message.ToList());
						break;
					case (ClassesForServerClent.Class.User):
                        foreach (var item in User.ServerUser.Select(x => x.Server))
                        {
							var s = item.ServerUser.FirstOrDefault(x1 => x1.IDUser == ((User)ob).ID);
							if (s is not null)
							{
								s.User = (User)ob;
							}
						}
						if (Server.ServerUser.Any(x => x.IDUser == ((User)ob).ID))
							EventUpUserStatus((User)ob);
						break;
					default:
						break;
				}
			} while (true);
		}

		private void AddMessageInListbox(List<Message> obj)
		{
			ListUserMessage.Dispatcher
				.Invoke(new Action(
					() =>
					{
						if (obj is null || obj.Count() == 0)
							return;

						if (Server.TextChat.ToList()[ListTextChat.SelectedIndex].ID == obj[0]?.IDTextChat)
							ListUserMessage.ItemsSource = obj;
					}
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

		private void TextChatWasSelected(object sender, SelectionChangedEventArgs e)
		{
			if (ListTextChat.SelectedIndex == -1)
				ListTextChat.SelectedIndex = 0;
			ListUserMessage.ItemsSource = Server.TextChat.ToList()[ListTextChat.SelectedIndex].Message;
		}

		private void SendMessage(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(MessagePop.Text))
				return;

			SendMessageSerialize(new Message()
			{
				IDUser = User.ID,
				IDTextChat = Server.TextChat.ToList()[ListTextChat.SelectedIndex].ID,
				Text = MessagePop.Text,
				Date = DateTime.Now,
			});

			MessagePop.Text = "";
		}
		
		private void SendMessageSerialize(Message message)
			=> formatter.Serialize(Stream, message);
		private Object GetMessageSerialize()
			=> formatter.Deserialize(Stream);

        private void MenuCloseEvent(object sender, RoutedEventArgs e)
        {
			ButtonMenuClose.Visibility = Visibility.Collapsed;
			ButtonMenuOpen.Visibility = Visibility.Visible;
			ListServers.Visibility = Visibility.Collapsed;
		}

        private void MenuOpenEvent(object sender, RoutedEventArgs e)
        {
			ButtonMenuOpen.Visibility = Visibility.Collapsed;
			ButtonMenuClose.Visibility = Visibility.Visible;
			ListServers.Visibility = Visibility.Visible;
		}

		private void ServerWasSelected(object sender, SelectionChangedEventArgs e)
		{
			ListTextChat.SelectedIndex = 0;
			Server = User.ServerUser.ToList()[ListServers.SelectedIndex].Server;
			ListTextChat.ItemsSource = Server.TextChat;
			ListUserMessage.ItemsSource = Server.TextChat.ToList()[0].Message;
			ListUserOffline.ItemsSource = Server.ServerUser.Where(x => x.User.Status == Status.Offline).Select(x => x.User);
			ListUserOnline.ItemsSource = Server.ServerUser.Where(x => x.User.Status == Status.Online).Select(x => x.User);
		}

        private void SearchServer(object sender, RoutedEventArgs e)
        {
			GetServer();
        }

        private void UpClientClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
