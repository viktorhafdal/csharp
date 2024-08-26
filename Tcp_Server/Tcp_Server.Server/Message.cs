using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tcp_Server.Server {
    internal class Message {
        private string to;
        private string message;

        public Message (string to, string message) {
            this.to = to;
            this.message = message;
        }

        public bool IsTo(string userName) {
           return to.Equals(userName);
        }

        public string GetMessage() {
            return message;
        }
    }
}
