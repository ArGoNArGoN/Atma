using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("Server")]
	public class Server
	{
		private Int32 id;
		private String name;
		private String info;
		private String language;
		private DateTime dateCreate;

		public Server(Int32 id, String name, DateTime dateCreate, String language = "Void", String info = "", Boolean status = false)
		{
			try
			{
				ID = id;
				Name = name;
				Language = language;
				Info = info;
				Status = status;
				Date = dateCreate;
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

				if (value.Length > 15)
					throw new ArgumentNullException("value.Length > 15", nameof(value));

				name = value;
			}
		}

		[Column(TypeName = "datetime")]
		public DateTime Date
		{
			get => dateCreate;
			set
			{
				if (value > DateTime.Now)
					throw new ArgumentException("value > DateTime.Now", nameof(value));
				
				dateCreate = value;
			}
		}
		public Boolean Status { get; set; }
		public String Language
		{
			get => language;
			set
			{
				if (value?.Length > 15)
					throw new ArgumentNullException("value.Length > 15", nameof(value));

				language = value;
			}
		}

		[StringLength(500)]
		public String Info
		{
			get => info;
			set
			{
				if (value?.Length > 500)
					throw new ArgumentException("value > 500", nameof(value));

				info = value;
			}
		}

		[Column(TypeName = "image")]
		public byte[] Icon { get; set; }

		[NotMapped]
		public ActionForServer ActionForServer { get; set; }

		[NotMapped]
		public ActionOnServer ActionOnServer { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Server()
		{
			Chat = new HashSet<Chat>();
			EventLog = new HashSet<EventLog>();
			Opinion = new HashSet<Opinion>();
			Role = new HashSet<Role>();
			ServerUser = new HashSet<ServerUser>();
			TextChat = new HashSet<TextChat>();
			UserLog = new HashSet<UserLog>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Chat> Chat { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<EventLog> EventLog { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Opinion> Opinion { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<Role> Role { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<ServerUser> ServerUser { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<TextChat> TextChat { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<UserLog> UserLog { get; set; }
	}
}
