using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ClassesForServerClent.Class
{
	public sealed class User
	{
		private Int32 id;
		private String name;
		private DateTime dater;
		private String rname;
		private String icon;
		private String status;

		public User(Int32 id, String name, String realName)
		{
			try
			{
				Id = id;
				Name = name;
				RealName = realName;
			}
			catch { throw; }
		}
		public User(Int32 id, String name, DateTime dateReg, String realName, String icon, Status status, String status2) : this(id, name, realName)
		{
			try
			{ 
				Icon = icon;
				Status = status;
				Status2 = status2;
				DateReg = dateReg;
			}
			catch { throw; }
		}

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
		public String Name
		{
			get => name;
			set 
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));

				if (value.Length > 50)
					throw new ArgumentNullException("value = null", nameof(value));

				name  = value;
			}
		}
		public DateTime DateReg
		{
			get => dater;
			set 
			{
				if ((value) > DateTime.Now)
					throw new ArgumentException("value > DataTime.Now", nameof(value));

				dater = value;
			}
		}
		public String RealName
		{
			get => rname;
			set 
			{
				if (String.IsNullOrWhiteSpace(value)) 
					throw new ArgumentNullException("value is null", nameof(value));

				rname = value;
			}
		}
		public String Icon
		{
			get => icon;
			set 
			{
				if (String.IsNullOrWhiteSpace(value)) 
					throw new ArgumentNullException("value is null", nameof(value));

				icon = value;
			} 
		}
		public Status Status { get; set; }
		public String Status2
		{
			get => status;
			set 
			{
				if (value == null) 
					throw new ArgumentNullException("Status is null", nameof(value));

				if (value.Length > 100)
					throw new ArgumentException("value > 100", nameof(value));

				status = value;
			}
		}
	}
}