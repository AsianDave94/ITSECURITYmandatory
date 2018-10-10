using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PasswordCrackerSlave.MasterServiceReference;

namespace PasswordCrackerSlave
{
    class Program
    {
        static readonly HashAlgorithm _messageDigest = new SHA1CryptoServiceProvider();
        static async Task Main(string[] args)
        {
            var client = new SocketClient();
            var ip = IPAddress.Parse("127.0.0.1");
            await client.ConnectAsync(ip, 6789);
            
            while (true)
            {
                Console.WriteLine("Getting words.");
                var words = await client.GetWordsAsync();
                Console.WriteLine("Got words");
                foreach (var word in words)
                {

                    Console.WriteLine(word);
                    //IEnumerable<Result> results = CheckWordWithVariations(word);
                    //foreach (var result in results)
                    //{
                    //    Console.WriteLine(result.Password);
                    //    Console.WriteLine(result.Hash);
                    //}
                    //client.SendResultAsync(results.ToArray());
                }
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
