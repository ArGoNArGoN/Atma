using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	public sealed class UserLog
	{
        private Int32 id;
        private User user;
        private String info;
        private DateTime date;
        public Int32 Id
		{
			get => id;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				id = value;
			}
		}

		[ForeignKey("")]
		public User User
		{
			get => user;
			set => user = value
				?? throw new ArgumentException("value is null", nameof(value));
		}
		public TypeActionUser TypeActionUser { get; set; }
		public String Info
		{
			get => info;
			set
			{
				if (value == null)
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Length > 500)
					throw new ArgumentException("value.Length > 500", nameof(value));

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
	}
}
