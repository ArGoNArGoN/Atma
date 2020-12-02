using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("Message")]
	public class Message
	{
		private Int32 id;
		private Int32 idTextChat;
		private Int32 idUser;
		private String text;

		private ServerUser user;
        private TextChat textChat;

		public Message() 
		{
			EventLog = new HashSet<EventLog>();
		}
        public Message(int id, string text, ServerUser serverUser) : base()
		{
			try
			{ 
				ID = id;
				Text = text;

				ServerUser = serverUser;
			}
			catch { throw; }
		}
		public Message(Boolean pinnedMessage, DateTime dateCreate, String file, int id, string text, ServerUser serverUser) : this(id, text, serverUser)
		{
			try
			{
				PinnedMessage = pinnedMessage;
				Date = dateCreate;
				File = file;

			}
			catch { throw; }
		}

		[Key]
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
		public Int32 IDServerUser
		{
			get => idUser;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idUser = value;
			}
		}
		public Int32 IDTextChat
		{
			get => idTextChat;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

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

		[Column(TypeName = "datetime")]
		public DateTime Date { get; set; } = DateTime.Now;

		public String File { get; set; }

		public ServerUser ServerUser
		{
			get => user;
			set => user = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public TextChat TextChat
		{
			get => textChat;
			set => textChat = value;
		}

		[NotMapped]
		public String DateTimeCreate { get => Date.ToShortTimeString(); }
		[NotMapped]
		public String UserName { get => ServerUser?.Name; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<EventLog> EventLog { get; set; }
    }
}
