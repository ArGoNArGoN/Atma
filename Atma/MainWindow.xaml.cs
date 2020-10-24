using Atma.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Atma
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public User User { get; private set; }
		public ServerUser ServerUser { get; private set; }
		public Server Server { get; private set; }
		public TextChat TextChat { get; private set; }

		/// <summary>
		/// так нельзя!!!
		/// </summary>
		private Int32 Count { get; set; } = 1;

		public MainWindow()
		{
			User = new User(2, "консерВич", "Консервыч");
			Server = new Server(2, "Chat", DateTime.Now);
			ServerUser = new ServerUser(2, Server, User);
			TextChat = new TextChat(2, "Chatik", "");

			var messages = new List<Message>()
			{
				new Message(Count++, "dawd", User),
				new Message(Count++, "dawdw awdwa", User),
				new Message(Count++, "dawd", User),
			};

			TextChat.Messages.AddRange(messages);
			InitializeComponent();
			listUserMessage.Items.Clear();
			listUserMessage.ItemsSource = TextChat.Messages;
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			if (MessagePop.Text.Trim() == "")
				return;

			try
			{
				var message = new Message(Count++, MessagePop.Text, User);
				TextChat.Messages.Add(message);

				listUserMessage.ItemsSource = null;
				listUserMessage.ItemsSource = TextChat.Messages;
			}
			catch (Exception e1) { MessageBox.Show(e1.Message); }
			finally { MessagePop.Text = ""; }
		}
	}
}
