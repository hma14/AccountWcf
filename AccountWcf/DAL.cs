using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AccountWcf
{
    public class DAL
    {
        private SqlConnection sqlConnection;

        private string connectionString()
        {
            return ConfigurationManager.ConnectionStrings["AccountDB"].ConnectionString;
        }


        public DAL()
        {
            try
            {
                if (sqlConnection == null)
                {
                    sqlConnection = new SqlConnection(connectionString());
                    if (sqlConnection.State == System.Data.ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public SqlConnection OpenConnection()
        {
            try
            {
                if (sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
            }
            catch
            {
                throw;
            }
            return sqlConnection;
        }

        public void CloseConnection()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }


        public void SpInsertUser(AccountInfo info)
        {
            string encrptedPassword = EncryptDecrypt.Encrypt(info.password);
            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertUser", sqlConnection))
                {                   
                    cmd.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["ConnectionTimeOut"]);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", info.userid);
                    cmd.Parameters.AddWithValue("@password", encrptedPassword);
                    cmd.Parameters.AddWithValue("@firstName", info.firstname);
                    cmd.Parameters.AddWithValue("@lastName", info.lastname);
                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
        }

        public bool SpAuthenticateUser(string userid, string passwd)
        {
            if (SpIsUserExists(userid) > 0)
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("RetrievePasswordById", sqlConnection))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["ConnectionTimeOut"]);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userId", userid);
                        if (cmd.Connection.State == ConnectionState.Closed)
                        {
                            cmd.Connection.Open();
                        }                       
                        string userPassword = EncryptDecrypt.Decrypt(cmd.ExecuteScalar().ToString());
                        return userPassword == passwd;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return false;
        }

        public int SpIsUserExists(string userid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("IsUserExists", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", userid);
                    
                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        cmd.Connection.Open();
                    }
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
