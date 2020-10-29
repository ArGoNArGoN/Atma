using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesForServerClent.Class
{
	public sealed class RightRole
	{
        private int id;
        private Right right;
        private Role role;

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
		public Right Right
        {
			get => right;
			set => right = value
				?? throw new ArgumentException("value is null", nameof(value); 
        }
		public Role Role
		{
			get => role;
			set => role = value
				?? throw new ArgumentException("value is null", nameof(value);
		}
	}
}
