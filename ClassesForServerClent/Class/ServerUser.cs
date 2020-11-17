using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("ServerUser")]
	public class ServerUser
	{
		private Int32 id;
		private Int32 idUser;
		private Int32 idServer;
		private Server server;
		private User user;

        public ServerUser(Int32 id, Server server, User user)
		{
			try
			{
				ID = id;
				Server = server;
				User = user;
			}
			catch { throw; }
		}

		public Int32 ID
		{
			get => id;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				id = value;
			}
		}
		public Int32 IDUser
		{
			get => idUser;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idUser = value;
			}
		}
		public Int32 IDServer
		{
			get => idServer;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idServer = value;
			}
		}
		[Column(TypeName ="datetime")]
		public DateTime Date { get; set; } = DateTime.Now;

		public Server Server 
		{
			get => server;
			set => server = value;
		}
		
		public User User 
		{
			get => user;
			set => user = value;
		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ServerUser()
		{
			Right = new HashSet<Right>();
			Role = new HashSet<Role>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Right> Right { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Role> Role { get; set; }
	}
}