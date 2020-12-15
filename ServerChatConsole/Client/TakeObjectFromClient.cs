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
						if(u.ActionFromUser == ActionFromUser.None)
							this.GetUserLoud(u);
						else
							this.GetUserLoud(u);
						break;

					case (Server s):
						this.GetServer(s);
						break;

					case (ServerUser s):
						this.GetServerUser(s);
						break;

					case (Request s):
						this.GetRequest(s);
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

        private void GetRequest(Request request)
        {
			using DB db = new DB();

            switch (request.StatusObj)
            {
                case StatusObj.Add:
					db.Request.Add(request);
                    break;

                case StatusObj.Edit:
					var req = db.Request.FirstOrDefault(x => x.IDFriend == request.IDFriend && x.IDUser == request.IDUser);
					if (req is null)
						break;

					req.UserRequest = request.UserRequest;
					req.FriendRequest = request.FriendRequest;
					break;

                case StatusObj.Delete:

					var request1 = request.ID < 0 ? db.Request.FirstOrDefault(x => x.IDFriend == request.IDFriend && x.IDUser == request.IDUser) : db.Request.Find(request.ID);
					db.Request.Remove(request1);
					break;
                default:
                    break;
			}

			db.SaveChanges();
		}
        private void GetServerUser(ServerUser s)
		{
			using DB db = new DB();

			switch (s.StatusObj)
			{
				case StatusObj.Add:
					db.ServerUser.Add(s);
					db.SaveChanges();

					var s1 = db.ServerUser.Include(x => x.Server).First(x => x.ID == s.ID);
					s1.StatusObj = StatusObj.Add;
					SendObjectToClient(s1);
					break;

				case StatusObj.Delete:
					var delserver = db.ServerUser.Include(x => x.Server).FirstOrDefault(x => x.ID == s.ID);
					delserver.StatusObj = StatusObj.Delete;
					SendObjectToClient(delserver);

					db.ServerUser.Remove(delserver);
					db.SaveChanges();
					break;

				default:
					break;
			}

			db.SaveChanges();
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
		private void GetUserLoud(User user)
		{
			using DB db = new DB();

			Object ob = null;

			if(user.StatusObj == StatusObj.Edit)
            {
                try
				{
					Console.WriteLine("Изменения!");
					var user1 = new User() { Name = user.Name, RealName = user.RealName, ID = user.ID, Password = user.Password, Status = Status.Online, DateOfBirht = user.DateOfBirht };
					db.Entry(user1).State = EntityState.Modified;
					if (user.ServerUser is not null && user.ServerUser.Count() > 0)
					{
						var ad = user.ServerUser.ToList()[0];
						
						db.Entry(new ServerUser() { ID = ad.ID, Status = ad.Status, Name = ad.Name, IDServer = ad.IDServer, IDUser = ad.IDUser, IDRole = ad.IDRole, Date = ad.Date}).State = EntityState.Modified;
						ServerUser = user.ServerUser.ToList()[0];
					}
					User = user;
				}
                catch (Exception e)
                {
					var u = db.User.Find(user.ID);
					u.StatusObj = StatusObj.Edit;
					SendObjectToClient(u);
					Console.WriteLine("dawdawd");
					db.SaveChanges();
				}

				db.SaveChanges();
				return;
			}

			switch (user.ActionFromUser)
			{
				case ActionFromUser.Loud:
					var aq = (Int32?)user.ServerUser.ToList()[0]?.IDServer;

					var SU = db.GetUser(user.ID, aq);
					SU.ActionFromUser = ActionFromUser.Loud;
					ob = SU;
					break;

				case ActionFromUser.LoudFriends:
					ob = db.GetRequestFriends(user.ID);
					break;

				case ActionFromUser.LoudReq:
					ob = db.GetRequest(user.ID);
					break;

				case ActionFromUser.Search:
					ob = db.GetUserSearch(user.Name, User.ID);
					break;

				case ActionFromUser.LoudUserLog:
					ob = db.GetUserLog(user.ID);
					break;

				default:
					break;
			}

			if (ob is not null)
				SendObjectToClient(ob);

			db.SaveChanges();
		}
		private void GetServer(Server server)
		{
			if (server is null)
				throw new ArgumentNullException("server is null", nameof(server));

			if (server.ActionOnServer == ActionOnServer.Connect)
			{
				using (DB db = new DB())
				{
					var s = db.GetServer(server.ID);
					s.TextChat = db.GetTextChatAndMessageFromServer(server.ID);
					s.ServerUser = db.GetServerUserFromServer(server.ID);

					SendObjectToClient(s.TextChat);
					SendObjectToClient(s.ServerUser);

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
				case ActionForServer.Search:
					obj = DB.GetServerSerch(server.Name);
					break;

				case ActionForServer.LoudTextChat:
					obj = DB.GetTextChatFromServer(server.ID);
					break;

				case ActionForServer.LoudOpinion:
					obj = DB.GetOpinions(server.ID);
					break;

				case ActionForServer.LoudEventLog:
					obj = DB.EventLog.Where(x => x.IDServer == server.ID).ToList();
					break;

				case ActionForServer.LoudServerUsers:
					obj = DB.GetServerUserFromServer(server.ID);
					break;

				case ActionForServer.LoudRole:
					obj = DB.Role
						.Include(x => x.RightRole)
						.Where(x => x.IDServer == server.ID)
						.ToList();
					break;

				case ActionForServer.Loud:
					obj = DB.GetServer(server.ID);
					break;

				case ActionForServer.Registration:
					obj = DB.CreateAndGetServerUser(server.ID, User.ID);
					break;

				default:
					break;
			}

			SendObjectToClient(obj);
		}
	}
}