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
		private String name;
		private String info;
		private Int32 number;
		private List<ServerUser> serverUsers = new List<ServerUser>();
		private List<Message> messages = new List<Message>();
		private Server server;

		[NotMapped]
		public ActionForTextChat ActionForTextChat { get; set; }

		public TextChat(int id, string name, string info)
		{
			try
			{
				ID = id;
				Name = name;
				Info = info;
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
		public Int32 Number
		{
			get => number;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));
				number = value;
			}
		}

		public Server Server
		{
			get => server;
			set => server = value
					?? throw new ArgumentNullException("value is null", nameof(value));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public TextChat()
		{
			Message = new HashSet<Message>();
			EventLog = new HashSet<EventLog>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Message> Message { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<EventLog> EventLog { get; set; }

		[NotMapped]
		public StatusObj StatusObj { get; set; }
    }
}
