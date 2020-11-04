using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("TextChat")]
	public class TextChat
	{
		private Int32 id;
		private Int32 idServer;
		private Int32? idCategory;
		private String name;
		private String info;
		private Int32? maxCountUser;
		private List<ServerUser> serverUsers = new List<ServerUser>();
		private List<Message> messages = new List<Message>();
        private Server server;

        public TextChat(int id, string name, string info, int? maxCountUser = null)
		{
			try
			{
				ID = id;
				Name = name;
				Info = info;
				MaxCountUser = maxCountUser;
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
		public Int32? IDCategory
		{
			get => idCategory;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idCategory = value;
			}
		}
		public String Name
		{
			get => name;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Length > 50)
					throw new ArgumentException("value.Length > 50", nameof(value));

				name = value;
			}
		}
		public Int32? MaxCountUser
		{
			get => maxCountUser;
			set
			{
				if (value < 2)
					throw new ArgumentException("value < 2", nameof(value));

				maxCountUser = value;
			}
		}

		[Column("Info")]
		public String Info
		{
			get => info;
			set
			{
				if (value?.Length > 200)
					throw new ArgumentException("value.Length > 500", nameof(value));

				info = value;
			}
		}


		public Server Server
		{
			get => server;
			set => server = value
					?? throw new ArgumentNullException("value is null", nameof(value));
		}

		public Category Category { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public TextChat()
		{
			Message = new HashSet<Message>();
			Right = new HashSet<Right>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Message> Message { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Right> Right { get; set; }
	}
}
