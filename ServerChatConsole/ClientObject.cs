using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ServerChatConsole
{
	internal class ClientObject
	{
		public ClientObject(TcpClient client, ServerObj serverObj)
		{
			Client = client;
			ServerObj = serverObj;
		}
		public TcpClient Client { get; set; }
		public ServerObj ServerObj { get; set; }
		public NetworkStream Stream { get; internal set; }
		internal User User { get; set; }
		private BinaryFormatter formatter = new BinaryFormatter();

		/// <summary>
		/// Пользователь должен отправить Свое имя и id
		/// </summary>
		internal void Process()
		{
			try
			{
				Stream = Client.GetStream();

				/// Отправим пользователю его данные, а так же все сервера, на которых он присутствует
				GetInfoOfUser(out String RName, out Int32 id);
				SendInfoForUser(RName, id);
				ServerObj.AddConnection(this);
				ServerObj.UserLogOrUnLog(User);
				TakeObjectFromUser();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				if (User != null)
				{
					ServerObj.RemoveConnection(User.ID);
				}
				Close();
			}
		}

        private void GetInfoOfUser(out String Name, out Int32 id)
        {
			User user = null;
			try 
			{
				user = (User)formatter.Deserialize(Stream);
			}
			catch(InvalidCastException e)
			{
				Console.WriteLine("Не удалось преобразовать тип, который отправил пользователь.. Подробнее: \r" + e.Message);
			}
			if(user == null)
				throw new Exception("Из-за критической ошибки сервер разрывает связь с пользователем "); 
			Name = user.Name;
			id = user.ID;
		}

        private void TakeObjectFromUser()
        {
            do
            {
				var Obj = formatter.Deserialize(Stream);

                using (DB DB = new DB())
                {
                    switch (Obj)
					{
						case (Message):
							Message message = (Message)Obj;
							message.User = DB.User.Find(message.IDUser);
							message.TextChat = DB.TextChat.Find(message.IDTextChat);
							message.TextChat.Server = DB.Server.Find(message.TextChat.IDServer);
							DB.Entry(message).State = EntityState.Added;
							Console.WriteLine($"{User.Name}: {message.Text}");
							ServerObj.BroadcastMessage(message);
							DB.SaveChanges();
							break;
						default:
							Console.WriteLine("Пользователь отправил объект, который не приводится к типам," +
								" которые описаны в switch");
							throw new Exception("Неопределенный тип");
					}
				}
            } while (true);
        }

        private void SendInfoForUser(string rName, int id)
        {
			GetUserFromDB(rName, id);
			SendUserInStrem();
        }

        private void SendUserInStrem()
        {
			formatter.Serialize(Stream, User);
        }

        private void GetUserFromDB(string rName, int id)
        {
            using (DB DB = new DB())
            {
				User = DB.User.Find(id);
				if (User == null)
					throw new Exception("Пользовотель не найден!");
				if (User.RealName != rName)
					throw new Exception("Отправленное имя пользователя не совпадает с именем в DB");

				try
				{
					var serversUsers = User.ServerUser = DB.ServerUser
						.Include(x => x.User)
						.Include(x => x.Server)
						.Where(x => x.IDUser == User.ID)
						.ToList();

					var servers = DB.Server
						.Include(x => x.ServerUser)
						.Include(x => x.TextChat)
						.Include(x => x.Chat)
						.ToList();

					var srevers = new List<Server>();
					foreach (var SerUsr in serversUsers)
					{
						SerUsr.Server = servers.Find(x => x.ID == SerUsr.IDServer);
						SerUsr.User = DB.User.Find(SerUsr.IDUser);
						var TextChats = SerUsr.Server.TextChat;
						srevers.Add(SerUsr.Server);

						foreach (var textChat in TextChats)
						{
							textChat.Server = SerUsr.Server;

							textChat.Message = DB.Message
								.Include(x => x.User)
								.Include(x => x.TextChat)
								.Where(x => x.IDTextChat == textChat.ID)
								.ToList();
						}
					}

                    foreach (var ser in servers)
                        foreach (var item in ser.ServerUser)
							item.User = DB.User.Find(item.IDUser);
				}
				catch(Exception e)
				{ 
					Console.WriteLine(e.Message + " " + e.InnerException?.Message);
					throw new Exception("Не удалось получить данные из DB, опять Debug!!!");
				}

				DB.Entry(User).State = EntityState.Modified;

				User.Status = Status.Online;

				DB.SaveChanges();
			}
        }

		internal void Close()
		{
            using (DB DB = new DB())
            {
				User.Status = Status.Offline;
				DB.Entry(User).State = EntityState.Modified;
				DB.SaveChanges();
            }

			ServerObj.UserLogOrUnLog(User);
			ServerObj.RemoveConnection(User.ID);

            if (Stream != null)
				Stream.Close();
			if (Client != null)
				Client.Close();
		}
	}
}