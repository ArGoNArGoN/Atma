using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
    [Serializable]
    [Table("Chat")]
    public class Chat
       : IChat
    {
        private Int32 id;
        private Int32 idServer;
        private Int32 type;
        private String name;
        private Int32? maxCountUser;
        private Server server;
        private String info;
        private Int32 number;

        public Int32 ID
        {
            get => id;
            set
            {
                if (value < 1)
                    throw new ArgumentException("value < 1", nameof(value));

                id = value;
            }
        }
        public Int32 IDServer
        {
            get => idServer;
            set
            {
                if (value < 1)
                    throw new ArgumentException("value < 1", nameof(value));

                idServer = value;
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
        public Int32 Number
        {
            get => number;
            set
            {
                if (value < 0)
                    throw new ArgumentException("value < 0", nameof(value));
                number = value;
            }
        }

        public Server Server
        {
            get => server;
            set => server = value
                ?? throw new ArgumentNullException("value is null", nameof(value));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chat()
        {
            EventLog = new HashSet<EventLog>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<EventLog> EventLog { get; set; }
    }
}