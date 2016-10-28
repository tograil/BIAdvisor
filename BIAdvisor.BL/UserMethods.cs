using BIAdvisor.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BIAdvisor.BL
{

    public class UserMethods : IUserMethods
    {
        private string connectionString;

        public UserMethods()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        }

        public DataRow GetUser(string username)
        {
            string commandText = "uspdsSecurityGetUserByName";
            try
            {
                List<SqlParameter> iParam = new List<SqlParameter>();

                iParam.Add(new SqlParameter("Username", username));

                var results = DBHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, commandText, iParam);
                if (results.Tables.Count > 0 && results.Tables[0].Rows.Count > 0)
                {
                    return results.Tables[0].Rows[0];
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
    }
}
