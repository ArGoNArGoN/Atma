using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public User User = new User();
        public Reg()
        {
            InitializeComponent();
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(UserName.Text) && !String.IsNullOrWhiteSpace(Id.Text))
            {
                try
                {
                    User = new User() { Name = UserName.Text, ID = Int32.Parse(Id.Text) };
                    Close();
                }
                catch { }
            }
        }
    }
}
