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
		internal static List<ServerUsers> ServerUsers { get; set; }
		private BinaryFormatter formatter = new BinaryFormatter();

		internal ServerObj()
		{
			ServerUsers = new List<ServerUsers>();
		}

		internal void AddConnection(ClientObject client, Server server)
        {
			var a = ServerUsers
				.Find(x => x.Server.ID == server.ID);
			a.ClientObjectsOfServer.Add(client);
			client.ServerUsers = a;
		}
		internal void RemoveConnection(ClientObject client)
		{
			client.ServerUsers?.Close(client);
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
			TcpListener = new TcpListener(IPAddress.Any, 888);
			TcpListener.Start();
			ServerUsers = new List<ServerUsers>();

            using DB db = new DB();

            foreach (var server in db.Server)
            {
                ServerUsers.Add(new ServerUsers() { Server = server });
            }
        }

		/// Отключаем пользователей и сервер
		/// Доделать
		internal void Disconnect()
		{
			ServerUsers
				.Select(x => x.ClientObjectsOfServer)
				.Aggregate((x, y) => x.Concat(y).ToList())
				.Distinct()
				.ToList()
				.ForEach(x => x.Close());

			TcpListener.Stop();

			Environment.Exit(0);
		}

        internal void SendUserToServer(Object ob)
        {
			if (ob is not User user)
				throw new Exception("ob is not User user");

            foreach (var item in user.ServerUser)
            {
				var a = ServerUsers.FirstOrDefault(x => x.Server.ID == item.IDServer);
				if (a is not null)
					new Thread(a.SendUserToServer).Start(item);
            }
        }
    }
}