using System.Collections.Generic;
using System.Data.SqlClient;
using Homework1;

namespace FinalService
{
    public class OrganizationDBOperator
    {

        private string _ConnectionString;
        public OrganizationDBOperator(string connectionString)
        {
            _ConnectionString = connectionString;
        }   

        internal Organization GetOrganizationByID(int id)
        {

            string searchExpression = "SELECT * from dbo.Organization WHERE ID = @id";
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand searchCommand = new SqlCommand(searchExpression, connection);
                searchCommand.Parameters.Add(new SqlParameter("@id", id));
                connection.Open();
                
                using (SqlDataReader reader = searchCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ContactsDataConverter converter = new ContactsDataConverter();
                        Organization organization = new Organization();
                        while (reader.Read())
                        {
                            organization = (converter.OrganizationFromDataReader(reader));
                        }
                        connection.Close();
                        return organization;
                    }
                    else
                    {
                        connection.Close();
                        return null;
                    }
                };
            }

        }


     
        internal List<Organization> GetOrgList()
        {
            string searchExpression = "SELECT * from dbo.Organization";
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand searchCommand = new SqlCommand(searchExpression, connection);
                
                OpenConnection(connection);
                try
                {
                    using (SqlDataReader reader = searchCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            ContactsDataConverter converter = new ContactsDataConverter();
                            List<Organization> result = new List<Organization>();
                            while (reader.Read())
                            {
                                result.Add(converter.OrganizationFromDataReader(reader));
                            }
                            connection.Close();
                            return result;
                        }
                        else
                        {
                            connection.Close();
                            return null;
                        }
                    };
                }
                catch(SqlException ex)
                {
                    Logger.Log.Info($"SQLCommand failed, {ex.ToString()}");
                    throw new SQLCommandException(ex);
                }
                

            }
        }

        private void OpenConnection(SqlConnection connection)
        {
            try
            {
                connection.Open();
                Logger.Log.Info($"Connection to db {connection.ConnectionString} established");
            }
            catch (SqlException ex)
            {
                Logger.Log.Info($"Connection to db {connection.ConnectionString} failed, {ex.ToString()}");
                throw new DBConnectionException(ex);
            }

        }

    }

}