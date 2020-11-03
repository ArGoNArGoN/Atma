using ClassesForServerClent.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ServerChatConsole
{
	internal class ClientObject
	{
		public ClientObject(TcpClient client, ServerObj serverObj)
		{
			Client = client;
			ServerObj = serverObj;
		}

		public TcpClient Client { get; set; }
		public ServerObj ServerObj { get; set; }
		public NetworkStream Stream { get; internal set; }
		internal User User { get; set; }
		private DB DB { get; set; } = new DB();
		private BinaryFormatter formatter = new BinaryFormatter();

		/// <summary>
		/// Пользователь должен отправить Свое имя и id
		/// </summary>
		internal void Process()
		{
			try
			{
				Stream = Client.GetStream();

				GetUser();
				Console.WriteLine("Подключаем пользователя!");

				ServerObj.AddConnection(this);

				UpdateStatusDB();
				/// Отправляем пользователю текстовые чаты и сообщения
				SendServers();
				Console.WriteLine("Отправляем ему Текстовые часты");

				GetMessageWhile();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				if (User != null)
				{
					ServerObj.RemoveConnection(User.ID);
				}
				Close();
			}
		}

        private void UpdateStatusDB()
        {
			User.Status = Status.Online;
			DB.SaveChanges();
        }

        private void GetMessageWhile()
		{
			do
			{
				var message = GetMessage();
				DB.Message.Add(message);
				Console.WriteLine($"{User.Name}: {message.Text}");
				ServerObj.BroadcastMessage(message);
				DB.SaveChanges();
			}
			while (true);
		}

        private Message GetMessage()
		{
			return (Message)formatter.Deserialize(Stream);
		}

		private void SendTextChatAndMessage()
		{
			var textChats = DB.TextChat.Include(x => x.Message).ToList();

            foreach (var item in textChats)
            {
				item.Message = DB.Message
					.Include(x => x.User)
					.Where(x => x.IDTextChat == item.ID)
					.ToList();
            }

			formatter.Serialize(Stream, textChats.ToArray());
		}

		private void GetUser()
		{
			if (!((formatter.Deserialize(Stream)) is User user))
				throw new InvalidOperationException($"Не получилось получить объект класса User!");
			if (FindUser(user.Name, user.ID))
				throw new InvalidOperationException($"Пользователь \"{user.Name}\" с ID \"{user.ID}\" не найден!");
		}

		internal void Close()
		{
			User.Status = Status.Offline;
			DB.SaveChanges();
			if (Stream != null)
				Stream.Close();
			if (Client != null)
				Client.Close();
		}
		
		private Boolean FindUser(String rName, Int32 id)
		{
			User = DB.User.Find(id);
			return User.Name.Trim() != rName.Trim() || User == null;
		}
	}
}