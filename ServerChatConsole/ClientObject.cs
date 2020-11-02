using ClassesForServerClent.Class;
using System;
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

				ServerObj.AddConnection(this);

				/// Отправляем пользователю сообщение об успешной операции
				formatter.Serialize(Stream, true);

				/// Отправляем пользователю текстовые чаты и сообщения
				SendTextChatAndMessage();

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
					formatter.Serialize(Stream, false);
				}
				Close();
			}
		}

        private void GetMessageWhile()
		{
			do
			{
				var message = GetMessage();
				DB.Message.Add(message);
				Console.WriteLine($"{User.Name}: {message.Text}");
				ServerObj.BroadcastMessage(message);
			}
			while (true);
		}

        private Message GetMessage()
		{
			return (Message)formatter.Deserialize(Stream);
		}

		private void SendTextChatAndMessage()
		{
			var textChats = DB.TextChat
				.Include(x => x.Message)
				.Include(x => x.Info)
				.Include(x => x.MaxCountUser)
				.ToList();

			formatter.Serialize(Stream, textChats);
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
			throw new NotImplementedException();
		}
		
		private Boolean FindUser(String rName, Int32 id)
		{
			User = DB.User.Find(id);
			return User.RealName.Trim() != rName.Trim() || User == null;
		}
	}
}