using System;
using System.Threading;



namespace ServerChatConsole
{
    class Program
	{
		/// Объект сервера
		static ServerObj Server { get; set; }

		/// Поток
		static Thread ThreadList { get; set; }

		static void Main()
		{
			try
			{
				Server = new ServerObj();
				ThreadList = new Thread(new ThreadStart(Server.Listen));
				ThreadList.Start();
			}
			catch (Exception ex)
			{
				Server.Disconnect();
				Console.WriteLine(ex.Message);
				Console.ReadLine();
			}
		}
	}
}
