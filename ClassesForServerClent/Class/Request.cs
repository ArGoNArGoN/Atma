using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	public sealed class Request
	{
		private Int32 id;
		private User user;
		private User friend;

		public Int32 Id
		{
			get => id;
			set
			{
				if (value < 0) throw new ArgumentException("value < 0", nameof(value));
				id = value;
			}
		}
		public User User
		{
			get => user;
			set => user = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public User Friend
		{
			get => friend;
			set => friend = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public Boolean UserRequest { get; set; }
		public Boolean FriendRequest { get; set; }
	}
}
