using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AccountWcf
{
    public class AccountService : IAccountService
    {
        private DAL dal;
        public bool Authenticate(AccountInfo info)
        {
            dal = new DAL();
            try
            {
                dal.OpenConnection();
                return dal.SpAuthenticateUser(info.userid, info.password);
            }
            catch (Exception ex)
            {
                throw new FaultException<AccountServiceFault>(new AccountServiceFault(ex.Message));
            }
            finally
            {
                dal.CloseConnection();
            }
        }

        public bool CreateAccount(AccountInfo info)
        {
            dal = new DAL();
            try
            {
                dal.OpenConnection();
                if (dal.SpIsUserExists(info.userid) > 0)
                {
                    return false;
                }
                dal.SpInsertUser(info);
                return true;
            }
            catch (Exception ex)
            {
                throw new FaultException<AccountServiceFault>(new AccountServiceFault(ex.Message));
            }
            finally
            {
                dal.CloseConnection();
            }
        }

    }
}
