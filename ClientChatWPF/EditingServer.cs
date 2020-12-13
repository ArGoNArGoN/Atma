using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientChatWPF
{
    public partial class WindowEditingServer
    {
        private void EditingServerClick(object sender, RoutedEventArgs e)
        {
            var server = new Server()
            {
                ActionForServer = ActionForServer.Editing,
                ID = Server.ID,
                Name = Name.Text,
                Status = (Boolean)StatusServer.IsChecked,
                Info = Info.Text,
                Language = Language.Text
            };
            Server = server;
            SendMessageToServer.SendMessageSerialize(server);
        }

        private void EditingServerCancelClick(object sender, RoutedEventArgs e)
        {
            UpServer(Server);
        }

        private void UserOnServerEditClick(object sender, RoutedEventArgs e)
        {

        }

        private void UserOnServerCancelClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddRoleClick(object sender, RoutedEventArgs e)
        {

        }

        private void CancelAddRoleClick(object sender, RoutedEventArgs e)
        {

        }

        private void AssignRoleClick(object sender, RoutedEventArgs e)
        {

        }

        private void DelRoleClick(object sender, RoutedEventArgs e)
        {

        }

        private void AddTextChatClick(object sender, RoutedEventArgs e)
        {
            var tC = new TextChat()
            {
                Name = NameTextChat.Text,
                IDServer = Server.ID,
                StatusObj = StatusObj.Add
            };
            SendMessageToServer.SendMessageSerialize(tC);
        }

        private void EditTextChatClick(object sender, RoutedEventArgs e)
        {

        }

        private void DelTextChatClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
