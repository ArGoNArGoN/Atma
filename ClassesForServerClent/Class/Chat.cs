using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassesForServerClent.Class
{
    public sealed class Chat : IChat
    {
        private List<ServerUser> serverUsers;
        private Int32 id;
        private Server server;

        [Key]
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
        public Server Server
        {
            get => server;
            set => server = value
                ?? throw new ArgumentNullException("value is null", nameof(value));
        }
        public Category Category { get; set; }

        public List<ServerUser> ServerUsers
        {
            get => serverUsers;
            set => serverUsers = value
                ?? throw new ArgumentNullException("serverUser is null", nameof(value));
        }

    }
}