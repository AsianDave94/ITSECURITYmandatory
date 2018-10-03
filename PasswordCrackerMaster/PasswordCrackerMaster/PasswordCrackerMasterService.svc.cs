using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PasswordCrackerMaster
{
    public class PasswordCrackerMasterService : IPasswordCrackerMasterService
    {

        public List<string> GetWords()
        {
            throw new NotImplementedException();
        }

        public void SendResult(List<Result> results)
        {
            throw new NotImplementedException();
        }
    }
}
