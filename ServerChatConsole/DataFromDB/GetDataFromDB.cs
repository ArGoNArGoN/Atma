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
			var a = this.Server
				.Where(x => x.Name.Contains(name))
				.ToList();

			return a;
        }

		public ICollection<Opinion> GetOpinions(Int32 IDServer)
        {
			return this.Opinion
				.Where(x => x.IDServer == IDServer)
				.Include(x => x.User)
				.ToList();
        }
    }
}
