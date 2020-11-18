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
    /// Логика взаимодействия для UpClient.xaml
    /// </summary>
    public partial class UpClient : Window
    {
        /// <summary>
        /// Инициализ. при Открытии формы
        /// </summary>
        public User User { get; set; }
        public UpClient(User user)
        {
            User = user;
            InitializeComponent();
        }

        /// <summary>
        /// Изменяет пользователя и закрывает форму.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpClientClick(object sender, RoutedEventArgs e)
        {
            /// Логика изменения пользователя
            /// После успешного изменения форма должна закрытся
            /// и у пользователя должно проинициализироваться след. поле
            

            User.ActionForServer = ActionForServer.Upp;
            Close();

            /// Дальше сервер сохраняет информациию о пользователе.
        }

        /// <summary>
        /// Закрывает форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoOutClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
