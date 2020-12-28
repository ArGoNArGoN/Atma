using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClientChatWPF
{
	/// <summary>
	/// Логика взаимодействия для ServerSearchWindow.xaml
	/// </summary>
	public partial class ServerSearchWindow : Window
	{
		public User User { get; set; } = null;
		public List<Server> UserServerList { get; set; } = new List<Server>();
		public List<Server> SerchServersList { get; set; } = new List<Server>();


		public ServerSearchWindow(User user)
		{
			InitializeComponent();

            User = user;
			UserServerList = User.ServerUser.Select(x => x?.Server).ToList();
            ServersList.ItemsSource = UserServerList;
			SerchServers.ItemsSource = null;
		}


		private void Search(object sender, RoutedEventArgs e)
		{
			SendMessageToServer.SendMessageSerialize(new Server() { Name = SeachTextBox.Text, ActionForServer = ActionForServer.Search });
		}

        private void SerchServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			if (SerchServers.SelectedIndex == -1 && ServersList.SelectedIndex == -1)
			{
				OpinionList.ItemsSource = new List<Opinion>();
				ServerName.Text = "";
				ServerInfo.Text = "";
				return;
			}

			if (SerchServers.SelectedIndex == -1)
				return;

			SerchServers.ItemsSource = SerchServersList;
			ServersList.SelectedIndex = -1;

			SendMessageToServer.SendMessageSerialize(new Server() { ID = SerchServersList[SerchServers.SelectedIndex].ID, ActionForServer = ActionForServer.LoudOpinion });
			LoudInfo(SerchServersList[SerchServers.SelectedIndex]);
		}

        private void ServersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (SerchServers.SelectedIndex == -1 && ServersList.SelectedIndex == -1)
			{
				OpinionList.ItemsSource = new List<Opinion>();
				ServerName.Text = "";
				ServerInfo.Text = "";
				return;
			}

			if (ServersList.SelectedIndex == -1)
				return;

			SerchServers.ItemsSource = SerchServersList;
			SerchServers.SelectedIndex = -1;

			SendMessageToServer.SendMessageSerialize(new Server() { ID = UserServerList[ServersList.SelectedIndex].ID, ActionForServer = ActionForServer.LoudOpinion });
			LoudInfo(UserServerList[ServersList.SelectedIndex]);
		}

        private void LoudInfo(Server server)
        {
			ServerName.Text = server.Name;

			if (server.Info is null)
				ServerInfo.Text = "Вождя не оставил информацию о сервере!";

			else
				ServerInfo.Text = server.Info;
		}


		public void UpServerFromUser(List<Server> obj)
		{
			ServersList.Dispatcher
				.Invoke(new Action(() =>
				{
					UserServerList = ((Server[])obj.ToArray().Clone()).ToList();
					ServersList.ItemsSource = UserServerList;
				}));
		}
		public void UpOpinion(List<Opinion> obj)
        {
			if (obj.Count == 0)
			{
				AvgOpinion.Dispatcher
					.Invoke(new Action(() =>{AvgOpinion.Text = "Оценок нет!";}));
			}
			else
			{
				AvgOpinion.Dispatcher
					.Invoke
					(
						new Action
						(
							() =>
							{
								AvgOpinion.Text = String.Format("{0:##.#}", obj.Average(x => x.Mark));
							}
						)
					);
			}

			OpinionList.Dispatcher
				.Invoke(new Action(() =>
						{
							OpinionList.ItemsSource = obj;
						}));
		}
		public void UpServerSearch(List<Server> obj)
		{
			SerchServers.Dispatcher
				.Invoke(new Action(() =>
				{
					SerchServersList = ((Server[])obj.ToArray().Clone()).ToList();
					SerchServers.ItemsSource = SerchServersList;
				}));
		}


        private void ConnectionFromServer(object sender, RoutedEventArgs e)
        {
			if (SerchServers.SelectedIndex == -1)
				return;
			
			var server = SerchServersList[SerchServers.SelectedIndex];

			var SU = new ServerUser() { IDServer = server.ID, IDUser = User.ID, StatusObj = StatusObj.Add, Name = User.Name };

			SendMessageToServer.SendMessageSerialize(SU);
		}


        private void DisСonnectionFromServer(object sender, RoutedEventArgs e)
        {
			if (SerchServers.SelectedIndex == -1)
				return;

			var server = SerchServersList[SerchServers.SelectedIndex];

			if (UserServerList.FirstOrDefault(x => x.ID == server.ID) is null)
				return;

            var SU = new ServerUser() { ID = User.ServerUser.FirstOrDefault(x => x.IDServer == server.ID).ID, StatusObj = StatusObj.Delete };

			SendMessageToServer.SendMessageSerialize(SU);
		}
    }
}
