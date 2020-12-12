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
    /// Логика взаимодействия для WindowFriendsAndUsers.xaml
    /// </summary>
    public partial class WindowFriendsAndUsers : Window
    {        
        /// <summary>
        /// Инициализ. при Открытии формы
        /// </summary>
        public User User { get; set; }
        public WindowFriendsAndUsers()
        {
            InitializeComponent();
        }

        public WindowFriendsAndUsers(User user)
            : this()
            =>  User = user;

        private void TabControlSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
