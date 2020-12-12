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
	internal partial class ClientObject
	{
		private void TakeObjectFromUser()
		{
			using DB DB = new DB();
			do
			{
				var Obj = formatter.Deserialize(Stream);

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
			if (message is null)
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
	}
}
