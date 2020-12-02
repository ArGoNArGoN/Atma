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
		internal ServerUser ServerUser { get; set; }
		private BinaryFormatter formatter = new BinaryFormatter();
		internal ServerUsers ServerUsers { get; set; }

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
						if (user.ActionForServer == ActionForServer.Registration)
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
				
				GetUser(User);
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

                using DB DB = new DB();
                switch (Obj)
                {
                    case (Message):
                        this.GetMessade((Message)Obj);
                        break;
                    case (ClassesForServerClent.Class.User):
                        this.GetUser((User)Obj);
                        break;
                    case (Server):
                        this.GetServer((Server)Obj);
                        break;
					case String str:
						if (str == "closestream")
						{
							SendObjectToClient("closestream");
							return;
						}
						break;
                    default:
                        Console.WriteLine("Пользователь отправил объект, который не приводится к типам," +
                            " которые описаны в switch");
                        throw new Exception("Неопределенный тип");
                }
            } while (true);
        }

		private void GetMessade(Message message)
        {
			if(message is null)
				throw new ArgumentNullException("message is null", nameof(message));

			Console.WriteLine($"{User.RealName}: {message.Text}");

			using (DB DB = new DB())
            {
				message.ServerUser = DB.ServerUser.Find(ServerUser.ID);
				DB.Entry(message).State = EntityState.Added;
				DB.SaveChanges();
			}

			new Thread(ServerUsers.SendMessageToServer)
				.Start(message);
		}
		private void GetUser(User user)
		{
			if (user is null)
				throw new ArgumentNullException("user is null", nameof(user));

			Console.WriteLine($"{User.Name}: {user.Status}");

			new Thread(ServerObj.SendUserToServer).Start(User);
		}
		private void GetServer(Server server)
        {
			if (server is null)
				throw new ArgumentNullException("server is null", nameof(server));

			if (server.ActionOnServer == ActionOnServer.Connect)
            {
				using (DB db = new DB())
				{
					server.TextChat = db.TextChat
						.Include(x => x.Message)
						.Where(x => x.IDServer == server.ID)
						.ToList();

					server.ServerUser = db.ServerUser
						.Where(x => x.IDServer == server.ID)
						.ToList();

                    foreach (var item in server.ServerUser)
                    {
						item.User = db.User.FirstOrDefault(x => x.ID == item.IDUser);
						if (item.User is null)
							Console.WriteLine("item.User is null");
                    }
					foreach (var item in server.TextChat)
					{
						item.Message = db.Message
							.Include(x => x.ServerUser)
							.Where(x => x.IDTextChat == item.ID)
							.ToList();
					}

					ServerUser = db.ServerUser
						.FirstOrDefault(x => x.IDUser == User.ID && x.IDServer == server.ID);
				}
				if (ServerUsers is not null)
					ServerObj.RemoveConnection(this);
				ServerObj.AddConnection(this, server);
				SendObjectToClient(server);
			}
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
            using DB DB = new DB();

            User = DB.User.FirstOrDefault(x => x.RealName == rName);

            if (User == null)
                throw new Exception("Пользовотель не найден!");

            if (User.Password != password)
                throw new Exception("Отправленый пароль не совпадает с паролем из DB");

            try
            {
                User.ServerUser = DB.ServerUser
                    .Include(x => x.Server)
                    .Where(x => x.IDUser == User.ID)
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.InnerException?.Message);
                throw new Exception("Не удалось получить данные из DB, опять Debug!!!");
            }

            User.Status = Status.Online;
            DB.Entry(User).State = EntityState.Modified;

            DB.SaveChanges();
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

				GetUser(User);
			}
			ServerObj.RemoveConnection(this);

            if (Stream != null)
				Stream.Close();
			if (Client != null)
				Client.Close();
		}
	}
}