using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
    [Serializable]
    [Table("Chat")]
    public class Chat
    {
        private Int32 id;
        private Int32 idServer;
        private Int32? idCategory;
        private Int32 type;
        private String name;
        private Int32? maxCountUser;
        private Server server;
        private String info;

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
        public Int32 Type
        {
            get => type;
            set
            {
                if (value < 0)
                    throw new ArgumentException("value < 0", nameof(value));

                type = value;
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
                    throw new ArgumentNullException("value = null", nameof(value));

                name = value;
            }
        }
        public Int32? MaxCountUser 
        {
            get => maxCountUser;
            set => maxCountUser = value > 0 ? value
                : throw new ArgumentException("value < 0", nameof(value));
        }
        public String Info
        {
            get => info;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value is null", nameof(value));

                if (value.Length > 200)
                    throw new ArgumentException("value > 200", nameof(value));

                info = value;
            }
        }

        public virtual Server Server
        {
            get => server;
            set => server = value
                ?? throw new ArgumentNullException("value is null", nameof(value));
        }
        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chat()
        {
            Right = new HashSet<Right>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Right> Right { get; set; }
    }
}