using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientChatWPF
{
	public static class SendMessageToServer
	{
		private static BinaryFormatter formatter = new BinaryFormatter();
		public static NetworkStream Stream { get; set; }

		public static void SendMessageSerialize(Object message)
		{
			try
			{
				formatter.Serialize(Stream, message);
			}
			catch (Exception) { }
		}
		public static Object TakeMessageSerialize()
			=> formatter.Deserialize(Stream);
	}
}
