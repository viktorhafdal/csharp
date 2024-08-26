using System;

namespace Tcp_Server.Client {
    public class Program() {
        public static void Main () {
            Client client = new Client();
            client.Run();
        }
    }
}