using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Логика взаимодействия для WindowFriendsAndUsers.xaml
	/// </summary>
	public partial class WindowFriendsAndUsers : Window
	{
		public WindowFriendsAndUsers()
		{
			InitializeComponent();


			EventUpServerUser       += new Action<User>(UpServerUser);
			EventUpFrieds           += new Action<List<User>>(UpFrieds);
			EventUpRequestsUsers    += new Action<List<Request>>(UpRequestsUsers);
			EventUpServerUsers		+= new Action<List<User>>(UpRequestFrieds);
			EventUpUserLog          += new Action<List<UserLog>>(UpUserLog);
		}

		public WindowFriendsAndUsers(User user, ServerUser serverUser)
			: this()
		{
			User = user ?? throw new ArgumentNullException(nameof(user));
            ServerUser = serverUser;
			a = TabControlSet.SelectedIndex;
		}

		/// <summary>
		/// Инициализ. при Открытии формы
		/// </summary>
		public User User { get; set; }
		public ServerUser ServerUser { get; set; }

		public List<User> Friends { get; set; } = new List<User>();
		public List<Request> RequestsUsers { get; set; } = new List<Request>();
		public List<User> RequestFrieds { get; set; } = new List<User>();

		public event Action<User> EventUpServerUser;
		public event Action<List<User>> EventUpFrieds;
		public event Action<List<Request>> EventUpRequestsUsers;
		public event Action<List<User>> EventUpServerUsers;
		public event Action<List<UserLog>> EventUpUserLog;

		Int32 a;

		private void TabControlSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (a == TabControlSet.SelectedIndex)
				return;

			a = TabControlSet.SelectedIndex;

			var ob = new User() { ID = User.ID };


			switch (TabControlSet.SelectedIndex)
			{
				case (0): break;

				case (1):
					ob.ActionFromUser = ActionFromUser.Loud;
					ob.ServerUser = new List<ServerUser>() { ServerUser };
					break;

				case (2):
					ob.ActionFromUser = ActionFromUser.LoudFriends;
					break;

				case (3):
					ob.ActionFromUser = ActionFromUser.LoudReq;
					break;

				case (5):
					ob.ActionFromUser = ActionFromUser.LoudUserLog;
					break;
				default:
					break;
			}

			if (ob is null)
				return;

			SendMessageToServer.SendMessageSerialize(ob);
		}



		public void StartEventOfObject(Object ob)
		{
			switch (ob)
			{
				case(User SU):
					EventUpServerUser?.Invoke(SU);
					break;

				case (List<Request> Request):
					if (Request is null || Request.Count() == 0)
						break;

					EventUpRequestsUsers?.Invoke(Request);
					break;

				case (List<User> SUL):
					var a = -1;
					TabControlSet.Dispatcher?.Invoke(new Action(() => a = TabControlSet.SelectedIndex));

					if (a == 2)
						EventUpFrieds?.Invoke(SUL);

					if (a == 4)
						EventUpServerUsers?.Invoke(SUL);

					break;

				case (List<UserLog> UserLog):
					EventUpUserLog?.Invoke(UserLog);
					break;

				default:
					break;
			}
		}



		public void UpServerUser(User User)
		{
			if (User is null)
			{
				textError.Text = "Сервер кинул что-то странное!";
				return;
			}
			if (User.StatusObj == StatusObj.Edit)
			{
				textError.Dispatcher?.Invoke(new Action(() => textError.Text = "Такое имя уже существует!"));
				return;
			}

			if (User.ServerUser is not null && User.ServerUser.Count() != 0)
			{
				nameUser.Dispatcher?.Invoke(new Action(() => nameUser.Text = User.ServerUser.ToList()[0].Name));
				statusUser.Dispatcher?.Invoke(new Action(() => statusUser.Text = User.ServerUser.ToList()[0].Status2));
			}
			else
			{
				nameUser.Dispatcher?.Invoke(new Action(() => nameUser.IsEnabled = false));
				statusUser.Dispatcher?.Invoke(new Action(() => statusUser.IsEnabled = false));
			}

			realNameUser.Dispatcher?.Invoke(new Action(() => realNameUser.Text = User.RealName));
			passwordUser.Dispatcher?.Invoke(new Action(() => passwordUser.Text = User.Password));
			NameUser.Dispatcher?.Invoke(new Action(() => NameUser.Text = User.Name));
			dateOfBirhtUser.Dispatcher?.Invoke(new Action(() => dateOfBirhtUser.Text = User.DateOfBirht != null ? User.DateOfBirht.Value.ToShortDateString() : ""));

			if (ServerUser is not null && User.ServerUser is not null && User.ServerUser.Count() > 0)
			{
				ServerUser.Name = User.ServerUser.ToList()[0].Name;
				ServerUser.Status2 = User.ServerUser.ToList()[0].Status2;
			}
			this.User.RealName = User.RealName;
			this.User.Password = User.Password;
			this.User.Name = User.Name;
			this.User.DateOfBirht = User.DateOfBirht;
		}
		public void UpFrieds(List<User> requests)
		{
			if (requests is null || requests.Count() == 0)
				return;

			Friends = requests;
			ListFriend.Dispatcher?.Invoke(new Action(() => ListFriend.ItemsSource = requests));
		}
		public void UpRequestsUsers(List<Request> requests)
		{
			if (requests is null || requests.Count() == 0)
				return;

			RequestsUsers = requests;
			requestsList.Dispatcher?.Invoke(new Action(() => requestsList.ItemsSource = requests.Select(x => 
			{
				if (x.IDUser == User.ID)
					return x.User1;
				else 
					return x.User;
			}).ToList()));
		}
		public void UpRequestFrieds(List<User> requests)
		{
			if (requests is null || requests.Count() == 0)
				return;

			RequestFrieds = requests;
			RequestFriends.Dispatcher?.Invoke(new Action(() => RequestFriends.ItemsSource = requests));
		}
		public void UpUserLog(List<UserLog> userLogs)
		{
			if (userLogs is null || userLogs.Count() == 0)
				return;

			UserLogList.Dispatcher?.Invoke(new Action(() => UserLogList.ItemsSource = userLogs));
		}



		private void DeleteFromFriendsClick(object sender, RoutedEventArgs e)
		{
			var up = Friends[ListFriend.SelectedIndex];
			var req = new Request() { IDUser = User.ID, IDFriend = up.ID, FriendRequest = true, StatusObj = StatusObj.Edit };

			Friends.Remove(up);
			ListFriend.ItemsSource = Friends.Select(x => x);

			SendMessageToServer.SendMessageSerialize(req);
		}

        private void ToAcceptRequestClick(object sender, RoutedEventArgs e)
        {
			var up = RequestsUsers[requestsList.SelectedIndex];

			var req = new Request() { IDUser = User.ID, IDFriend = up.IDFriend, FriendRequest = true, UserRequest = true, StatusObj = StatusObj.Edit };

			RequestsUsers.Remove(up);
			requestsList.ItemsSource = RequestsUsers.Select(x =>
			{
				if (x.IDUser == User.ID)
					return x.User1;
				else
					return x.User;
			});

			SendMessageToServer.SendMessageSerialize(req);
		}

        private void RejectRequestClick(object sender, RoutedEventArgs e)
        {
			var up = RequestsUsers[requestsList.SelectedIndex];

            var req = new Request() { ID = up.ID, StatusObj = StatusObj.Delete };

			RequestsUsers.Remove(up);
			requestsList.ItemsSource = RequestsUsers.Select(x =>
			{
				if (x.IDUser == User.ID)
					return x.User1;
				else
					return x.User;
			});

			SendMessageToServer.SendMessageSerialize(req);
		}

        private void SearchClick(object sender, RoutedEventArgs e)
        {
			var u = new User() { Name = SeachTextBox.Text };
			u.ActionFromUser = ActionFromUser.Search;
			
			SendMessageToServer.SendMessageSerialize(u);
        }

        private void SendRequestClick(object sender, RoutedEventArgs e)
        {
			var u = RequestFrieds[RequestFriends.SelectedIndex];
			var req = new Request() { IDUser = User.ID, IDFriend = u.ID, UserRequest = true, StatusObj = StatusObj.Add };

			RequestFrieds.Remove(u);
			RequestFriends.ItemsSource = null;
			RequestFriends.ItemsSource = RequestFrieds;

			SendMessageToServer.SendMessageSerialize(req);
        }

        private void SaveChangesClick(object sender, RoutedEventArgs e)
        {
			var s = dateOfBirhtUser.Text.Trim();
			if (s != String.Empty && !DateTime.TryParse(dateOfBirhtUser.Text, out _))
			{
				textError.Text = "Некорректная дата!";
				return;
            }
			s = DateTime.TryParse(dateOfBirhtUser.Text, out _) ? s : "";
			try
            {
				var u = new User()
				{
					ID = User.ID,
					Name = NameUser.Text,
					RealName = realNameUser.Text,
					Password = passwordUser.Text,
				};

				if (s != String.Empty && !DateTime.TryParse(dateOfBirhtUser.Text, out _))
					u.DateOfBirht = DateTime.Parse(dateOfBirhtUser.Text);

				textError.Text = "";

				u.StatusObj = StatusObj.Edit;

				if ((User.ServerUser is not null && ServerUser is not null) && User.ServerUser.Count() > 0 )
				{
					ServerUser.Name = nameUser.Text != String.Empty ? nameUser.Text : User.Name;
					ServerUser.Status2 = statusUser.Text;
					ServerUser.StatusObj = StatusObj.Edit;
					u.ServerUser = new List<ServerUser>() { ServerUser };
				}

				SendMessageToServer.SendMessageSerialize(u);
			}
            catch (Exception ex)
            {
				textError.Text = ex.Message;
            }
		}

        private void RelClick(object sender, RoutedEventArgs e)
        {
			if (ServerUser is not null && User.ServerUser is not null && User.ServerUser.Count() != 0)
			{
				nameUser.Text = User.ServerUser.ToList()[0].Name;
				statusUser.Text = User.ServerUser.ToList()[0].Status2;
			}
			else
			{
				nameUser.IsEnabled = false;
				statusUser.IsEnabled = false;
			}

			realNameUser.Text = User.RealName;
			passwordUser.Text = User.Password;
			NameUser.Text = User.Name;
			dateOfBirhtUser.Text = User.DateOfBirht != null ? User.DateOfBirht.Value.ToShortDateString() : "";
		}
    }
}
