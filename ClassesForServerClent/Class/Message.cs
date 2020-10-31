using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	[Table("Message")]
	public class Message
	{
		private Int32 id;
		private Int32 idTextChat;
		private Int32 idUser;
		private String text;
		private User user;
        private TextChat textChat;

        public Message(int id, string text, User serverUser)
		{
			try
			{ 
				ID = id;
				Text = text;

				User = serverUser;
			}
			catch { throw; }
		}
		public Message(Boolean pinnedMessage, DateTime dateCreate, String file, int id, string text, User serverUser) : this(id, text, serverUser)
		{
			try
			{
				PinnedMessage = pinnedMessage;
				DateCreate = dateCreate;
				File = file;

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
		public Int32 IDTextChat
		{
			get => idTextChat;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idTextChat = value;
			}
		}
		public String Text
		{
			get => text;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Trim().Length > 500)
					throw new ArgumentNullException("value.Length > 50", nameof(value));

				text = value.Trim();
			}
		}

		[Column(TypeName = "bit")]
		public Boolean PinnedMessage { get; set; }

		[Column(TypeName = "date")]
		public DateTime DateCreate { get; set; } = DateTime.Now;

		public String File { get; set; }


		public virtual User User
		{
			get => user;
			set => user = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public virtual TextChat TextChat
		{
			get => textChat;
			set => textChat = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}

		public String DateTimeCreate { get => DateCreate.ToShortTimeString(); }
		public String UserName { get => User.Name; }
	}
}
