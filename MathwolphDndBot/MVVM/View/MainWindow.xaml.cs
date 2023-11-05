using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MathwolphDndBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Bot bot = new Bot();

        public MainWindow()
        {
            InitializeComponent();
            ChangeCtrlConnectionSattus(true);

            /*txt_chanel.Text = TwitchInfo.ChannelName;
            txt_token.Text = TwitchInfo.BotToken;

            lv_wait_user.ItemsSource = bot.RequestPlayers;
            lv_terminal.ItemsSource = bot.Terminals;*/

            //bot.OnRequestedPlayer += (o, PlayerName => lv_wait_user.ItemsSource = bot.RequestPlayers) ;

        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            //ChangeCtrlConnectionSattus(bot.IsConnected);

            //if (bot.IsConnected)
            //{
            //    bot.Disconnect();
            //}
            //else
            //{
            //    bot.Connect(true);
            //}
        }

        private void closingForm(object sender, CancelEventArgs e)
        {
            //if (bot.IsConnected)
            //{
            //    bot.Disconnect();
            //}
        }

        private void ChangeCtrlConnectionSattus(bool value)
        {
            //txt_chanel.IsEnabled = value;
            //txt_token.IsEnabled = value;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*if(bot.RequestPlayers.Count > 1)
            {
                lv_wait_user.Items.Refresh();
                return;
            }
            lv_wait_user.ItemsSource = bot.RequestPlayers;*/
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //bot.test();
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
