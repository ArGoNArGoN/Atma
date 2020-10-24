using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atma.Class
{
	public sealed class Message
	{
		private int id;
		private String text;
		public PinnedMessage PinnedMessage { get; set; }

		public DateTime DateCreate { get; set; } = DateTime.Now;
		public String DateTimeCreate { get => DateCreate.ToShortTimeString(); }
		public Stream File  { get; set; }
		private User user;

        public Message(int id, string text, User serverUser)
        {
			try
			{ 
				Id = id;
				Text = text;

				User = serverUser;
			}
            catch { throw; }
        }

        public Message(PinnedMessage pinnedMessage, DateTime dateCreate, Stream file, int id, string text, User serverUser) : this(id, text, serverUser)
        {
			try
			{
				PinnedMessage = pinnedMessage;
				DateCreate = dateCreate;
				File = file;

            }
            catch { throw; }
        }

        public Int32 Id
		{
			get => id;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", "value");
				id = value;
			}
		}
		public String Text
		{
			get => text;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", "value");

				if (value.Trim().Length > 500)
					throw new ArgumentNullException("value.Length > 50", "value");

				text = value.Trim();
			}
		}
		public User User 
		{ 
			get => user;
			set => user = value
				?? throw new ArgumentNullException("value is null", "value");
		}
		public String UserName { get => User.Name; }
	}
}
