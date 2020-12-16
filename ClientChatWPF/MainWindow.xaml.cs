using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClientChatWPF
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class WindowMain : Window
	{
		Server Server { get; set; }
		static User User { get; set; }
		static ServerUser ServerUser { get; set; }
		public WindowEditingServer WEditingServer { get; set; }
		public ServerSearchWindow ServerSearch { get; set; }
		public WindowFriendsAndUsers WFAU { get; set; }


		/// <summary>
		/// Начало всего
		/// </summary>
		public WindowMain()
		{
			InitializeComponent();

			/// авторизация пользователя
			if (!LogUp())
				return;

			/// Инициализируем все события
			/// Файлик LoadInfoForClienInThread
			this.Loaded += new RoutedEventHandler(LoadInfoServer);

			LoadedDataInWindow();
		}

		/// <summary>
		/// Запускает Авторизацию
		/// </summary>
		/// <returns>Возвращает True, если пользователь Авторизироваля, иначе False</returns>
		private Boolean LogUp()
		{
			Reg reg = new Reg();
			reg.ShowDialog();

			User = reg.User;

			if (reg.User == null)
			{ Close(); return false; }
			return true;
		}

		/// <summary>
		/// Подгружаем в окно данные о пользователях
		/// </summary>
		private void LoadedDataInWindow()
		{
			Servers = User.ServerUser.Select(x => x.Server).ToList();
			ListServers.ItemsSource = User.ServerUser.Select(x => x.Server);
			nickNameUser.Text = User.Name;
			statusUser.Text = User.Status.ToString();
		}

		/// <summary>
		/// Посылает запрос подзагрузки текстового чата, тобишь сообщений
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextChatWasSelected(object sender, SelectionChangedEventArgs e)
		{
			if (ListTextChat.SelectedIndex == -1)
				return;

			sendMessageButton.IsEnabled = true;
			var text = TextChats[ListTextChat.SelectedIndex];
			EventUpMessage(text.Message.ToList());
		}

		/// <summary>
		/// Посылает запрос подзагрузки сервера, тобишь текстовых чатов
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ServerWasSelected(object sender, SelectionChangedEventArgs e)
		{
			if (ListServers.SelectedIndex == -1)
				return;

			ListUserMessage.ItemsSource = new List<Message>();
			sendMessageButton.IsEnabled = false;

			Server = Servers[ListServers.SelectedIndex];

			serverName.Text = Server.Name;
			ServerUser = User.ServerUser.First(x => x.IDServer == Server.ID);
			nickNameUser.Text = ServerUser.Name;
			Server.ActionOnServer = ActionOnServer.Connect;
			SendMessageToServer.SendMessageSerialize(Server);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendMessage(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(MessagePop.Text))
				return;

			SendMessageToServer.SendMessageSerialize(new Message()
			{
				IDServerUser = ServerUser.ID,
				IDTextChat = TextChats[ListTextChat.SelectedIndex].ID,
				Text = MessagePop.Text,
				Date = DateTime.Now,
			});

			MessagePop.Text = "";
		}

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

		private void SearchServer(object sender, RoutedEventArgs e)
		{
			SearchServer();
		}

		/// <summary>
		/// Запускает окно поиска сервера 
		/// И отправляет информацию об этом событии серверу. 
		private void SearchServer()
		{
			SendMessageToServer.SendMessageSerialize(new Server() { ActionForServer = ActionForServer.Search });

			ServerSearch = new ServerSearchWindow(User);

			this.EventUpServerSearch += new Action<List<Server>>(ServerSearch.UpServerSearch);
			this.EventUpOpinion += new Action<List<Opinion>>(ServerSearch.UpOpinion);
			this.EventUpServersFromUser += new Action<List<Server>>(ServerSearch.UpServerFromUser);
			ServerSearch.ShowDialog();
			this.EventUpServerSearch -= new Action<List<Server>>(ServerSearch.UpServerSearch);
			this.EventUpOpinion -= new Action<List<Opinion>>(ServerSearch.UpOpinion);
			this.EventUpServersFromUser -= new Action<List<Server>>(ServerSearch.UpServerFromUser);

			ServerSearch = null;
		}

		/// <summary>
		///	Запускаем окно
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpClientClick(object sender, RoutedEventArgs e)
		{
			/// Отправляем пользователя
			WFAU = new WindowFriendsAndUsers(User, ServerUser);
			this.EventUpFriendsAndUsers += new Action<object>(WFAU.StartEventOfObject);
			WFAU.ShowDialog();
			this.EventUpFriendsAndUsers -= new Action<object>(WFAU.StartEventOfObject);
			WFAU = null;
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			SendMessageToServer.SendMessageSerialize("closestream");
		}

		private void UpServerClick(object sender, RoutedEventArgs e)
		{
			if (Server is null)
				return;

			WEditingServer = new WindowEditingServer(Server);
			WEditingServer.ShowDialog();
			WEditingServer = null;
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

		}
    }
}
