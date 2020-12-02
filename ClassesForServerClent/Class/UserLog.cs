using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("UserLog")]
	public class UserLog
	{
        private Int32 id;
		private Int32 idUser;
		private Int32? idServer;
		private User user;
        private String info;
        private DateTime date;

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
		public Int32? IDServer
		{
			get => idServer;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idServer = value;
			}
		}
		[Column(TypeName = "int")]
		public TypeActionUser Action { get; set; }
		
		public String Message
		{
			get => info;
			set
			{
				if (value == null)
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Length > 100)
					throw new ArgumentException("value.Length > 100", nameof(value));

				info = value;
			}
		}

		public DateTime Date
		{
			get => date;
			set
			{
				if (value > DateTime.Now)
					throw new ArgumentException("date > DateTime.Now", nameof(value));

				date = value;
			}
		}

		public Server Server { get; set; }

		public User User
		{
			get => user;
			set => user = value
				?? throw new ArgumentException("value is null", nameof(value));
		}
	}
}
