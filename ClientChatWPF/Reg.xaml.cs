using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientChatWPF
{
    /// <summary>
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        public User User;
        public Reg()
        {
            InitializeComponent();
        }
        private const string host = "127.0.0.1";
        private const int port = 8888;
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }

        private void ClickButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Name.Text) || String.IsNullOrWhiteSpace(Password.Text))
            { ErrorText.Text = "Некоторые поля оказались незаполненными!"; return; }

            try
            {
                User = new User() { Name = Name.Text, Password = Password.Text };

                try
                {
                    Client = new TcpClient();
                    Client.Connect(host, port);
                    Stream = Client.GetStream();
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(Stream, User);
                    User = (User)formatter.Deserialize(Stream);
                    Close();
                }
                catch (Exception ex)
                {
                    ErrorText.Text = ex.Message;
                    User = null;
                }
            }
            catch 
            {
                ErrorText.Text = "Вы ввели некорректные данные";
                User = null;
            }
        }
    }
}
