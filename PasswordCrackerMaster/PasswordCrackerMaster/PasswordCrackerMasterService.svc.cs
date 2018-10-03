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

        public IEnumerable<string> GetWords()
        {
            List<string> words = new List<string>();

            using (FileStream fs = new FileStream("C:/webster-dictionary-reduced.txt", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader dictionary = new StreamReader(fs))
                {
                    while(!dictionary.EndOfStream)
                    {
                        string entry = dictionary.ReadLine();

                        words.Add(entry);
                    }
                }
            }

            return words.Take(100);
        }

        public void SendResult(List<Result> results)
        {
            throw new NotImplementedException();
        }
    }
}
