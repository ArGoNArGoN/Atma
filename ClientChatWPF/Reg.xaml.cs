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
        public User User;
        public Reg()
        {
            InitializeComponent();
        }


        private void ClickButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Name.Text) || String.IsNullOrWhiteSpace(Password.Text))
            { ErrorText.Text = "Некоторые поля оказались незаполненными!"; return; }


            try
            {
                User = new User() { Name = Name.Text, ID = Int32.Parse(Password.Text) };
                Close();
            }
            catch { User = null; }
        }
    }
}
