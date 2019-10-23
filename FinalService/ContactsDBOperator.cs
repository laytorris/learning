﻿using System;
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
                Logger.Log.Info($"New contact inserted, {newcontact.ToString()}");
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
                Logger.Log.Info($"Contact updated, {newcontact.ToString()}");
                connection.Close();
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
                Logger.Log.Info($"Contact deleted, ID = {id}");
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
                    Logger.Log.Info($"SQLCommand failed, {ex.Message}");
                    throw new FaultException<string>(ex.Message, "Ошибка в процессе выполнения запроса к БД");
                }
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

        internal List<Contact> GetByNameParts(string [] parameters, bool allowPartialMatch)
        {
            string searchExpression = (allowPartialMatch) ?
                "SELECT * from dbo.Contact WHERE Name LIKE @nameTemplate OR Surname LIKE @surnameTemplate" :
                "SELECT * from dbo.Contact WHERE Name LIKE @nameTemplate AND Surname LIKE @surnameTemplate";
            SqlCommand command = new SqlCommand(searchExpression);
            command.Parameters.AddWithValue("surnameTemplate", "%" + parameters[0] + "%");
            command.Parameters.AddWithValue("nameTemplate", "%" + parameters[1] + "%");
            return GetList(command);
        }

        internal List<Contact> GetByNameParts(string param)
        {
            string searchExpression = "SELECT * from dbo.Contact WHERE Name LIKE @nameTemplate or Surname LIKE @surnameTemplate";
            SqlCommand command = new SqlCommand(searchExpression);
            command.Parameters.AddWithValue("surnameTemplate", "%"+param+"%");
            command.Parameters.AddWithValue("nameTemplate", "%" + param + "%");
            return GetList(command);
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

        private List<Contact> GetList(SqlCommand command)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                command.Connection = connection;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        ContactsDataConverter converter = new ContactsDataConverter();
                        List<Contact> result = new List<Contact>();
                        while (reader.Read())
                        {
                            Contact contact = (converter.ContactFromDataReader(reader));
                            result.Add(contact);
                            Logger.Log.Info($" New result row added, {contact.ToString()}");
                        }
                        connection.Close();
                        return result;
                    }
                    else
                    {
                        Logger.Log.Info($" No results");
                        connection.Close();
                        return null;
                    }
                };


            }
        }
        internal List<Contact> GetAll()
        {
            string searchExpression = "SELECT * from dbo.Contact";
            SqlCommand command = new SqlCommand(searchExpression);
            return GetList(command);
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
                Logger.Log.Info($"SQLCommand failed, {ex.Message}");
                throw new FaultException<string>(ex.Message, "Ошибка в процессе выполнения запроса к БД");
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
                Logger.Log.Info($"Connection to db {connection.ConnectionString} failed, {ex.Message}");
                throw new FaultException<string>(ex.Message, "Ошибка подключения к базе данных");
            }
            
        }

    }

}