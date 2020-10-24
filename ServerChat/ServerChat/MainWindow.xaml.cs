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

namespace ServerChat
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			listUserMessage.Items.Clear();
			listUserMessage.ItemsSource = new List<Message>() 
			{
				new Message()
				{
					Name = "Name",
					Text = "йпйп" 
				},
				new Message()
				{
					Name = "Name",
					Text = "dawdawdawdawd awd aw daw daw daw daw\n dwa daw dawd awd awd awd awdawd awd a\n daw daw daw daw d"
				},
				new Message()
				{
					Name = "Name",
					Text = " daw da wda wdaw da wd aw, da wd awd\n d awd aw dawd awd \nd awd awd awdaw daw daw d\n daw daw daw daw dawd "
				}
			};
		}
	}
}
