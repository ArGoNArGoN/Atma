/// <summary>
/// Логика подгрузки данных сервера в отдельном потоке
/// подгрузка данных о сервере, пользователях и текстовых чатах и сообщениях
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
	/// Блять, пожалуйста, не изменяй события!
	/// (оно и так плохо работает)
	/// </summary>
    public partial class MainWindow
    {
		/// <summary>
		/// События, отвечающие за обнавление информации на стороне клиента
		/// </summary>
		public event Action<List<Message>> EventUpMessage;
		public event Action<List<ServerUser>> EventUpUserStatus;
		public event Action<List<Server>> EventUpServer;
		public event Action<List<TextChat>> EventUpTextChat;

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
			this.EventUpMessage += new Action<List<Message>>(AddMessageInListbox);
			this.EventUpUserStatus += new Action<List<ServerUser>>(UpUserStatusInListBoxs);
			this.EventUpServer += new Action<List<Server>>(ServerSearch.UpServer);
			this.EventUpTextChat += new Action<List<TextChat>>(UpTextChat);

			Thread thr = new Thread(new ThreadStart(TakeMessageOfServer));
			thr.IsBackground = false;
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
					ob = TakeMessageSerialize();
				}
                catch (Exception)
                {
					break;
                }

				switch (ob)
				{
					case Message message:
						var text = TextChats.Find(x => x.ID == message.IDTextChat);
						text.Message.Add(message);

						EventUpMessage(text.Message.ToList());
						break;

					case ServerUser user:
						if (Users is null)
							break;
						var a = Users.FirstOrDefault(x => x.IDUser == user.IDUser);
						if (a is null)
							break;
						

						EventUpUserStatus(Users);
						break;

					case (List<Server> Servers):
						if (Servers is null || Servers.Count() == 0)
							break;
						
						EventUpServer(Servers);
						break;

					case (Server Server):
						if (Server is null)
							break;

						TextChats = Server.TextChat.ToList();
						EventUpTextChat?.Invoke(Server.TextChat.ToList());
						EventUpUserStatus?.Invoke(Server.ServerUser.ToList());
						break;

					case List<TextChat> textChat:
						TextChats = textChat;
						EventUpTextChat?.Invoke(textChat);
						break;

					case String str:
						if (str == "closestream")
							Environment.Exit(0);
						break;
					default:
						break;
				}
			} while (true);
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
		private void AddMessageInListbox(List<Message> obj)
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

						if (ListTextChat.SelectedIndex != -1 && TextChats[ListTextChat.SelectedIndex].ID == obj[0]?.IDTextChat)
							ListUserMessage.ItemsSource = obj;
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
