using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	[Table("Category")]
	public class Category
	{
		private int id;
		private string name;
		private string info;
		private int type;
		
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
		public string Name
		{
			get => name;
			set 
			{
				if (String.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("value = NULL", nameof(value));

				if (value.Length > 50)
					throw new ArgumentException("value.Lenght > 50", nameof(value));

				name = value;
			}
		}
		public string Info
		{
			get => info;
			set
			{
				if (value == null)
					throw new ArgumentNullException("value = NULL", nameof(value));

				if (value.Length > 200)
					throw new ArgumentException("value.Lenght > 500", nameof(value));

				info = value;
			}
		}

		public int Type 
		{
			get => type;
			set
			{
				if(value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				type = value;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Category()
		{
			Chat = new HashSet<Chat>();
			Right = new HashSet<Right>();
			TextChat = new HashSet<TextChat>();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Chat> Chat { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Right> Right { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<TextChat> TextChat { get; set; }
	}
}