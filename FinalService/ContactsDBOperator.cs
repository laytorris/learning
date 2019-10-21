using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;
using Homework1;

namespace FinalService
{
    public class ContactsDBOperator
    {

        private string _ConnectionString;
        public ContactsDBOperator(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public void InsertContact(Contact newcontact)
        {
            string insertSQLExpression = "INSERT INTO dbo.Contact (Name, Surname, MiddleName, " +
                "Gender, BirthDate, Phone, TaxNumber, Position, OrganizationID)" +
                " VALUES (@name, @surname, @middlename, @gender, @birthdate," +
                " @phone, @taxnumber, @position, @organizationid)";

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand insertCommand = FillCommandParameters(insertSQLExpression, connection, newcontact);
                OpenConnection(connection);
                DoSQLCommand(insertCommand);
                connection.Close();
            }

        }
        public void UpdateContact(Contact newcontact)
        {
            string insertSQLExpression = "UPDATE dbo.Contact SET " +
                "Name = @name, Surname = @surname, MiddleName = @middlename, " +
                "Gender = @gender, BirthDate = @birthdate, Phone = @phone, " +
                "TaxNumber = @taxnumber, Position = @position, OrganizationID = @organizationid" +
                " WHERE ID = @id";

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand insertCommand = FillCommandParameters(insertSQLExpression, connection, newcontact);
                insertCommand.Parameters.Add(new SqlParameter("@id", newcontact.ID));
                OpenConnection(connection);
                DoSQLCommand(insertCommand);
                connection.Close();
            }

        }

        internal bool RowExists(string id)
        {
            string searchExpression = "SELECT ID FROM dbo.Contact WHERE ID = @id";

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand searchCommand = new SqlCommand(searchExpression, connection);
                searchCommand.Parameters.AddWithValue("@id", id);
                OpenConnection(connection);
                try
                {
                    SqlDataReader reader = searchCommand.ExecuteReader();
                    bool result = (reader.HasRows) ? true : false;
                    connection.Close();
                    return result;
                }
                catch (SqlException ex)
                {
                    connection.Close();
                    throw new FaultException<string>(ex.Message, "Ошибка в процессе выполнения запроса к БД");
                }
            }

        }

        internal void DeleteContact(string id)
        {
            string deleteSqlExpression = "DELETE FROM dbo.Contact WHERE ID = @id";

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand deleteCommand = new SqlCommand(deleteSqlExpression, connection);
                deleteCommand.Parameters.Add(new SqlParameter("@Id", id));
                OpenConnection(connection);
                DoSQLCommand(deleteCommand);
                connection.Close();
            }
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

        internal Contact GetById(string id)
        {
            
            string searchExpression = "SELECT * from dbo.Contact WHERE ID = @id";
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
                        Contact contact = new Contact();
                        while (reader.Read())
                        {
                            contact = (converter.ContactFromDataReader(reader));
                        }
                        connection.Close();
                        return contact;
                    }
                    else
                    {
                        connection.Close();
                        return null;
                    }
                };
            }
            
        }

        internal List<Contact> GetAll()
        {
            string searchExpression = "SELECT * from dbo.Contact";
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand searchCommand = new SqlCommand(searchExpression, connection);
                connection.Open();
                using (SqlDataReader reader = searchCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ContactsDataConverter converter = new ContactsDataConverter();
                        List<Contact> result = new List<Contact>();
                        while (reader.Read())
                        {
                            result.Add(converter.ContactFromDataReader(reader));
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


        }

        internal List<Organization> GetOrgList()
        {
            string searchExpression = "SELECT * from dbo.Organization";
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                SqlCommand searchCommand = new SqlCommand(searchExpression, connection);
                
                OpenConnection(connection); 
                
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
        }




        //public SqlDataReader ContactsSearch(string name, string surname)
        //{
        //    string searchSqlExpression = "SELECT * from dbo.Contact WHERE Name LIKE @namep, Surname LIKE @surnamep ";

        //    using (SqlConnection connection = new SqlConnection(_ConnectionString))
        //    {
        //        SqlCommand searchCommand = new SqlCommand(searchSqlExpression, connection);
        //        searchCommand.Parameters.Add(new SqlParameter("@name", '%' + name + '%'));
        //        searchCommand.Parameters.Add(new SqlParameter("@surname", '%' + surname + '%'));
        //        return searchCommand.ExecuteReader();
        //    }
        //}




        private SqlCommand FillCommandParameters(string SQLExpression, SqlConnection connection, Contact newcontact)
        {
            SqlCommand insertCommand = new SqlCommand(SQLExpression, connection);

            insertCommand.Parameters.Add(new SqlParameter("@name", newcontact.Name));

            insertCommand.Parameters.Add(new SqlParameter("@surname", newcontact.Surname));

            insertCommand.Parameters.Add(new SqlParameter("@middlename", newcontact.MiddleName));

            insertCommand.Parameters.Add(new SqlParameter("@gender", newcontact.Gender));

            if (newcontact.BirthDate != null)
            {
                insertCommand.Parameters.Add(new SqlParameter("@birthdate", newcontact.BirthDate));
            }
            else
            {
                insertCommand.Parameters.Add(new SqlParameter("@birthdate", DBNull.Value));
            }

            insertCommand.Parameters.Add(new SqlParameter("@phone", newcontact.Phone));

            insertCommand.Parameters.Add(new SqlParameter("@taxnumber", newcontact.TaxNumber));

            insertCommand.Parameters.Add(new SqlParameter("@position", newcontact.Position));
            if (newcontact.Job != null)
            {
                insertCommand.Parameters.Add(new SqlParameter("@organizationid", newcontact.Job.ID));
            }
            else
            {
                insertCommand.Parameters.Add(new SqlParameter("@organizationid", DBNull.Value));
            }

            return insertCommand;
        }

        private void DoSQLCommand(SqlCommand command)
        {
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new FaultException<string>(ex.Message, "Ошибка в процессе выполнения запроса к БД");
            }
        }

        private void OpenConnection(SqlConnection connection)
        {
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                throw new FaultException<string>(ex.Message, "Ошибка подключения к базе данных");
            }
            
        }

    }

}