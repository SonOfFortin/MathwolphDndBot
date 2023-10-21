using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace MathwolphDndBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bot bot = new Bot();

        public MainWindow()
        {
            InitializeComponent();
            ChangeCtrlConnectionSattus(true);

            txt_chanel.Text = TwitchInfo.ChannelName;
            txt_token.Text = TwitchInfo.BotToken;
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            ChangeCtrlConnectionSattus(bot.IsConnected);

            if (bot.IsConnected)
            {
                bot.Disconnect();
            }
            else
            {
                bot.Connect(true);
            }
        }

        private void closingForm(object sender, CancelEventArgs e)
        {
            if (bot.IsConnected)
            {
                bot.Disconnect();
            }
        }

        private void ChangeCtrlConnectionSattus(bool value)
        {
            txt_chanel.IsEnabled = value;
            txt_token.IsEnabled = value;
        }
    }
}
