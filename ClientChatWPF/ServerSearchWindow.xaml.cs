using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ClientChatWPF
{
    /// <summary>
    /// Логика взаимодействия для ServerSearchWindow.xaml
    /// </summary>
    public partial class ServerSearchWindow : Window
    {
        public User User { get; set; } = null;
        
        ///
        public List<Server> Servers { get; set; } = new List<Server>();

        public ServerSearchWindow(User user)
        {
            InitializeComponent();
            User = user;
            ServersList.ItemsSource = User.ServerUser.Select(x => x?.Server);
            SerchServers.ItemsSource = Servers;
        }
    }
}
