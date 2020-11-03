using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ServerChatConsole
{
    internal class ServerObj
    {
        /// Сервер для прослушивания
        internal static TcpListener TcpListener { get; set; }
        /// Список *Подключившихся на сервер* пользователей
        internal static List<ClientObject> ClientObjects { get; set; }
        private BinaryFormatter formatter = new BinaryFormatter();

        internal ServerObj()
        {
            ClientObjects = new List<ClientObject>();
        }
        
        internal void AddConnection(ClientObject client)
            => ClientObjects.Add(client);
        internal void RemoveConnection(Int32 id)
        {
            var client = ClientObjects.FirstOrDefault(x => x.User.ID == id);

            if (client != null)
                ClientObjects.Remove(client);
        }

        /// Добавляем пользователя на сервер
        internal void Listen()
        {
            try
            {
                StartServer();
                while (true) 
                {
                    GetClient();
                    Console.WriteLine("Кто-то пытается подключиться");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// Прослушиваем подключения
        /// Создаем пользователя (clientObj)
        /// Обрабатываем его в отдельном потоке
        private void GetClient()
        {
            var client = TcpListener.AcceptTcpClient();
            var clientObj = new ClientObject(client, this);
            new Thread(new ThreadStart(clientObj.Process)).Start();
        }

        /// Инициализируем прослушивание и запускаем сервер
        private void StartServer()
        {
            TcpListener = new TcpListener(IPAddress.Any, 8888);
            TcpListener.Start();
        }

        /// Отключаем пользователей и сервер
        internal void Disconnect()
        {
            ClientObjects.ForEach(x => x.Close());

            TcpListener.Stop();

            Environment.Exit(0);
        }

        /// ОТправляет сообщение всем пользователям
        internal void BroadcastMessage(Message message)
            => BroadcastMessage(message, message.IDUser);

        /// ОТправляет сообщение всем пользователям
        internal void BroadcastMessage(User User)
        {
            foreach (var item in ClientObjects)
            {
                if (item.User.ID != User.ID)
                    formatter.Serialize(item.Stream, User);
            }
        }

        /// ОТправляет сообщение всем пользователям
        internal void BroadcastMessage(Message message, Int32 iDUser)
        {
            if (iDUser < 0)
                throw new ArgumentException("idUser < 0", nameof(iDUser));

            ClientObjects
                .ForEach(x => formatter.Serialize(x.Stream, message));
        }
    }
}