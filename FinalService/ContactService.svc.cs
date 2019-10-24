using Homework1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace FinalService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ContactService
    {
        public ContactService()
        {
            Logger.InitLogger();
        }

        [OperationContract]
        public string InsertContact(string Name, string Surname, string MiddleName,
                string Gender, string BirthDate, string Phone, string TaxNumber,
                string Position, string JobID)
        {
            Logger.Log.Info($"InsertContact got params: name = {Name}, surname = {Surname}, " +
                $"middlename = {MiddleName},gender = { Gender}, birthday = {BirthDate}, phone = {Phone}," +
                $" taxnumber = {TaxNumber}, position = {Position}, jobid = {JobID}");
            ContactsDataConverter converter = new ContactsDataConverter();
            Contact newContact = converter.CreateInstance(Name, Surname, MiddleName,
                Gender, BirthDate, Phone, TaxNumber, Position, JobID);

            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

            try 
            {
                
                int id = dBOperator.InsertContact(newContact);
                string result = JsonConvert.SerializeObject(id);
                return result;
            }
            catch (DBConnectionException)
            {
                throw new FaultException("Ошибка подключения к базе данных");
            }
            catch(SQLCommandException)
            {
                throw new FaultException("Ошибка выполнения запроса к базе данных");
            }

        }

        [OperationContract]
        public void UpdateContact(string ID, string Name, string Surname, string MiddleName,
                string Gender, string BirthDate, string Phone, string TaxNumber,
                string Position, string JobID)
        {
            Logger.Log.Info($"UpdateContact got params: id = {ID}, name = {Name}, surname = {Surname}, " +
               $"middlename = {MiddleName},gender = { Gender}, birthday = {BirthDate}, phone = {Phone}," +
               $" taxnumber = {TaxNumber}, position = {Position}, jobid = {JobID}");

            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

            if (dBOperator.RowExists(ID))
            {
                ContactsDataConverter converter = new ContactsDataConverter();
                Contact newContact = converter.CreateInstance(Name, Surname, MiddleName,
                    Gender, BirthDate, Phone, TaxNumber, Position, JobID, ID);
                try
                {
                    dBOperator.UpdateContact(newContact);
                }
                catch (DBConnectionException)
                {
                    throw new FaultException("Ошибка подключения к базе данных");
                }
                catch (SQLCommandException)
                {
                    throw new FaultException("Ошибка выполнения запроса к базе данных");
                }
            
            }
            else
            {
                Logger.Log.Info($"Contact doesn't exist: id = {ID}");

                throw new FaultException<string>(ID, "Контакт не существует");
            }
        }

        [OperationContract]
        public void DeleteContact(string id)
        {
            Logger.Log.Info($"DeleteContact got params: id = {id}");
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

            if (dBOperator.RowExists(id))
            {
                try 
                { 
                    dBOperator.DeleteContact(id); 
                }
                catch (DBConnectionException)
                {
                    throw new FaultException("Ошибка подключения к базе данных");
                }
                catch (SQLCommandException)
                {
                    throw new FaultException("Ошибка выполнения запроса к базе данных");
                }
                
            }
            else
            {
                Logger.Log.Info($"Contact doesn't exist: id = {id}");
                throw new FaultException<string>(id, "Контакт не существует");
            }
        }

        [OperationContract]
        public string GetContact(string id)
        {
            Logger.Log.Info($"GetContact got params: id = {id}");
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);
            try
            {
                if (dBOperator.RowExists(id))
                {
                    Contact contact = dBOperator.GetById(id);

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.DateFormatString = "yyyy-MM-dd";
                    string responce = JsonConvert.SerializeObject(contact, settings);
                    return responce;
                }
                else
                {
                    Logger.Log.Info($"Contact doesn't exist: id = {id}");
                    throw new FaultException<string>(id, "Контакт не существует");
                }
            }
            catch(DBConnectionException)
            {
                throw new FaultException("Ошибка подключения к базе данных");
            }
            catch (SQLCommandException)
            {
                throw new FaultException("Ошибка выполнения запроса к базе данных");
            }
           
        }

        [OperationContract]
        public string GetAllContacts()
        {
            Logger.Log.Info($"GetAllContacts called");
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);
            List<Contact> contacts = dBOperator.GetAll();
            if (contacts == null)
            {
                Logger.Log.Info($"Contacts list is empty");
                throw new FaultException<int>(0, $"Список контактов пуст");
            }
            else {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.DateFormatString = "d MMMM, yyyy";
                string responce = JsonConvert.SerializeObject(contacts, settings);
                Logger.Log.Info($"GetAllContacts return {contacts?.Count} contacts");
                return responce;
            }
            
        }

        [OperationContract]
        public string GetContactsByParts(string searchString)
        {
            Logger.Log.Info($"GetContactsByParts got searching params: id = {searchString}");
            searchString.Trim(',', ' ');

            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

            List<Contact> contacts;

            string[] parameters;
            if (searchString.Contains(" "))
            {
                //имя и фамилия должны частично соответствовать поиску
                Logger.Log.Info($"Search by two params, search condition AND");
                parameters = searchString.Split(' ');
               contacts = dBOperator.GetByNameParts(parameters, false);
            }
            else if (searchString.Contains(","))
            {
                //имя ИЛИ фамилия должны частично соответствовать поиску
                Logger.Log.Info($"Search by two params, search condition OR");
                parameters = searchString.Split(',');
                contacts = dBOperator.GetByNameParts(parameters, true);
            }
            else
            {
                //поиск по одной строке
                Logger.Log.Info($"Search by one param");
                contacts = dBOperator.GetByNameParts(searchString);
             }
            if (contacts == null)
            {
                Logger.Log.Info($"No search results. Return exception");
                throw new FaultException<int>(0, $"Подходящие контакты отсутствуют. Уточните критерии поиска");
            }
            int MaxSearchResultCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxSearchResultCount"]);
            if (contacts?.Count> MaxSearchResultCount)
            {
                Logger.Log.Info($"Results count = {contacts.Count} more than {MaxSearchResultCount}. Return exception");
                throw new FaultException<int>(contacts.Count, $"Число результатов поиска превышает {MaxSearchResultCount}. Уточните критерии поиска"); 
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "d MMMM, yyyy";
            string responce = JsonConvert.SerializeObject(contacts, settings);
            Logger.Log.Info($"GetContactsByParts return {contacts?.Count} contacts");
            return responce;
        }

        [OperationContract]
        public string GetOrganizationList()
        {
            Logger.Log.Info($"GetOrganizationList got request");
            string connectionString = ConfigurationManager.AppSettings["DBConnectionString"];
            OrganizationDBOperator organizationDBOperator = new OrganizationDBOperator(connectionString);

            List<Organization> organizations = organizationDBOperator.GetOrgList();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "d MMMM, yyyy";
            string responce = JsonConvert.SerializeObject(organizations, settings);
            Logger.Log.Info($"GetOrganizationList return {organizations?.Count} organizations");
            return responce;
        }
    }
}
