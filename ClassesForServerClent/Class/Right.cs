using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("Right")]
	public class Right
	{
        private Int32 id;
        private Server server;
        private String name;
        private String info;
		private DateTime date;
        private Int32 idServer;
		private Int32? idChat;
		private Int32? idTextChat;
		private Int32? idUser;
        private Int32? idCategory;

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
		public Int32? IDChat
		{
			get => idChat;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idChat = value;
			}
		}
		public Int32? IDTextChat
		{
			get => idTextChat;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idTextChat = value;
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
		public Int32? IDCategory
		{
			get => idCategory;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idCategory = value;
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
					throw new ArgumentException("value.Length > 50", nameof(value));

				name = value;
			}
		}
		[Column(TypeName = "int")]
		public Priority Priority { get; set; }
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
		[Column(TypeName = "datetime")]
		public DateTime Date
		{
			get => date;
			set
			{
				if ((value) > DateTime.Now)
					throw new ArgumentException("value > DataTime.Now", nameof(value));

				date = value;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Right()
		{
			RightRole = new HashSet<RightRole>();
		}

		public Server Server
		{
			get => server;
			set => server = value
				?? throw new ArgumentNullException("value is null", nameof(value));
		}
		public Chat Chat { get; set; }
		public TextChat TextChat { get; set; }
		public ServerUser ServerUser { get; set; }
		public Category Category { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public ICollection<RightRole> RightRole { get; set; }
	}
}
