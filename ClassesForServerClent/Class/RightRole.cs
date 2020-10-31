using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
    [Table("RightRole")]
	public class RightRole
	{
        private Int32 id;
		private Int32 idRight;
		private Int32 idRole;
		private Right right;
        private Role role;

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
		public Int32 IDRight
		{
			get => idRight;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idRight = value;
			}
		}
		public Int32 IDRole
		{
			get => idRole;
			set
			{
				if (value < 0)
					throw new ArgumentException("value < 0", nameof(value));

				idRole = value;
			}
		}

		public virtual Right Right
        {
			get => right;
			set => right = value
				?? throw new ArgumentException("value is null", nameof(value)); 
        }
		public virtual Role Role
		{
			get => role;
			set => role = value
				?? throw new ArgumentException("value is null", nameof(value));
		}
	}
}
