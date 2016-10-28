using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BIAdvisor.DAL
{
	public class DBHelper
	{
		#region ExecuteSP

		public static DataSet ExecuteDataset(string connString, CommandType commandType, string commandText)
		{
			return PerformDbOperation(connString, commandType, commandText, new List<SqlParameter>());
		}

		public static DataSet ExecuteDataset(string connString, CommandType commandType, string commandText, List<SqlParameter> commandParameters)
		{
			return PerformDbOperation(connString, commandType, commandText, commandParameters);
		}

		private static DataSet PerformDbOperation(string connString, CommandType commandType, string commandText, List<SqlParameter> commandParameters)
		{
			using (var connection = new SqlConnection(connString))
			{
				//create a command and prepare it for execution
				SqlCommand cmd = new SqlCommand();
				foreach (SqlParameter p in commandParameters)
				{
					//check for derived output value with no value assigned
					if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
					{
						p.Value = DBNull.Value;
					}

					cmd.Parameters.Add(p);
				}

				connection.Open();

				cmd.Connection = connection;
				cmd.CommandType = commandType;
				cmd.CommandText = commandText;

				//create the DataAdapter & DataSet
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataSet ds = new DataSet();

				//fill the DataSet using default values for DataTable names, etc.
				da.Fill(ds);
				connection.Close();

				return ds;
			}
		}


        public static DataTable GetSchemaTable(string connString, CommandType commandType, string commandText, List<SqlParameter> commandParameters)
        {
            using (var connection = new SqlConnection(connString))
            {
                //create a command and prepare it for execution
                SqlCommand cmd = new SqlCommand();
                DataTable schemaTable;
                SqlDataReader myReader;

                foreach (SqlParameter p in commandParameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(p);
                }

                connection.Open();

                cmd.Connection = connection;
                cmd.CommandType = commandType;
                cmd.CommandText = commandText;

                myReader = cmd.ExecuteReader(CommandBehavior.KeyInfo);

                //Retrieve column schema into a DataTable.
                schemaTable = myReader.GetSchemaTable();

                //Always close the DataReader and connection.
                myReader.Close();
                connection.Close();

                return schemaTable;
            }
        }
        #endregion ExecuteSP

    }
}
