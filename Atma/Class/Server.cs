using System;
using System.Collections.Generic;

namespace Atma.Class
{
    internal sealed class Server
	{
		private Int32 id;
		private String name;
		private String info;
		private String language;
		private DateTime dateCreate;
		private List<User> list = new List<User>();
		public StatusServer Status { get; set; }

		public Server(Int32 id, String name, DateTime dateCreate, String language = "Void", String info = "", StatusServer status = StatusServer.@private)
        {
			try
			{
				Id = id;
				Name = name;
				Language = language;
				Info = info;
				Status = status;
				DateCreate = dateCreate;
			}
            catch { throw; }
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
					throw new ArgumentNullException("value is null", "value");

				if (value.Length > 50)
					throw new ArgumentNullException("value.Length > 50", "value");

				name = value;
			}
		}
		public String Language
		{
			get => language;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is value", "language");
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
					throw new ArgumentException("value > 500", "value");

				info = value;
			}
		}
		public DateTime DateCreate
		{
			get => dateCreate;
			set
			{
				if (value > DateTime.Now)
					throw new ArgumentException("value > DateTime.Now", "value");
				dateCreate = value;
			}
		}
		
		/// <summary>
		/// ??
		/// а если User = null
		/// </summary>
		public List<User> List
		{
			get => list;
			set
			{
				if (value == null)
					throw new ArgumentNullException("value = null", "value");
				list = value;
			}
		}
	}
}
