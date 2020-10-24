using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Atma.Class
{
    internal sealed  class Category
    {
        private int id;
        private string name;
        private string info;
        public List<IChat> GetChats { get; } = new List<IChat>();
        public Int32 ID
        {
            get => id;
            set
            {
                if (value < 0) throw new ArgumentException("value < 0", "value");
                id = value;
            }
        }
        public string Name
        {
            get => name;
            set 
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("value = NULL", "value");
                if (value.Length > 50) throw new ArgumentException("value.Lenght > 50", "value");
                name = value;
            }
        }
        public string Info
        {
            get => info;
            set
            {
                if (value == null) throw new ArgumentNullException("value = NULL", "value");
                if (value.Length > 500) throw new ArgumentException("value.Lenght > 500", "value");
                info = value;
            }
        }
    }
}