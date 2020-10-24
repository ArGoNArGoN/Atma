using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atma.Class
{
	public sealed class TextChat : IChat
	{
		private Int32 id;
		private String name;
		private String info;
		private Int32? maxCountUser;
		private List<ServerUser> serverUsers = new List<ServerUser>();
		private List<Message> messages = new List<Message>();
		
		public TextChat(int id, string name, string info, int? maxCountUser = null)
        {
			try
			{
				Id = id;
				Name = name;
				Info = info;
				MaxCountUser = maxCountUser;
			}
            catch { throw; }
        }
        public List<ServerUser> ServerUsers 
		{
			get => serverUsers;
			set => serverUsers = value ??
					throw new ArgumentNullException("value is null");
		}
		public List<Message> Messages
		{
			get => messages;
			set => messages = value ??
					throw new ArgumentNullException("value is null");
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
		public String Name
        {
			get => name;
            set
            {
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null");
				if (value.Length > 50)
					throw new ArgumentException("value.Length > 50", "value");
				name = value;
			}
		}
		public String Info
        {
			get => info;
            set
            {
				if (value == null)
					throw new ArgumentNullException("value is null", "value");
				if (value.Length > 500)
					throw new ArgumentException("value.Length > 500", "value");
				info = value;
			}
		}
		public Int32? MaxCountUser
        {
			get => maxCountUser;
			set
            {
				if (value < 2)
					throw new ArgumentException("value < 2", "value");
				maxCountUser = value;
			}
		}
    }
}
