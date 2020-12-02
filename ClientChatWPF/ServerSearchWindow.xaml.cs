using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ClientChatWPF
{
	/// <summary>
	/// Логика взаимодействия для ServerSearchWindow.xaml
	/// </summary>
	public partial class ServerSearchWindow : Window
	{
		public User User { get; set; } = null;

		public List<Server> SerchServersList { get; set; } = new List<Server>();

		public ServerSearchWindow(User user)
		{
			InitializeComponent();
			User = user;
			ServersList.ItemsSource = User.ServerUser.Select(x => x?.Server);
			SerchServers.ItemsSource = SerchServersList;
		}

		public void UpServer(List<Server> obj)
		{
			SerchServers.Dispatcher
				.Invoke
				(
					new Action
					(
						() =>
						{
							SerchServersList = obj;
							SerchServers.ItemsSource = SerchServersList;
						}
					)
				);
		}

		private void Search(object sender, RoutedEventArgs e)
		{
			MainWindow.SendMessageSerialize(new Server() { Name = SeachTextBox.Text, ActionForServer = ActionForServer.Search });
		}
	}
}
