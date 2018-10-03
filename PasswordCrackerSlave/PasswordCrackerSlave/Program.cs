using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PasswordCrackerSlave.MasterServiceReference;

namespace PasswordCrackerSlave
{
    class Program
    {
        private readonly HashAlgorithm _messageDigest = new SHA1CryptoServiceProvider();
        static void Main(string[] args)
        {
            var endpointName = "BasicHttpBinding_IPasswordCrackerMasterService";
            var endpointAddress = args.FirstOrDefault();
            if (string.IsNullOrEmpty(endpointAddress))
            {
                Console.WriteLine("Must provide endpoint address command line argument");
                return;
            }
            var client = new PasswordCrackerMasterServiceClient(endpointName, endpointAddress);
            Console.WriteLine($"Response from master: {client.Foo()}");
        }
        private IEnumerable<Result> CheckWordWithVariations(String dictionaryEntry)
        {
            List<Result> result = new List<Result>(); //might be empty

            String possiblePassword = dictionaryEntry;
            IEnumerable<Result> partialResult = CheckSingleWord(userInfos, possiblePassword);
            result.AddRange(partialResult);

            String possiblePasswordUpperCase = dictionaryEntry.ToUpper();
            IEnumerable<UserInfoClearText> partialResultUpperCase = CheckSingleWord(userInfos, possiblePasswordUpperCase);
            result.AddRange(partialResultUpperCase);

            String possiblePasswordCapitalized = StringUtilities.Capitalize(dictionaryEntry);
            IEnumerable<UserInfoClearText> partialResultCapitalized = CheckSingleWord(userInfos, possiblePasswordCapitalized);
            result.AddRange(partialResultCapitalized);

            String possiblePasswordReverse = StringUtilities.Reverse(dictionaryEntry);
            IEnumerable<UserInfoClearText> partialResultReverse = CheckSingleWord(userInfos, possiblePasswordReverse);
            result.AddRange(partialResultReverse);

            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordEndDigit = dictionaryEntry + i;
                IEnumerable<UserInfoClearText> partialResultEndDigit = CheckSingleWord(userInfos, possiblePasswordEndDigit);
                result.AddRange(partialResultEndDigit);
            }

            for (int i = 0; i < 100; i++)
            {
                String possiblePasswordStartDigit = i + dictionaryEntry;
                IEnumerable<UserInfoClearText> partialResultStartDigit = CheckSingleWord(userInfos, possiblePasswordStartDigit);
                result.AddRange(partialResultStartDigit);
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    String possiblePasswordStartEndDigit = i + dictionaryEntry + j;
                    IEnumerable<UserInfoClearText> partialResultStartEndDigit = CheckSingleWord(userInfos, possiblePasswordStartEndDigit);
                    result.AddRange(partialResultStartEndDigit);
                }
            }

            return result;
        }
        private Result CheckSingleWord(String possiblePassword)
        {
            Result result = new Result();
            result.Password = possiblePassword;

            char[] charArray = possiblePassword.ToCharArray();
            byte[] passwordAsBytes = Array.ConvertAll(charArray, PasswordFileHandler.GetConverter());

            byte[] encryptedPassword = _messageDigest.ComputeHash(passwordAsBytes);
            string encryptedPasswordBase64 = System.Convert.ToBase64String(encryptedPassword);
            result.Hash = encryptedPasswordBase64;
            return result;
        }
    }
}
