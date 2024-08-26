using System.Net;
using System.Net.Sockets;

namespace Tcp_Server.Server {
    internal class ClientWorker {
        private TcpClient client;
        private ClientManager clientManager;
        private string clientName;
        private string workerName;
        private bool loggedIn = false;
        private Dictionary<string, string> users;

        public ClientWorker(ClientManager clientManager, TcpClient client, string clientName, string workerName, Dictionary<string, string> users) {
            this.clientManager = clientManager;
            this.client = client;

            this.clientName = clientName;
            this.workerName = workerName;
            this.users = users;
        }

        public void Run() {
            string clientIp = (client.Client.RemoteEndPoint as IPEndPoint).Address.ToString();
            Console.WriteLine($"[{workerName}] New connection from: {clientIp}");
            bool firstMessage = true;

            try {
                StreamWriter writer = new StreamWriter(client.GetStream());
                StreamReader reader  = new StreamReader(client.GetStream());

                string msg = string.Empty;

                //if (firstMessage) {
                //    SendLine(writer, "::You are connected to the server::");
                //    firstMessage = false;
                //}

                while (!msg.Equals("logout", StringComparison.OrdinalIgnoreCase)) {

                    if (!loggedIn) {
                        int attempts = 0;

                        SendLine(writer, "Please login using the command login <userid>:");
                        Thread.Sleep(1000);

                        string line = reader.ReadLine();
                        string userName = line.Substring(6).Trim();

                        if (line.StartsWith("login", StringComparison.OrdinalIgnoreCase) && users.ContainsKey(userName)) {
                            SendLine(writer, "Please enter your password:");
                            string password = reader.ReadLine();

                            if (users[userName].Equals(password)) {
                                loggedIn = true;
                                SendLine(writer, "You are now logged in. Use one of the commands: message <userid> <message>, get <userid>, logout");
                            } else {
                                while (attempts < 3) {
                                    SendLine(writer, "Incorrect password, please try again:");
                                    attempts++;
                                }
                                SendLine(writer, "Too many attempts, closing connection");
                                client.Close();
                            }
                        } else {
                            SendLine(writer, "Invalid user, closing connection");
                            client.Close();
                        }

                        msg = reader.ReadLine();

                        if (msg.StartsWith("message", StringComparison.OrdinalIgnoreCase)) {
                            string[] msgParts = msg.Split(' ');
                            if (msgParts.Length < 3) {
                                SendLine(writer, "Invalid message format, please use \"message <userid> <message>\"");
                            } else {
                                clientManager.StoreMessage(msgParts[1], msgParts[2]);
                            }
                        }

                        if (msg.StartsWith("get", StringComparison.OrdinalIgnoreCase)) {
                            string[] msgParts = msg.Split(' ');
                            if (msgParts.Length < 2) {
                                SendLine(writer, "Invalid message format, please use \"get <userid>\"");
                            } else {
                                string message = clientManager.GetMessage(msgParts[1]);
                                if (message == null) {
                                    SendLine(writer, "No messages.");
                                } else {
                                    SendLine(writer, message);
                                }
                            }
                        }

                        Console.WriteLine($"[{clientName}] sent: {msg}");
                    }
                    SendLine(writer, "You are now logged out.");
                    loggedIn = false;
                    Console.WriteLine($":::{workerName} connection closed:::");
                }
            } catch (IOException e) {
                Console.WriteLine($"[{workerName}] I/O error");
                Console.WriteLine(e.Message);
            }
        }

        public void Start() {
            Thread workerThread = new Thread(new ThreadStart(Run));
            workerThread.Start();
        }

        public void SendLine (StreamWriter writer, string line) {
            Console.WriteLine($"[{workerName}] sending line: {line}");

            writer.WriteLine(line);
            writer.Flush();
        }
    }
}
