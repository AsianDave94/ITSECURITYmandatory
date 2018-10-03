using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PasswordCrackerMaster
{
    public class PasswordCrackerMasterService : IPasswordCrackerMasterService
    {
        public static readonly List<string> passwordFile = new List<string>(new string[] {
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

        public IEnumerable<string> GetWords()
        {
            List<string> words = new List<string>();

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

            return words.Take(100);
        }



        public static List<Tuple<string, string>> usernamesAndPassword = new List<Tuple<string, string>>();

        public void SendResult(List<Result> results)
        {
            var hashToPassword = new Dictionary<string, string>();
            foreach (var result in results)
            {
                hashToPassword[result.Hash] = result.Password;
            }

            var lines = passwordFile;
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                var username = parts[0];
                var passwordHash = parts[1];
                string password = null;
                try
                {
                    password = hashToPassword[passwordHash];
                }
                catch
                {
                }
                if (password != null)
                {
                    usernamesAndPassword.Add(new Tuple<string, string>(username, password));
                }
            }

        }

        public IEnumerable<string> GetMatches()
        {
            return usernamesAndPassword.Select(x => $"{x.Item1},{x.Item2}");
        }
    }
}
