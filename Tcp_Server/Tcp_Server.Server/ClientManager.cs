using System.Net.Sockets;
using System.Net;

namespace Tcp_Server.Server {
    public class ClientManager {
        private List<ClientWorker> workers;
        private Dictionary<string, string> users;
        private List<Message> messages;
        private int port;
        private bool stop;

        public ClientManager(int port) {
            this.port = port;
            this.stop = false;

            workers = new List<ClientWorker>();
            users = new Dictionary<string, string>();
            messages = new List<Message>();
        }

        public void PopulateUsers() {
            users.Add("client1", "client1");
            users.Add("client2", "client2");
            users.Add("client3", "client3");
        }

        public void Run() {
            TcpListener server = new TcpListener(IPAddress.Any, port);
            TcpClient client;
            int clientCounter = 0;
            int workerCounter = 0;

            try {
                server.Start(100);
                Console.WriteLine(":::ClientHandler server online:::");

                while (!stop) {
                    try {
                        if (server.Pending()) {
                            client = server.AcceptTcpClient();
                            string clientName = $"Client{clientCounter}";
                            string workerName = $"Worker{workerCounter}";

                            ClientWorker worker = new ClientWorker(this, client, clientName, workerName, users);
                            workers.Add(worker);
                            worker.Start();

                            clientCounter++;
                            workerCounter++;
                        } else {
                            Thread.Sleep(100);
                        }
                    } catch (SocketException e) {
                        Console.WriteLine($"[ClientHandler] Socket exception: {e.Message}");
                    }
                }

                server.Stop();
                Console.WriteLine(":::Server offline:::");
            } catch (IOException e) {
                Console.WriteLine(e.Message);
            } finally {
                server?.Stop();
            }
        }

        public void shutdown() {
            stop = true;
        }

        public void StoreMessage(string to, string message) {
            messages.Add(new Message(to, message));
        }

        public string GetMessage(string userName) {
            foreach (Message message in messages) {
                if (message.IsTo(userName)) {
                    messages.Remove(message);
                    return message.GetMessage();
                }
            }
            return null;
        }
    }


}
