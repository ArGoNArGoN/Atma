using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	public sealed class EventLog
	{
		private Int32 id;
        private Server server;
        private String message;
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
		public ActionInServer Action { get; set; }
		public String Message
		{
			get => message;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Trim().Length > 500)
					throw new ArgumentNullException("value.Length > 50", nameof(value));

				message = value.Trim();
			}
		}
		public TypeEventServer TypeEvent { get; set; }
		public DateTime Date
		{
			get => date;
			set
			{
				if (date > DateTime.Now)
					throw new ArgumentException("date > DateTime.Now", nameof(value));

				date = value;
			}
		}
	}
}
