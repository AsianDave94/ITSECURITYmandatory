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
        public string Foo()
        {
            return "test service";
        }
    }
}
