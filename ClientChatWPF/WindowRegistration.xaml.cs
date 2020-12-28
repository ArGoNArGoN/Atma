using ClassesForServerClent.Class;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace ClientChatWPF
{
    /// <summary>
    /// Логика взаимодействия для Reg2.xaml
    /// </summary>
    public partial class Reg2 : Window
	{
		public User User { get; private set; } = null;
		public Reg2()
		{
			InitializeComponent();
		}
		public NetworkStream Stream { get; set; }

		private void RegisterClick(object sender, RoutedEventArgs e)
		{
			if (Text2.Text != Text3.Text)
			{ Error.Text = "Passwords don't match!"; return; }
			DateTime? date = null;
			if (!String.IsNullOrWhiteSpace(Text4.Text))
			{
				try
				{
					date = DateTime.Parse(Text4.Text);
				}
				catch (Exception)
				{ Error.Text = "Use \"dd mm yy\"";  return; }
			}

			try
			{
				User = new User()
				{
					RealName = Text1.Text.Trim(),
					Password = Text2.Text.Trim(),
					DateOfBirht = date,
					Name = Text6.Text.Trim(),
					ActionForServer = ActionForServer.Registration,
				};

				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(Stream, User);
				Object ob = formatter.Deserialize(Stream);

				if (ob is User user)
					User = user;

				else if (ob is Exception exception)
					throw new Exception(exception.Message);

				else
					throw new Exception("An unknown error occurred!");

				Close();
			}
			catch(Exception ex)
			{ Error.Text = ex.Message; User = null; }
		}
		private void GoOutClick(object sender, RoutedEventArgs e)
		{
			User = null;
			Close();
		}
	}
}
