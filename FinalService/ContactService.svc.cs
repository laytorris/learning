using Homework1;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Script.Serialization;

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
        public void InsertContact(string Name, string Surname, string MiddleName,
                string Gender, string BirthDate, string Phone, string TaxNumber,
                string Position, string JobID)
        {
            Logger.Log.Info($"InsertContact got params: name = {Name}, surname = {Surname}, " +
                $"middlename = {MiddleName},gender = { Gender}, birthday = {BirthDate}, phone = {Phone}," +
                $" taxnumber = {TaxNumber}, position = {Position}, jobid = {JobID}");
            ContactsDataConverter preparer = new ContactsDataConverter();
            Contact newContact = preparer.CreateInstance(Name, Surname, MiddleName,
                Gender, BirthDate, Phone, TaxNumber, Position, JobID);

            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);
            dBOperator.InsertContact(newContact);
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
                ContactsDataConverter preparer = new ContactsDataConverter();
                Contact newContact = preparer.CreateInstance(Name, Surname, MiddleName,
                    Gender, BirthDate, Phone, TaxNumber, Position, JobID, ID);
                dBOperator.UpdateContact(newContact);
            }
            else
            {
                throw new FaultException<string>(ID, "Контакт не существует");
            }
        }

        [OperationContract]
        public void DeleteContact(string id)
        {
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

            if (dBOperator.RowExists(id))
            {
                dBOperator.DeleteContact(id);
            }
            else
            {
                throw new FaultException<string>(id, "Контакт не существует");
            }
        }

        [OperationContract]
        public string GetContact(string id)
        {
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

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
                throw new FaultException<string>(id, "Контакт не существует");
            }
        }

        [OperationContract]
        public string GetAllContacts()
        {
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);
            List<Contact> contacts = dBOperator.GetAll();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "d MMMM, yyyy";
            string responce = JsonConvert.SerializeObject(contacts, settings) ;
            return responce;
        }

        [OperationContract]
        public string GetContactsByParts(string searchString)
        {
            searchString.Trim(',', ' ');

            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

            List<Contact> contacts;

            string[] parameters;
            if (searchString.Contains(" "))
            {
                //имя и фамилия должны частично соответствовать поиску
               parameters = searchString.Split(' ');
               contacts = dBOperator.GetByNameParts(parameters, false);
            }
            else if (searchString.Contains(","))
            {
                //имя ИЛИ фамилия должны частично соответствовать поиску
                parameters = searchString.Split(',');
                contacts = dBOperator.GetByNameParts(parameters, true);
            }
            else
            {
                //поиск по одной строке
                contacts = dBOperator.GetByNameParts(searchString);
             }
     
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "d MMMM, yyyy";
            string responce = JsonConvert.SerializeObject(contacts, settings);
            return responce;
        }

        [OperationContract]
        public string GetOrganizationList()
        {
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);
            List<Organization> organizations = dBOperator.GetOrgList();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "d MMMM, yyyy";
            string responce = JsonConvert.SerializeObject(organizations, settings);
            return responce;
        }
    }
}
