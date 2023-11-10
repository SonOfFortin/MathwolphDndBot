using System.ComponentModel;
using System.Windows;

namespace MathwolphDndBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void closingForm(object sender, CancelEventArgs e)
        {
            //if (bot.IsConnected)
            //{
            //    bot.Disconnect();
            //}
        }

        private void Button_Click_add_player(object sender, RoutedEventArgs e)
        {
            //if (sender == null)
            //    return;

            //Button button = sender as Button;

            //if (button == null)
            //    return;

            //bot.AddPlayer(button.DataContext.ToString() ?? string.Empty);
        }

        private void Button_Click_del_player(object sender, RoutedEventArgs e)
        {
            //if (sender == null)
            //    return;

            //Button button = sender as Button;

            //if (button == null)
            //    return;

            //bot.RemoveRequestPlayers(button.DataContext.ToString() ?? string.Empty);
        }
    }
}
