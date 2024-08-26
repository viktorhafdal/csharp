using System.Diagnostics;
using System.Threading;
using Tcp_Server.Server;

namespace Program {
    public class Program {
        public static void Main() { 
            //Launching each Client in seperate terminal window
            ProcessStartInfo serverStartInfo = new ProcessStartInfo();
            serverStartInfo.FileName = "cmd.exe";
            serverStartInfo.Arguments = $"/C start cmd.exe /K dotnet run --project C:\\Users\\Viktor\\Documents\\GitHub\\Side-Projects\\csharp\\Tcp_Server\\Tcp_Server.Server\\Tcp_Server.Server.csproj";
            serverStartInfo.RedirectStandardInput = false;
            serverStartInfo.RedirectStandardOutput = false;
            serverStartInfo.UseShellExecute = false;
            serverStartInfo.CreateNoWindow = false;
            Process process = new Process();
            process.StartInfo = serverStartInfo;
            process.Start();

            //Giving time to launch server
            Thread.Sleep(2000);

            StartClientInNewTerminal();
            Thread.Sleep(500);

            StartClientInNewTerminal();
            Thread.Sleep(500);

            //StartClientInNewTerminal();
            //Thread.Sleep(500);
        }

        private static void StartClientInNewTerminal() {
            ProcessStartInfo clientStartInfo = new ProcessStartInfo();
            clientStartInfo.FileName = "cmd.exe";
            clientStartInfo.Arguments = $"/C start cmd.exe /K dotnet run --project C:\\Users\\Viktor\\Documents\\GitHub\\Side-Projects\\csharp\\Tcp_Server\\Tcp_Server.Client\\Tcp_Server.Client.csproj";
            clientStartInfo.RedirectStandardInput = false;
            clientStartInfo.RedirectStandardOutput = false;
            clientStartInfo.UseShellExecute = false;
            clientStartInfo.CreateNoWindow = false;
            Process clientProcess = new Process();
            clientProcess.StartInfo = clientStartInfo;
            clientProcess.Start();
        }
    }
}