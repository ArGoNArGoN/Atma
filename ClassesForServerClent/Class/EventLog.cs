using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("EventLog")]
	public class EventLog
	{
		private Int32 id;
		private Int32 idServer;
		private Int32? idUser;
		private Int32? idChat;
		private Int32? idTextChat;
		private Int32? idRole;
		private Int32? idRight;
		private Int32? idMessage;
		private Int32? idOpinion;
		private Server server;
        private String message;
		private DateTime date;

        public Int32 ID
		{
			get => id;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				id = value;
			}
		}
		public Int32 IDServer
		{
			get => idServer;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idServer = value;
			}
		}
		public Int32? IDUser
		{
			get => idUser;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idUser = value;
			}
		}
		public Int32? IDChat
		{
			get => idChat;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idChat = value;
			}
		}
		public Int32? IDTextChat
		{
			get => idTextChat;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idTextChat = value;
			}
		}
		public Int32? IDRole
        {
			get => idRole;
			set
            {
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));
				
				idRole = value;
            }
        }
		public Int32? IDRight
		{
			get => idRight;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idRight = value;
			}
		}
		public Int32? IDMessage
        {
			get=> idMessage;
			set
            {
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idMessage = value;
            }
        }
		public Int32? IDOpinion
		{
			get => idOpinion;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idOpinion = value;
			}
		}

		[Column(TypeName = "int")]
		public ActionInServer Action { get; set; }
		[Column(name: "Message")]
		public String Text
		{
			get => message;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Trim().Length > 200)
					throw new ArgumentNullException("value.Length > 200", nameof(value));

				message = value.Trim();
			}
		}
		[Column(TypeName = "datetime")]
		public DateTime Date
		{
			get => date;
			set
			{
				if (date > DateTime.Now)
					throw new ArgumentException("date > DateTime.Now", nameof(value));

				date = value;
			}
		}

		public Server Server
		{
			get => server;
			set => server = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public Chat Chat { get; set; }
		public Message Message { get; set; }
		public Opinion Opinion { get; set; }
		public Role Role { get; set; }
		public TextChat TextChat { get; set; }
		public User User { get; set; }
	}
}
