using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
	[Table("Role")]
	public class Role
	{
        private Int32 id;
		private Int32 idServer;
		private ServerUser server;
        private String name;
        private String info;
        private Int32 priority;
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
		public String Name
		{
			get => name;
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value is null", nameof(value));
				
				if (value.Length > 50)
					throw new ArgumentNullException("value = null", nameof(value));

				name = value;
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
		public Int32 Priority
		{
			get => priority;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				priority = value;
			}
		}
		[Column(TypeName = "date")]
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


		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Role()
		{
			RightRole = new HashSet<RightRole>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<RightRole> RightRole { get; set; }

		public virtual ServerUser ServerUser
		{
			get => server;
			set => server = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
	}
}
