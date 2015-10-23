using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AccountWcf
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        [FaultContract(typeof(AccountServiceFault))]
        bool CreateAccount(AccountInfo info);

        [OperationContract]
        [FaultContract(typeof(AccountServiceFault))]
        bool Authenticate(AccountInfo info);
    }

    [DataContract]
    public class AccountInfo
    {
        [DataMember]
        public string userid;
        [DataMember]
        public string password;
        [DataMember]
        public string firstname;
        [DataMember]
        public string lastname;
    }

    [DataContract]
    public class AccountServiceFault
    {
        private string _message;

        public AccountServiceFault(string message)
        {
            _message = message;
        }

        [DataMember]
        public string Message { get { return _message; } set { _message = value; } }
    }
}
