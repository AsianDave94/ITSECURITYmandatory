using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerSlave
{
    class Program
    {
        static Dictionary<string, string> hashToUser = new Dictionary<string, string>();
        static readonly HashAlgorithm _messageDigest = new SHA1CryptoServiceProvider();
        static async Task Main(string[] args)
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

            var client = new SocketClient();
            var ip = IPAddress.Parse("127.0.0.1");
            try
            {
                await client.ConnectAsync(ip, 6789);
                while (true)
                {
                    Console.WriteLine("Getting words.");
                    var words = await client.GetWordsAsync();
                    Console.WriteLine("Got {0} words", words.Count());
                    if (words.Count() == 0)
                    {
                        break;
                    }

                    foreach (var word in words)
                    {
                        Console.WriteLine(word);
                        IEnumerable<Result> results = CheckWordWithVariations(word);
                        results = results.Where(result => hashToUser.TryGetValue(result.Hash, out string username));
                        Console.WriteLine("Sending results");
                        await client.SendResultAsync(results);
                        Console.WriteLine("Sent results");
                    }
                }
            }
            finally
            {
                client.Close();
            }
        }

        static IEnumerable<Result> CheckWordWithVariations(String dictionaryEntry)
        {
            List<Result> result = new List<Result>(); //might be empty

            String possiblePassword = dictionaryEntry;
            Result partialResult = CheckSingleWord(possiblePassword);
            result.Add(partialResult);

            String possiblePasswordUpperCase = dictionaryEntry.ToUpper();
            Result partialResultUpperCase = CheckSingleWord(possiblePasswordUpperCase);
            result.Add(partialResultUpperCase);

            String possiblePasswordCapitalized = StringUtilities.Capitalize(dictionaryEntry);
            Result partialResultCapitalized = CheckSingleWord(possiblePasswordCapitalized);
            result.Add(partialResultCapitalized);

            String possiblePasswordReverse = StringUtilities.Reverse(dictionaryEntry);
            Result partialResultReverse = CheckSingleWord(possiblePasswordReverse);
            result.Add(partialResultReverse);

            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordEndDigit = dictionaryEntry + i;
                Result partialResultEndDigit = CheckSingleWord(possiblePasswordEndDigit);
                result.Add(partialResultEndDigit);
            }

            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordStartDigit = i + dictionaryEntry;
                Result partialResultStartDigit = CheckSingleWord(possiblePasswordStartDigit);
                result.Add(partialResultStartDigit);
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    String possiblePasswordStartEndDigit = i + dictionaryEntry + j;
                    Result partialResultStartEndDigit = CheckSingleWord(possiblePasswordStartEndDigit);
                    result.Add(partialResultStartEndDigit);
                }
            }

            return result;
        }
        static Result CheckSingleWord(String possiblePassword)
        {
            Result result = new Result();
            result.Password = possiblePassword;

            char[] charArray = possiblePassword.ToCharArray();
            byte[] passwordAsBytes = Array.ConvertAll(charArray, ch => Convert.ToByte(ch));

            byte[] encryptedPassword = _messageDigest.ComputeHash(passwordAsBytes);
            string encryptedPasswordBase64 = System.Convert.ToBase64String(encryptedPassword);
            result.Hash = encryptedPasswordBase64;
            return result;
        }
    }
}
