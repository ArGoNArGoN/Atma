using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
				DateCreate = dateCreate;
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
					throw new ArgumentNullException("value.Length > 50", nameof(value));

				name = value;
			}
		}

		[Column(TypeName = "date")]
		public DateTime DateCreate
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
				if (value?.Length > 30)
					throw new ArgumentNullException("value.Length > 30", nameof(value));

				language = value;
			}
		}
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Server()
		{
			Chat = new HashSet<Chat>();
			EventLog = new HashSet<EventLog>();
			Opinion = new HashSet<Opinion>();
			Right = new HashSet<Right>();
			ServerUser = new HashSet<ServerUser>();
			TextChat = new HashSet<TextChat>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Chat> Chat { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<EventLog> EventLog { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Opinion> Opinion { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Right> Right { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<ServerUser> ServerUser { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<TextChat> TextChat { get; set; }
	}
}
