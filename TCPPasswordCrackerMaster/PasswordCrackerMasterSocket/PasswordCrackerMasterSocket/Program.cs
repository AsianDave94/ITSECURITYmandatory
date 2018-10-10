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

        static Dictionary<string, string> hashToUser = new Dictionary<string, string>();
        static List<string> words;
        static object wordsLock = new object();

        static void Main(string[] args)
        {
            List<string> passwordFile = new List<string>(new string[] {
                "anders:5en6G6MezRroT3XKqkdPOmY/BfQ=",
                "peter:qmz4syDsnnyBP+NQeqczRv/kJP4=",
                "michael:rIFGj9xqLUA0T0J8xiGCuMlfnvM=",
                "vibeke:EQ91A67FOpjss4uW8kV570lnSa0=",
                "lars:cupd+wYwjxfNBtLY4oc9WWhVDZU=",
                "poul:94roVc1d8UZEtbK9LBF3vuo0wkg=",
                "susanne:qVs4ZslBdqp0Xp2jcyt4RIpP5+8=",
                "per:AXPaVO/3DmqNsW2uPJw9ZJxf9lc=",
                "ebbe:vE8YzmcA85cX1cTVa89XN1TwPhw=",
                "steen:8Ssn+7nvQr6yRLdKLLHBJIX5ck0=",
                "mohammed:e5E3g74Ju2HGGnhzMCZ55DmzFgc=",
                "mogens:O6BE8Nyx/TWEI6VuCyHsI71zxV0=",
            });

            foreach (var line in passwordFile)
            {
                var parts = line.Split(':');
                var username = parts[0];
                var hash = parts[1];
                hashToUser[hash] = username;
            }

            words = new List<string>();

            using (FileStream fs = new FileStream("C:/webster-dictionary.txt", FileMode.Open, FileAccess.Read))
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

                while (true)
                {
                    var line = await reader.ReadLineAsync();
                    var cmd = JsonConvert.DeserializeObject<Command>(line);

                    if (cmd.cmd == "get words")
                    {
                        var response = JsonConvert.SerializeObject(GetWords());
                        await writer.WriteLineAsync(response);
                        await writer.FlushAsync();
                    }
                    else if (cmd.cmd == "send results")
                    {
                        var results = JsonConvert.DeserializeObject<List<Result>>(cmd.data);
                        foreach (var result in results)
                        {
                            var ok = hashToUser.TryGetValue(result.Hash, out string username);
                            if (ok)
                            {
                                Console.WriteLine($"Match found: username={username}, password={result.Password}, hash={result.Hash}");
                            }
                        }
                    }
                }
            }
        }

        static public IEnumerable<string> GetWords()
        {
            lock (wordsLock)
            {
                var hundred = words.Take(500);
                words = words.Skip(500).ToList();
                return hundred;
            }
        }

    }
}
