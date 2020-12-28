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
		private String password;
		private DateTime dater;
		private DateTime? dateB;
		private String rname;

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
		public User(Int32 id, String name, DateTime dateReg, String realName, Byte[] icon, Status status) 
			: this(id, name, realName)
		{
			Icon = icon;
			Status = status;
			DateReg = dateReg;
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
					throw new ArgumentNullException("The nickname cannot be empty!");

				if (value.Length > 15)
					throw new ArgumentNullException("The nickname is longer than 15 characters!");

				name = value;
			}
		}
		[Required]
		public String RealName
		{
			get => rname;
			set
			{
				if (value is null)
				{
					rname = value;
					return;
				}

				if (value.Length > 15)
					throw new ArgumentNullException("The Name is longer than 15 characters!");

				rname = value;
			}
		}
		public Byte[] Icon { get; set; }

		[Column(TypeName = "int")]
		public Status Status { get; set; }

		[Column(TypeName = "date")]
		public DateTime DateReg
		{
			get => dater;
			set
			{
				if ((value) > DateTime.Now)
					throw new ArgumentException("Are you trying to register in the future!? ");

				dater = value;
			}
		}

		[Column(TypeName = "date")]
		public DateTime? DateOfBirht
		{
			get => dateB;
			set
			{
				if ((value) > DateTime.Now)
					throw new ArgumentException("Were you born in the future!?");

				dateB = value;
			}
		}

		[Required]
		public String Password
		{
			get => password;
			set
			{
				if (value is null)
					return;

				if (value.Length < 6 || value.Length > 15)
					throw new ArgumentException("The password must be between 6 and 15 characters long!");

				password = value;
			}
		}

		[NotMapped]
		public ActionForServer ActionForServer { get; set; }
		[NotMapped]
		public ActionFromUser ActionFromUser { get; set; }
		[NotMapped]
		public StatusObj StatusObj { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public User()
		{
			EventLog = new HashSet<EventLog>();
			Opinion = new HashSet<Opinion>();
			Request = new HashSet<Request>();
			Request1 = new HashSet<Request>();
			ServerUser = new HashSet<ServerUser>();
			UserLog = new HashSet<UserLog>();
		}




		/// <summary>
		/// Ассоциация
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<EventLog> EventLog { get; set; }
		
		
		
		
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