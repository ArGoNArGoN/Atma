﻿/// <summary>
/// Логика подгрузки данных сервера в отдельном потоке
/// подгрузка данных сервера (Message, TC, Opinion)
/// </summary>

using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace ClientChatWPF
{
	/// <summary>
	/// Логика подгрузки информации, которая приходит с сервера,
	/// в клиентское приложение
	/// Здесь долбежка с потоками и ListBox'ами
	/// пожалуйста, не изменяй события!
	/// (оно и так плохо работает)
	/// </summary>
	public partial class WindowMain
	{
		/// <summary>
		/// События, отвечающие за обнавление информации на стороне клиента
		/// </summary>
		public event Action<IEnumerable<Message>> EventUpMessage;
		public event Action<List<ServerUser>> EventUpUserStatus;
		public event Action<List<Server>> EventUpServerSearch;
		public event Action<List<Server>> EventUpServer;
		public event Action<List<TextChat>> EventUpTextChat;
		public event Action<List<Opinion>> EventUpOpinion;
		public event Action<List<Server>> EventUpServersFromUser;
		public event Action<Object> EventUpFriendsAndUsers;

		/// <summary>
		/// Содержат информацию о
		/// гребаном сервере
		/// </summary>
		List<TextChat> TextChats { get; set; }
		List<Message> Messages { get; set; }
		List<Server> Servers { get; set; }
		/// <summary>
		/// Пользователи на сервере
		/// Оно используется?
		/// </summary>
		List<ServerUser> Users { get; set; }

		/// <summary>
		/// Запускается при открытии приложения
		/// Если ты решил(а) добавить гребаное событие,
		/// то создай его типа Action<Какой-тоОбъект>. 
		/// Создай метод, который будет работать с ListBox
		/// и прокинь ему его,
		/// естественно не забудь добавить в switch (TakeMessageOfServer)
		/// вызов этого тупова делегата
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoadInfoServer(object sender, RoutedEventArgs e)
		{
			this.EventUpMessage += new Action<IEnumerable<Message>>(AddMessageInListbox);
			this.EventUpUserStatus += new Action<List<ServerUser>>(UpUserStatusInListBoxs);
			this.EventUpServer += new Action<List<Server>>(UpServer);
			this.EventUpTextChat += new Action<List<TextChat>>(UpTextChat);

			Thread thr = new Thread(new ThreadStart(TakeMessageOfServer)) { IsBackground = false };
			thr.Start();
		}

		/// <summary>
		/// Прослушивает поток, на наличие сообщений от сервера
		/// Прежде чем добавить какой-нибудь объект в swich подумай 10 раз!
		/// Т.к. создатель этого метода не шибко умный, тебе придется либо увеличивать switch
		/// Либо писать огромный метод, который будет все это обрабатывать, что тоже не оч. верно
		/// </summary>
		private void TakeMessageOfServer()
		{
			Object ob = null;

			do
			{
				try
				{
					ob = SendMessageToServer.TakeMessageSerialize();
				}
				catch (Exception)
				{
					break;
				}

				switch (ob)
				{
					/// Получаем сообщения и отправляем их на текстовые чаты
					case Message message:
						var text = TextChats.Find(x => x.ID == message.IDTextChat);
						text.Message.Add(message);

						//// выполняются полиморфные вызовы
						EventUpMessage((IEnumerable<Message>)text.Message);
						break;

					/// Получаем пользователя и меняем его статус на сервере
					case ServerUser user:
						if (user.StatusObj == StatusObj.Add)
						{
							User.ServerUser.Add(user);

							var server = user.Server;
							Servers.Add(server);

							EventUpServer?.Invoke(Servers);

							EventUpServersFromUser?.Invoke(Servers);

							break;
						}
						else if (user.StatusObj == StatusObj.Delete)
						{
							User.ServerUser = User.ServerUser.Where(x => user.ID != x.ID).ToList();

							var server = user.Server;
							var asd = Servers.Where(x => server.ID != x.ID).ToList();
							Servers.Clear();
							foreach (var item in asd)
							{
								Servers.Add(item);
							}

							EventUpServer?.Invoke(Servers);

							EventUpServersFromUser?.Invoke(Servers);

							break;
						}

						if (Users is null)
							break;

						var a = Users.FirstOrDefault(x => x.IDUser == user.IDUser);
						if (a is null)
							break;

						EventUpUserStatus(Users);
						break;

					/// Получаем пользователей на сервере запоминаем и отправляем в окно редактирования, если оно открыто
					case (List<ServerUser> SU):
						if (SU is null)
							break;

						if (WEditingServer is not null)
						{
							WEditingServer.StartEventOfObject(SU);
							return;
						}
						if (WFAU is not null)
						{
							EventUpFriendsAndUsers?.Invoke(SU);
							return;
						}

						Users = SU;
						EventUpUserStatus?.Invoke(SU);

						break;

					/// получаем список серверов у пользователя или в окне поиска (TODO)
					case (List<Server> Servers):
						if (Servers is null || Servers.Count() == 0)
							break;
						
						EventUpServerSearch(Servers);
						break;

					/// получаем сервер, если он обновился и отправляем в окно редактирования, если оно открыто
					case (Server Server):
						if (Server is null)
							break;

						if (WEditingServer is not null)
							WEditingServer.StartEventOfObject(Server);

						break;

					/// получаем текстовые чаты и отправляем их в окно редактирования, если оно открыто
					case List<TextChat> textChat:
						if (textChat is null)
							break;

						TextChats = textChat;
						this.EventUpTextChat?.Invoke(textChat);

						if (WEditingServer is not null)
							WEditingServer.StartEventOfObject(textChat);

						break;

					/// получаем Отзывы о сервере и отправляем их в окно редактирования, если оно открыто
					case List<Opinion> Opinions:
						if (Opinions is null)
							break;

						if (WEditingServer is not null)
							WEditingServer.StartEventOfObject(Opinions);

						EventUpOpinion(Opinions);

						break;

					/// получаем Роли на сервере и отправляем их в окно редактирования, если оно открыто
					case List<Role> Role:
						if (Role is null)
							break;

						if (WEditingServer is not null)
							WEditingServer.StartEventOfObject(Role);

						break;

					/// получаем Журнал событий сервера и отправляем его в окно редактирования, если оно открыто
					case List<EventLog> EL:
						if (EL is null)
							break;

						if (WEditingServer is not null)
							WEditingServer.StartEventOfObject(EL);

						break;

					/// получаем Журнал событий сервера и отправляем его в окно редактирования, если оно открыто
					case List<Request> RL:
						if (RL is null)
							break;

						EventUpFriendsAndUsers.Invoke(RL);
						break;

					case List<User> UL:
						if (UL is null)
							break;

						EventUpFriendsAndUsers.Invoke(UL);
						break;
						
					case List<UserLog> UL:
						if (UL is null)
							break;

						EventUpFriendsAndUsers.Invoke(UL);
						break;

					case User user:
						EventUpFriendsAndUsers.Invoke(user);
						break;

					case String str:
						if (str == "closestream")
							Environment.Exit(0);
						break;

					default:
						MessageBox
							.Show("Сервер отправил какой-то странный объект.." + ob.GetType().Name);
						break;
				}
			} while (true);
		}
		


		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		private void UpServer(List<Server> obj)
		{
			ListServers.Dispatcher
				.Invoke(new Action(
					() =>
					{
						ListServers.ItemsSource = (Server[])Servers.ToArray().Clone();
					}
					));
		}

		private void UpTextChat(List<TextChat> obj)
		{
			ListTextChat.Dispatcher
				.Invoke(new Action(
					() =>
					{
						if (obj is null || obj.Count() == 0)
							return;

						ListTextChat.ItemsSource = (TextChat[])obj.ToArray().Clone();
					}
					));
		}
		
		/// <summary>
		/// Добавляет сообщения в список
		/// </summary>
		/// <param name="obj"></param>
		private void AddMessageInListbox(IEnumerable<Message> obj)
		{
			ListUserMessage.Dispatcher
				.Invoke(new Action(
					() =>
					{
						if (obj.Count() == 0)
						{
							ListUserMessage.ItemsSource = obj;
							return;
						}

						if (ListTextChat.SelectedIndex != -1 && TextChats[ListTextChat.SelectedIndex].ID == obj.ToArray()[0]?.IDTextChat)
							ListUserMessage.ItemsSource = obj;

						Scroll.ScrollToEnd();
					}
					));
		}

		/// <summary>
		/// Обновляет списки пользователей
		/// </summary>
		/// <param name="obj"></param>
		private void UpUserStatusInListBoxs(List<ServerUser> users)
		{
			ListUserOnline.Dispatcher
				.Invoke
				(
					new Action
					(
						() =>
						{
							var a = new List<ServerUser>();
							foreach (var item in users.Where(x => x.User.Status != Status.Offline))
							{
								a.Add(item);
							}
							ListUserOnline.ItemsSource = a;
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
							ListUserOffline.ItemsSource = (ServerUser[])users.Where(x => x.User.Status == Status.Offline).ToArray().Clone();
						}
					)
				);
		}
	}
}
