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
		public DateTime DateCreate { get; set; }
		public Stream File  { get; set; }
		private ServerUser user;

        public Message(int id, string text, ServerUser serverUser)
        {
			try
			{ 
				Id = id;
				Text = text;
				ServerUser = serverUser;
			}
            catch { throw; }
        }

        public Message(PinnedMessage pinnedMessage, DateTime dateCreate, Stream file, int id, string text, ServerUser serverUser) : this(id, text, serverUser)
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

				if (value.Length > 500)
					throw new ArgumentNullException("value.Length > 50", "value");

				text = value;
			}
		}

		public ServerUser ServerUser 
		{ 
			get => user;
			set => user = value
				?? throw new ArgumentNullException("value is null", "value");
		}
		public String UserName { get => ServerUser.User.Name; }
	}
}
