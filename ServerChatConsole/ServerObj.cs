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
		
		internal void AddConnection(ClientObject client)
		{
            foreach (var item in client.User.ServerUser)
            {
				client.ServerUsers.Add(ServerUsers.First(x => x.Server.ID == item.IDServer));
            }
            foreach (var item in client.ServerUsers)
            {
				item.ClientObjectsOfServer.Add(client);
			}
		}

		internal void RemoveConnection(ClientObject client)
		{
            foreach (var item in client.ServerUsers)
            {
				item.Close(client);
            }
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
			TcpListener = new TcpListener(IPAddress.Any, 22222);
			TcpListener.Start();
			ServerUsers = new List<ServerUsers>();

            using DB db = new DB();

            foreach (var server in db.Server)
            {
                ServerUsers.Add
                    (new ServerUsers() { Server = server });
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
	}
}