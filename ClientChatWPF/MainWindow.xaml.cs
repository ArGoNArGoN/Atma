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
		ServerSearchWindow ServerSearch { get; set; }
		Server Server { get; set; }
		static User User { get; set; }
		static ServerUser ServerUser { get; set; }
		static NetworkStream Stream { get; set; }
        static BinaryFormatter formatter = new BinaryFormatter();
		public WindowEditingServer WEditingServer { get; set; }

		/// <summary>
		/// Начало всего
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			/// авторизация пользователя
			if (!LogUp())
				return;

			ServerSearch = new ServerSearchWindow(User);

			/// Если пользователь только что зарегался, или у него нет сервера,
			/// то он должен на него зайти, иначе он гей
			if (User.ServerUser.Count() == 0)
				SearchServer();

			LoadedDataInWindow();

			/// Инициализируем все события
			/// Файлик LoadInfoForClienInThread
			this.Loaded += new RoutedEventHandler(LoadInfoServer);
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
			Stream = reg.Stream;

			if (reg.User == null)
			{ Close(); return false; }
			return true;
		}

		/// <summary>
		/// Запускает окно поиска сервера 
		/// И отправляет информацию об этом событии серверу. 
		private void SearchServer()
        {
			SendMessageSerialize(new Server() { ActionForServer = ActionForServer.Search });
            ServerSearch.ShowDialog();

			if (User.ServerUser.Count() == 0)
			{
				Close();
				Environment.Exit(0);
			}
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
			SendMessageSerialize(Server);
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

			SendMessageSerialize(new Message()
			{
				IDServerUser = ServerUser.ID,
				IDTextChat = TextChats[ListTextChat.SelectedIndex].ID,
				Text = MessagePop.Text,
				Date = DateTime.Now,
			});

			MessagePop.Text = "";
		}
		
		public static void SendMessageSerialize(Object message)
		{
			try
			{
				formatter.Serialize(Stream, message);
			}
			catch (Exception) { }
		}
		private Object TakeMessageSerialize()
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

        private void SearchServer(object sender, RoutedEventArgs e)
        {
			SearchServer();
        }

		/// <summary>
		///	Запускаем окно
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void UpClientClick(object sender, RoutedEventArgs e)
        {
			/// Отправляем пользователя
			UpClient upClient = new UpClient(User);
			upClient.ShowDialog();

			/// Если пользователь изменился, то отправляем его на сервер
			if(User.ActionForServer == ActionForServer.LoudTextChat)
				SendMessageSerialize(User);
		}

        private void Window_Closed(object sender, EventArgs e)
        {
			if(Stream != null)
				SendMessageSerialize("closestream");
        }

        private void UpServerClick(object sender, RoutedEventArgs e)
        {
			if (Server is null)
				return;

			WEditingServer = new WindowEditingServer(Server);
			WEditingServer.ShowDialog();
			WEditingServer = null;
		}
    }
}
