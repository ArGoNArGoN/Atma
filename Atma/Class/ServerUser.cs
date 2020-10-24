using System;


namespace Atma.Class
{
	public sealed class ServerUser
	{
		private Int32 id;
		public Status Status { get; set; }
        private Server server;
		private User user;

        public ServerUser(Int32 id, Server server, User user)
        {
			try
			{
				Id = id;
				Server = server;
				User = user;
			}
            catch { throw; }
        }

        public Int32 Id
		{
			get => id;
			set
			{
				if (value < 0) throw new ArgumentException("value < 0", "value");
				id = value;
			}
		}
		public Server Server 
		{
			get => server;
			set => server = value 
					?? throw new ArgumentNullException("value is null", "value");
		}
		public User User 
		{
			get => user;
			set => user = value
					?? throw new ArgumentNullException("value is null", "value");
		}
	}
}