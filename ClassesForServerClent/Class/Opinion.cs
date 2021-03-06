﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("Opinion")]
	public class Opinion
	{
		private Int32 id;
		private Int32 idUser;
		private User user;
		private Int32 idServer;
		private Server server;
		private Int32 mark;
		private String message;
		private DateTime date;


		public Int32 ID
		{
			get => id;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", "value");

				id = value;
			}
		}
		public Int32 IDUser
		{
			get => idUser;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idUser = value;
			}
		}
		public Int32 IDServer
		{
			get => idServer;
			set
			{
				if (value < 1)
					throw new ArgumentException("value < 1", nameof(value));

				idServer = value;
			}
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

				if (value.Trim().Length > 100)
					throw new ArgumentNullException("value.Length > 50", nameof(value));

				message = value.Trim();
			}
		}

		[Column(TypeName = "datetime")]
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

		public Opinion()
		{
			EventLog = new HashSet<EventLog>();
		}

		public Server Server
		{
			get => server;
			set => server = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public User User
		{
			get => user;
			set => user = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}


		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<EventLog> EventLog { get; set; }
	}
}
