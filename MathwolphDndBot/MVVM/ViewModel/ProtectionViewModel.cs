using MathwolphDndBot.Core;
using MathwolphDndBot.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MathwolphDndBot.MVVM.ViewModel
{
    internal class ProtectionViewModel : ObservableObject
    {
        public ObservableCollection<ServerModel> Servers { get; set; }

        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        private string _connectionStatus;

        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set { 
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConnectCommand { get; set; }

        public ProtectionViewModel()
        {
            Servers = new ObservableCollection<ServerModel>();

            for (int i = 0; i < 10; i++)
            {
                Servers.Add(new ServerModel()
                {
                    Country = "USA"
                });
            }

            ConnectCommand = new RelayCommand(o => 
            {
                Task.Run(() =>
                {
                    ConnectionStatus = "Connecting...";

                    var process = new Process();

                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                    process.StartInfo.ArgumentList.Add(@"/c rasdial MyServer vpnbook b7dh4n3 /phonebook:./VPN/VPN.pbk");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.WaitForExit();

                    switch (process.ExitCode)
                    {
                        case 0:
                            Debug.WriteLine("Success!");
                            ConnectionStatus = "Success!";

                            break;
                        case 691:
                            Debug.WriteLine("Wrong credentials!");
                            ConnectionStatus = "Wrong credentials!";

                            break;
                        default:
                            Debug.WriteLine($"Error: {process.ExitCode}");
                            ConnectionStatus = $"Error: {process.ExitCode}";

                            break;
                    }
                });
            });
        }

        private void ServerBuilder()
        {
            var address = "CA149.vpnbook.com";
            var folderPath = $"{Directory.GetCurrentDirectory()}/VPN";
            var pbkPath = $"{folderPath}/{address}.pbk";

            if (!Directory.Exists(folderPath)) 
                Directory.CreateDirectory(folderPath);

            if (File.Exists(pbkPath))
            {
                MessageBox.Show("Connection Already exists!");
                return;
            }

            var sb = new StringBuilder();

            sb.AppendLine("[MyServer]");
            sb.AppendLine("MEDIA=rastapi");
            sb.AppendLine("Port=VPN2-0");
            sb.AppendLine("Device=WAN Miniport (IKEv2)");
            sb.AppendLine("DEVICE=vpn");
            sb.AppendLine($"PhoneNumber={address}");

            File.WriteAllText(pbkPath, sb.ToString());
        }
    }
}
