using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordCrackerSlave.MasterServiceReference;

namespace PasswordCrackerSlave
{
    class Program
    {
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
    }
}
