using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Atma.Class
{
	internal sealed class User
	{
		private int id;
		private string name;
		private DateTime dater;
		private string rname;
		private string icon;
		private string status;

		public Int32 ID
		{
			
			get => id;
			set 
			{
				if (value < 1) throw new ArgumentException("value<0", "value");
					id = value;
			}
		}
		public string Name
        {
			get 
			{
				return name;
			}
			set 
			{
				if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("value = NULL");
					name  = value;
			}
        }
		public DateTime DateReg
		{
			get => dater;
			set 
			{
				if ((value) > DateTime.Now) throw new ArgumentException("value > DataTime.Now");
					dater = value;
			}
		}
		public string RealName
		{
			get => rname;
			set 
			{
				if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("RealName is NULL");
					rname = value;
			}
		}
		public string Icon
		{
			get => icon;
            set 
			{
				if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("Icon");
					icon = value;
			} 
		}
		public Status Status { get; set; }
		public string Status2
		{
			get => status;
			set 
			{
				if (value == null) throw new ArgumentNullException("Status is NULL");
				if (value.Length > 100) throw new ArgumentNullException("value>100");
					status = value;
			}
		}
	}
}