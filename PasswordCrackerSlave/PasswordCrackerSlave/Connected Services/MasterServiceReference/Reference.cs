﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PasswordCrackerSlave.MasterServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MasterServiceReference.IPasswordCrackerMasterService")]
    public interface IPasswordCrackerMasterService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPasswordCrackerMasterService/Foo", ReplyAction="http://tempuri.org/IPasswordCrackerMasterService/FooResponse")]
        string Foo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPasswordCrackerMasterService/Foo", ReplyAction="http://tempuri.org/IPasswordCrackerMasterService/FooResponse")]
        System.Threading.Tasks.Task<string> FooAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPasswordCrackerMasterServiceChannel : PasswordCrackerSlave.MasterServiceReference.IPasswordCrackerMasterService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PasswordCrackerMasterServiceClient : System.ServiceModel.ClientBase<PasswordCrackerSlave.MasterServiceReference.IPasswordCrackerMasterService>, PasswordCrackerSlave.MasterServiceReference.IPasswordCrackerMasterService {
        
        public PasswordCrackerMasterServiceClient() {
        }
        
        public PasswordCrackerMasterServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PasswordCrackerMasterServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PasswordCrackerMasterServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PasswordCrackerMasterServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Foo() {
            return base.Channel.Foo();
        }
        
        public System.Threading.Tasks.Task<string> FooAsync() {
            return base.Channel.FooAsync();
        }
    }
}