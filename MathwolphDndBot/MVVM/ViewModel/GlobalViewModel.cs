using MathwolphDndBot.Core;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class GlobalViewModel
    {
        public static GlobalViewModel Instance { get; } = new GlobalViewModel();
        public Bot Bots { get; set; } = new Bot();

        public GlobalViewModel()
        {
            
        }

        public void ChgStateBots()
        {
            if (Bots.IsConnected)
            {
                Bots.Disconnect();
            }
            else
            {
                Bots.Connect();
            }
        }
    }
}
