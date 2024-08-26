using System;

namespace Tcp_Server.Server {
    public class Program {
        public static void Main() {
            ClientManager clientManager = new ClientManager(6010);
            clientManager.PopulateUsers();
            clientManager.Run();
        }
    }
}