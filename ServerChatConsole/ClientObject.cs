using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ServerChatConsole
{
    internal class ClientObject
	{
		internal ClientObject(TcpClient client, ServerObj serverObj)
		{
			Client = client;
			ServerObj = serverObj;
		}
		internal TcpClient Client { get; set; }
		internal ServerObj ServerObj { get; set; }
		public NetworkStream Stream { get; internal set; }
		internal User User { get; set; }
		private BinaryFormatter formatter = new BinaryFormatter();
		internal List<ServerUsers> ServerUsers { get; set; } = new List<ServerUsers>();

		/// <summary>
		/// Пользователь должен отправить Свое имя и id
		/// </summary>
		internal void Process()
		{
			try
			{
                Stream = Client.GetStream();

                do
				{
					try
					{
						/// Отправим пользователю его данные, а так же все сервера, на которых он присутствует
						GetInfoOfUser(out User user);
						if (user.ActionForServer is not null && user.ActionForServer == ActionForServer.Registration)
						{
							RegistrationUser(user);
							continue;
						}

						SendInfoForUser(user.RealName, user.Password);

						break;
					}
					catch(SocketException e)
					{ Console.WriteLine(e.Message); }
					catch(Exception e)
					{ this.SendObjectToClient(e); }

				} while (true);

				ServerObj.AddConnection(this);
				SendUser(User);
				TakeObjectFromUser();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				Close();
			}
		}

        private void RegistrationUser(User user)
        {
			try
			{
				using DB db = new DB();

				if (db.User.FirstOrDefault(x => x.RealName.Equals(user.RealName)) is not null)
					throw new Exception("Use a different name!");

				db.User.Add(user);

				this.SendObjectToClient(user);

				db.SaveChanges();
			}
			catch (Exception e)
			{
				this.SendObjectToClient(e);
			}
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
							this.SendMessade((Message)Obj);
							break;
						case (ClassesForServerClent.Class.User):
							this.SendUser((User)Obj);
							break;
						case (Server):
							this.SendServer((Server)Obj);
							break;
						default:
							Console.WriteLine("Пользователь отправил объект, который не приводится к типам," +
								" которые описаны в switch");
							throw new Exception("Неопределенный тип");
					}
				}
            } while (true);
        }

		private void SendMessade(Message message)
        {
			if(message is null)
				throw new ArgumentNullException("message is null", nameof(message));

			Console.WriteLine($"{User.Name}: {message.Text}");

			using (DB DB = new DB())
            {
				message.User = DB.User.Find(message.IDUser);
				message.TextChat = DB.TextChat.Find(message.IDTextChat);
				message.TextChat.Server = DB.Server.Find(message.TextChat.IDServer);
				DB.Entry(message).State = EntityState.Added;

				DB.SaveChanges();
			}
			var sU = ServerUsers
				.Find(x => x.Server.ID == message.TextChat.Server.ID);

			new Thread(sU.SendMessageToServer)
				.Start(message);
		}
		private void SendUser(User user)
		{
			if (user is null)
				throw new ArgumentNullException("user is null", nameof(user));

			using(DB DB = new DB())
            {
				DB.Entry(user).State = EntityState.Modified;
            }

			Console.WriteLine($"{User.Name}: Отправил самого себя всем пользователям");

			foreach (var x in ServerUsers)
				new Thread(x.SendUserToServer).Start(user);
		}
		private void SendServer(Server server)
        {
			if (server is null)
				throw new ArgumentNullException("server is null", nameof(server));

			Console.WriteLine($"{User.Name}: изменил сервер {server.Name}");

			ServerUsers
				.Find(x => x.Server.ID == server.ID)
				.SendServerToServer(server);
		}
		private void SendInfoForUser(String rName, String password)
		{
			GetUserFromDB(rName, password);
			SendUserOfClient();
		}
		private void SendUserOfClient()
		{
			formatter.Serialize(Stream, User);
		}

		private void GetInfoOfUser(out User user)
		{
			User user1 = null;
			try
			{
				user1 = (User)formatter.Deserialize(Stream);
			}
			catch (InvalidCastException e)
			{
				Console.WriteLine("Не удалось преобразовать тип, который отправил пользователь.. Подробнее: \r" + e.Message);
			}
            user = user1 
				?? throw new Exception("Из-за критической ошибки сервер разрывает связь с пользователем ");
		}
        private void GetUserFromDB(String rName, String password)
        {
            using (DB DB = new DB())
            {
				User = DB.User.FirstOrDefault(x => x.RealName == rName);
				
				if (User == null)
					throw new Exception("Пользовотель не найден!");
				
				if (User.Password != password)
					throw new Exception("Отправленый пароль не совпадает с паролем из DB");

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

		internal void SendObjectToClient(Object message)
		{
			lock (formatter)
			{
				formatter.Serialize(Stream, message);
			}
		}
		internal void Close()
		{
			if (User is not null)
			{
				using (DB DB = new DB())
				{
					User.Status = Status.Offline;
					DB.Entry(User).State = EntityState.Modified;
					DB.SaveChanges();
				}

				SendUser(User);
			}
			ServerObj.RemoveConnection(this);

            if (Stream != null)
				Stream.Close();
			if (Client != null)
				Client.Close();
		}
	}
}