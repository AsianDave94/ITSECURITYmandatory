using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerSlave
{
    class SocketClient
    {
        private TcpClient client;

        public async Task ConnectAsync(IPAddress ip, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(ip, port);



        }

        public async Task<List<string>> GetWordsAsync()
        {
           
            
        }

        public async Task SendResultAsync(List<string> results)
        {


        }

        public void Close()
        {
            client.Close();
        }
    }
}
