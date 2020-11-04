using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("User")]
	public class User
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
				ID = id;
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
				Date = dateReg;
			}
			catch { throw; }
		}

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
		[Required]
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
		[Required]
		public String RealName
		{
			get => rname;
			set 
			{
				if (String.IsNullOrWhiteSpace(value)) 
					throw new ArgumentNullException("value is null", nameof(value));
				
				if (value.Length > 50)
					throw new ArgumentNullException("value = null", nameof(value));

				rname = value;
			}
		}
		public String Icon
		{
			get => icon;
			set 
			{
				if (value?.Length > 100)
					throw new ArgumentException("value > 100", nameof(value));

				icon = value;
			} 
		}

		[Column(TypeName = "int")]
		public Status Status { get; set; }

		public String Status2
		{
			get => status;
			set 
			{
				if (value?.Length > 100)
					throw new ArgumentException("value > 100", nameof(value));

				status = value;
			}
		}

		[Column(TypeName = "datetime")]
		public DateTime Date
		{
			get => dater;
			set
			{
				if ((value) > DateTime.Now)
					throw new ArgumentException("value > DataTime.Now", nameof(value));

				dater = value;
			}
		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public User()
		{
			EventLog = new HashSet<EventLog>();
			Message = new HashSet<Message>();
			Opinion = new HashSet<Opinion>();
			Request = new HashSet<Request>();
			Request1 = new HashSet<Request>();
			ServerUser = new HashSet<ServerUser>();
			UserLog = new HashSet<UserLog>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<EventLog> EventLog { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Message> Message { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Opinion> Opinion { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Request> Request { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Request> Request1 { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<ServerUser> ServerUser { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<UserLog> UserLog { get; set; }
	}
}