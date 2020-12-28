namespace ServerChatConsole
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using ClassesForServerClent.Class;

	public partial class DB : DbContext
	{
		public DB()
			: base("name=DB")
		{
		}

		public virtual DbSet<Chat> Chat { get; set; }
		public virtual DbSet<EventLog> EventLog { get; set; }
		public virtual DbSet<Message> Message { get; set; }
		public virtual DbSet<Opinion> Opinion { get; set; }
		public virtual DbSet<Request> Request { get; set; }
		public virtual DbSet<RightRole> RightRole { get; set; }
		public virtual DbSet<Role> Role { get; set; }
		public virtual DbSet<Server> Server { get; set; }
		public virtual DbSet<ServerUser> ServerUser { get; set; }
		public virtual DbSet<TextChat> TextChat { get; set; }
		public virtual DbSet<User> User { get; set; }
		public virtual DbSet<UserLog> UserLog { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Entity<Chat>()
                .HasMany(e => e.EventLog)
                .WithOptional(e => e.Chat)
                .HasForeignKey(e => e.IDChat);

            modelBuilder.Entity<Message>()
                .HasMany(e => e.EventLog)
                .WithOptional(e => e.Message)
                .HasForeignKey(e => e.IDMessage);

            modelBuilder.Entity<Opinion>()
                .HasMany(e => e.EventLog)
                .WithOptional(e => e.Opinion)
                .HasForeignKey(e => e.IDOpinion);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.EventLog)
                .WithOptional(e => e.Role)
                .HasForeignKey(e => e.IDRole);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.RightRole)
                .WithRequired(e => e.Role)
                .HasForeignKey(e => e.IDRole)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.ServerUser)
                .WithOptional(e => e.Role)
                .HasForeignKey(e => e.IDRole);

            modelBuilder.Entity<Server>()
                .HasMany(e => e.Chat)
                .WithRequired(e => e.Server)
                .HasForeignKey(e => e.IDServer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Server>()
                .HasMany(e => e.EventLog)
                .WithRequired(e => e.Server)
                .HasForeignKey(e => e.IDServer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Server>()
                .HasMany(e => e.Opinion)
                .WithRequired(e => e.Server)
                .HasForeignKey(e => e.IDServer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Server>()
                .HasMany(e => e.Role)
                .WithRequired(e => e.Server)
                .HasForeignKey(e => e.IDServer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Server>()
                .HasMany(e => e.ServerUser)
                .WithRequired(e => e.Server)
                .HasForeignKey(e => e.IDServer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Server>()
                .HasMany(e => e.TextChat)
                .WithRequired(e => e.Server)
                .HasForeignKey(e => e.IDServer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Server>()
                .HasMany(e => e.UserLog)
                .WithOptional(e => e.Server)
                .HasForeignKey(e => e.IDServer);

            modelBuilder.Entity<ServerUser>()
                .HasMany(e => e.Message)
                .WithRequired(e => e.ServerUser)
                .HasForeignKey(e => e.IDServerUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TextChat>()
                .HasMany(e => e.EventLog)
                .WithOptional(e => e.TextChat)
                .HasForeignKey(e => e.IDTextChat);

            modelBuilder.Entity<TextChat>()
                .HasMany(e => e.Message)
                .WithRequired(e => e.TextChat)
                .HasForeignKey(e => e.IDTextChat)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.EventLog)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.IDUser);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Opinion)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IDUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Request)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IDUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Request1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.IDFriend)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ServerUser)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IDUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserLog)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IDUser)
                .WillCascadeOnDelete(false);
        }
	}
}
