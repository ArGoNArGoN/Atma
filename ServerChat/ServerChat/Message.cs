using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChat
{
    public class Message
    {
        private String name;
        private String text;

        private DateTime DateTime { get; set; }
        public String DateCreateMessage { get => DateTime.ToShortTimeString(); }

        public Message(string name = "", string text = "")
        {
            Name = name;
            Text = text;
            DateTime = DateTime.Now;
        }
        public Message(DateTime dateTime, string name = "", string text = "")
        {
            Name = name ?? "";
            Text = text ?? "";
            DateTime = dateTime;
        }

        public string Name 
        { 
            get 
                => name;
            set 
                => name = value ?? "";
        }
        public string Text 
        { 
            get
                => text;
            set
                => text = value;
        }
    }
}
