using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("EventLog")]
	public class EventLog
	{
		private Int32 id;
		private Int32 idServer;
		private Int32? idUser;
		private Server server;
        private String message;
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
		public Int32 IDServer
		{
			get => idServer;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idServer = value;
			}
		}
		public Int32? IDUser
		{
			get => idUser;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idUser = value;
			}
		}
		[Column(TypeName = "int")]
		public ActionInServer Action { get; set; }
		public String Message
		{
			get => message;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Trim().Length > 200)
					throw new ArgumentNullException("value.Length > 200", nameof(value));

				message = value.Trim();
			}
		}
		[Column(TypeName = "int")]
		public TypeEventServer TypeEvent { get; set; }
		[Column(TypeName = "date")]
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


		public virtual Server Server
		{
			get => server;
			set => server = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public virtual User User { get; set; }
	}
}
