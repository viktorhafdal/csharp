using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace Tcp_Server.Client {
    public class Client {
        private bool firstMessage = true;
        public Client() {
           Run();
        }

        public void Run() {
            try {
                //Java: Socket connection = new Socket("127.0.0.1", 6010; | Connecting to server on port 6010
                TcpClient client = new TcpClient("127.0.0.1", 6010);
                //Console.WriteLine(":::Connection to server opened:::");

                StreamWriter writer = new StreamWriter(client.GetStream());
                StreamReader reader = new StreamReader(client.GetStream());
                string msg = string.Empty;
                string serverMsg = string.Empty;

                while (!msg.Equals("logout", StringComparison.OrdinalIgnoreCase)) {
                    serverMsg = reader.ReadLine();
                    Console.WriteLine(serverMsg);

                    //while (reader.Peek() > 0) {
                    //    Console.WriteLine(reader.Peek());
                    //    serverMsg = reader.ReadLine();
                    //   Console.WriteLine(serverMsg);
                    //   Console.WriteLine(reader.Peek());
                    //}

                    msg = Console.ReadLine();
                    SendLine(writer, msg);
                }

                Console.WriteLine("::Connection to server closed::");
                client.Close();
            } catch (IOException e) {
                Console.WriteLine("[Client] I/O error");
                Console.WriteLine(e.Message);
            }
        }

        public string SendLine(StreamWriter writer, params string[] msg) {

            string line = string.Join(" ", msg);
            writer.WriteLine(line);
            writer.Flush();

            return line;
        }
    }
}