using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("Request")]
	public class Request
	{
		private Int32 id;
		private Int32 idUser;
		private Int32 idFriend;
		private User user;
		private User friend;

        public Int32 ID
		{
			get => id;
			set
			{
				if (value < 0) throw new ArgumentException("value < 0", nameof(value));
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
		public Int32 IDFriend
		{
			get => idFriend;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idFriend = value;
			}
		}
		public Boolean UserRequest { get; set; }
		public Boolean FriendRequest { get; set; }
		[Column(TypeName = "datetime")]
		public DateTime Date { get; set; } = DateTime.Now;

		public User User
		{
			get => user;
			set => user = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public User User1
		{
			get => friend;
			set => friend = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
	}
}
