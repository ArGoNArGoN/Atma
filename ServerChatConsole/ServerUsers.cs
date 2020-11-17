using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerChatConsole
{
    /// <summary>
    /// </summary>
    class ServerUsers
    {
        public Server Server;
        public List<ClientObject> ClientObjectsOfServer = new List<ClientObject>();

        public void SendMessageToServer(Object msg)
        {
            if (msg is not Message)
                throw new ArgumentException("msg is not Message", nameof(msg));

            try
            {
                ClientObjectsOfServer
                    .ForEach(x => x.SendObjectToClient(msg));
            }
            catch (Exception) { }
        }
        public void SendUserToServer(Object usr)
        {
            if(usr is not User)
                throw new ArgumentException("usr is not User", nameof(usr));
            try
            {
                foreach (var client in ClientObjectsOfServer)
                {
                    if (((User)usr).ID != client.User?.ID)
                        client.SendObjectToClient(usr);
                }
            }
            catch (Exception) { }
        }
        public void SendServerToServer(Object srvr)
        {
            if (srvr is not ClassesForServerClent.Class.Server)
                throw new ArgumentException("srvr is not Server", nameof(Server));
            try
            {
                ClientObjectsOfServer.ForEach(x => x.SendObjectToClient(srvr));
            }
            catch (Exception) { }
        }

        internal void Close(ClientObject client)
        {
            var a = ClientObjectsOfServer
                .First(x => x.User.ID == client.User.ID);
            
            if(a is not null)
                ClientObjectsOfServer.Remove(a);
        }
    }
}
