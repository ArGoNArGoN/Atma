using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("RightRole")]
	public class RightRole
	{
        private Int32 id;
		private Int32 idRole;
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

		public DateTime Date { get; set; }
		public int Priority { get; set; }

		public Role Role
		{
			get => role;
			set => role = value
				?? throw new ArgumentException("value is null", nameof(value));
		}
	}
}
