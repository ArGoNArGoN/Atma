using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	public sealed class Opinion
	{
        private Int32 id;
        private Int32 mark;
		private User user;
		private Server server;
		private string message;
        private DateTime date;

        public Int32 Id
		{
			get => id;
			set
			{
				if (value < 0) throw new ArgumentException("value < 0", "value");
				id = value;
			}
		}
		public User User 
		{
			get => user;
			set => user = value 
				?? throw new ArgumentException("value is null", nameof(value));
		}
		public Server Server
		{
			get => server;
			set => server = value
				?? throw new ArgumentException("value is null", nameof(value));
		}
		public Int32 Mark
        {
			get => mark;
			set
            {
				if (value < 0 || value > 10)
					throw new ArgumentException("value < 0 || value > 10", nameof(value));

				mark = value;
            }
        }
		public String Message
		{
			get => message;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Trim().Length > 200)
					throw new ArgumentNullException("value.Length > 50", nameof(value));

				message = value.Trim();
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
