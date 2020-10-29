using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	public sealed class Right
	{
        private Int32 id;
        private Server server;
        private String name;
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
		public Server Server
		{
			get => server;
			set => server = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public Chat Chat { get; set; }
		public TextChat TextChat { get; set; }
		public User User { get; set; }
		public Category Category { get; set; }
		public String Name
		{
			get => name;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));

				name = value;
			}
		}
		public Priority Priority { get; set; }
		public String Info
		{
			get => info;
			set
			{
				if (value == null)
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Length > 500)
					throw new ArgumentException("value > 500", nameof(value));

				info = value;
			}
		}
		public DateTime Date
		{
			get => date;
			set
			{
				if ((value) > DateTime.Now)
					throw new ArgumentException("value > DataTime.Now", nameof(value));

				date = value;
			}
		}
	}
}
