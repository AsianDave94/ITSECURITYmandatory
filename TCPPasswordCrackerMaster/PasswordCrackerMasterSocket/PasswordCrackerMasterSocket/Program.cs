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
        static void Main(string[] args)
        {


            IPAddress ip = IPAddress.Parse("0.0.0.0");
            TcpListener CS = new TcpListener(ip,6789);


            CS.Start();
            var client = CS.AcceptTcpClient();

            var stream = client.GetStream();
            var reader = new StreamReader(stream);

            var line = reader.ReadLine();

            var cmd = JsonConvert.DeserializeObject<Command>(line);

           
            string SendWords()
            {


                return "puha";
            }

            string GetWords()
            {
                return "puha";
            }
        
        }
    }
}
