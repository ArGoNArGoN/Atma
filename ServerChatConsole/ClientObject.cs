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
		internal NetworkStream Stream { get; set; }
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
					{ Console.WriteLine(e.Message); return; }
					catch(Exception e)
					{ this.SendObjectToClient(e); return; }

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
                    case (Message m):
                        this.GetMessade(m);
                        break;
                    case (User u):
                        this.GetUser(u);
                        break;
                    case (Server s):
                        this.GetServer(s);
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
					var TC = db.TextChat
						.Include(x => x.Message)
						.Where(x => x.IDServer == server.ID)
						.ToList();

					foreach (var item in TC)
					{
						item.Message = db.Message
							.Include(x => x.ServerUser)
							.Where(x => x.IDTextChat == item.ID)
							.ToList();
					}

					SendObjectToClient(TC);

					var SU = db.ServerUser
						.Where(x => x.IDServer == server.ID)
						.ToList();

					foreach (var item in SU)
					{
						item.User = db.User.FirstOrDefault(x => x.ID == item.IDUser);
						if (item.User is null)
							Console.WriteLine("item.User is null");
					}

					SendObjectToClient(SU);

					ServerUser = db.ServerUser
						.FirstOrDefault(x => x.IDUser == User.ID && x.IDServer == server.ID);
				}
				if (ServerUsers is not null)
					ServerObj.RemoveConnection(this);

                ServerObj.AddConnection(this, server);

				return;
			}
            using DB DB = new DB();

			var obj = new Object();

			switch (server.ActionForServer)
            {
                case ActionForServer.None:
                    break;
                case ActionForServer.Search:
                    break;
                case ActionForServer.LoudTextChat:
					obj = DB.TextChat.Where(x => x.IDServer == server.ID).ToList();
					break;
                case ActionForServer.LoudOpinion:
					obj = DB.Opinion.Include(x => x.User).Where(x => x.IDServer == server.ID).ToList();
					break;
                case ActionForServer.LoudEventLog:
					obj = DB.EventLog.Where(x => x.IDServer == server.ID).ToList();
					break;
                case ActionForServer.LoudServerUsers:
                    var a = DB.ServerUser.Include(x => x.User).Include(x => x.Role).Where(x => x.IDServer == server.ID).ToList();
					a.ForEach(x => x.User.Password = null);
					obj = a;
					break;
                case ActionForServer.LoudChat:
					obj = DB.Chat.Where(x => x.IDServer == server.ID).ToList();
					break;
                case ActionForServer.LoudRole:
					obj = DB.Role.Include(x => x.RightRole).Where(x => x.IDServer == server.ID).ToList();
					break;
                case ActionForServer.Loud:
					obj = DB.Server.Find(server.ID);
                    break;
                case ActionForServer.Registration:
                    break;
                case ActionForServer.Cheack:
                    break;
                default:
                    break;
            }
			SendObjectToClient(obj);
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