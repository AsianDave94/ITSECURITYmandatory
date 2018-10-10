using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace PasswordCrackerMasterSocket
{
    class Program
    {

        static List<string> words;

        static void Main(string[] args)
        {

            words = new List<string>();

            using (FileStream fs = new FileStream("C:/webster-dictionary-reduced.txt", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader dictionary = new StreamReader(fs))
                {
                    while (!dictionary.EndOfStream)
                    {
                        string entry = dictionary.ReadLine();

                        words.Add(entry);
                    }
                }
            }



            IPAddress ip = IPAddress.Parse("0.0.0.0");
            TcpListener CS = new TcpListener(ip, 6789);

            CS.Start();

            while (true)
            {
                _ = HandleClientAsync(CS);
            }
        }

        private static async Task HandleClientAsync(TcpListener CS)
        {
            using (var client = await CS.AcceptTcpClientAsync())
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);

                var line = await reader.ReadLineAsync();
                var cmd = JsonConvert.DeserializeObject<Command>(line);

                if (cmd.cmd == "get words")
                {
                    var response = JsonConvert.SerializeObject(GetWords());
                    await writer.WriteLineAsync(response);
                    await writer.FlushAsync();
                }
            }
        }

        static public IEnumerable<string> GetWords()
        {
            var hundred = words.Take(100);
            words = words.Skip(100).ToList();
            return hundred;
        }

    }
}
