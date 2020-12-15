using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesForServerClent.Class
{
	[Serializable]
	[Table("ServerUser")]
    public class ServerUser
    {
        private Int32 id;
        private Int32 idUser;
        private Int32 idServer;
        private Int32? idRole;
        private Server server;
        private User user;

        public ServerUser(Int32 id, Server server, User user)
        {
            try
            {
                ID = id;
                Server = server;
                User = user;
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
        public Int32 IDUser
        {
            get => idUser;
            set
            {
                if (value < 0)
                    throw new ArgumentException("value < 0", nameof(value));

                idUser = value;
            }
        }
        public Int32? IDRole
        {
            get => idRole;
            set
            {
                if (value < 0)
                    throw new ArgumentException("value < 0", nameof(value));

                idRole = value;
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
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; } = DateTime.Now;

        public Server Server
        {
            get => server;
            set => server = value;
        }
        public Role Role { get; set; }
        public User User
        {
            get => user;
            set => user = value;
        }

        [NotMapped]
        public Status Status { get; set; }

        [NotMapped]
        public StatusObj StatusObj { get; set; }

        [NotMapped]
        public ActionFromUser ActionFromUser { get; set; }

        [StringLength(15)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Status2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServerUser()
        {
            Message = new HashSet<Message>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Message> Message { get; set; }
    }
}