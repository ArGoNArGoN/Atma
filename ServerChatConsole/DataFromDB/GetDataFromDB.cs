using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ServerChatConsole
{
	public partial class DB
	{
		public Server GetServer(Int32 IDServer)
		{
			return this.Server.Find(IDServer);	
		}
		
		public ICollection<ServerUser> GetServerUserFromServer(Int32 IDServer)
		{
			var SU = this.ServerUser
				.Include(x => x.User)
				.Include(x => x.Role)
				.Where(x => x.IDServer == IDServer)
				.ToList();

			SU.ForEach(x => { x.User.Password = null; x.User.RealName = null; });
			return SU;
		}

		private ICollection<User> GetUsers(Int32 IDServer)
		{
			return this.ServerUser
				.Where(x => x.IDServer == IDServer)
				.Include(x => x.User)
				.Select(x => x.User)
				.ToList();
		}
		                                 
		public ICollection<TextChat> GetTextChatFromServer(Int32 IDServer)
		{
			return this.TextChat
				.Where(x => x.IDServer == IDServer)
				.ToList();
		}

		public ICollection<TextChat> GetTextChatAndMessageFromServer(Int32 IDServer)
		{
			return this.TextChat
				.Include(x => x.Message)
				.Where(x => x.IDServer == IDServer)
				.ToList();
		}

		public ICollection<Message> GetMessageFromServer(Int32 IDTextChat)
		{
			return this.Message
				.Include(x => x.ServerUser)
				.Where(x => x.IDTextChat == IDTextChat)
				.ToList();
		}

		public ServerUser CreateAndGetServerUser(Int32 IDServer, Int32 IDUser)
		{
			var SU = new ServerUser() { IDServer = IDServer, IDUser = IDUser };
			this.ServerUser.Add(SU);

			return SU;
		}

		public ICollection<Server> GetServerSerch(String name)
		{
			return this.Server
				.Where(x => x.Name.Contains(name))
				.ToList();
		}

		public ICollection<Opinion> GetOpinions(Int32 IDServer)
		{
			return this.Opinion
				.Where(x => x.IDServer == IDServer)
				.Include(x => x.User)
				.ToList();
		}

		public User GetUser(Int32 ID, Int32? IDServer)
		{
			var u = this.User
				.Find(ID);

			if (IDServer is not null)
				u.ServerUser.Add(this.ServerUser
					.Where(x => x.IDServer == IDServer && x.IDUser == ID).First());

			return u;
		}

		private User GetUserReq(Request request, Int32 IDUser)
		{
			if (request.IDFriend == IDUser)
				return request.User;
			else if (request.IDUser == IDUser)
				return request.User1;
			else
				throw new Exception();
		}
		public ICollection<User> GetRequestFriends(Int32 IDUser)
		{
			return this.Request
				.Where(x => (x.IDUser == IDUser || x.IDFriend == IDUser) && (x.UserRequest && x.FriendRequest))
				.Include(x => x.User1)
				.Include(x => x.User)
				.AsEnumerable()
				.Select(x => GetUserReq(x, IDUser)).ToList();
		}

		public ICollection<Request> GetRequest(Int32 IDUser)
		{
			return this.Request
				.Where(x => (x.IDUser == IDUser || x.IDFriend == IDUser) && (x.UserRequest || x.FriendRequest) && !(x.UserRequest && x.FriendRequest))
				.Include(x => x.User1)
				.Include(x => x.User)
				.ToList();
		}

		public ICollection<User> GetUserSearch(String name, Int32 IDUser)
		{
			return this.User
				.Include(x => x.Request)
				.Include(x => x.Request1)
				.Where(x => x.Name.Contains(name) && x.ID != IDUser 
				&& x.Request.All(x => x.IDFriend != IDUser && x.IDUser != IDUser) 
				&& x.Request1.All(x => x.IDFriend != IDUser && x.IDUser != IDUser))
				.ToList();
		}

		public ICollection<UserLog> GetUserLog(Int32 iDUser)
		{
			return this.UserLog
				.Where(x => x.IDUser == iDUser).ToList();
		}
	}
}
