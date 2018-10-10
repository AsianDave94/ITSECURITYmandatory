using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private StreamReader sr;
        private StreamWriter sw;

        public async Task ConnectAsync(IPAddress ip, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(ip, port);
            sr = new StreamReader(client.GetStream());
            sw = new StreamWriter(client.GetStream());
            sw.AutoFlush = true;
        }

        public async Task<List<string>> GetWordsAsync()
        {
            var command = new Command
            {
                cmd = "get words"
            };
            ;
            await sw.WriteLineAsync(JsonConvert.SerializeObject(command));
            var line = await sr.ReadLineAsync();
            if (line == null)
            {
                return new List<string>();
            }
            return JsonConvert.DeserializeObject<List<string>>(line);
        }

        public async Task SendResultAsync(IEnumerable<Result> results)
        {
            var command = new Command
            {
                cmd = "send results",
                data = JsonConvert.SerializeObject(results),
            };
            await sw.WriteLineAsync(JsonConvert.SerializeObject(command));
        }

        public void Close()
        {
            client.Close();
        }
    }
}
