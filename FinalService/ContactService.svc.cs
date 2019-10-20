using Homework1;
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

        [OperationContract]
        public void InsertContact(string Name, string Surname, string MiddleName,
                string Gender, string BirthDate, string Phone, string TaxNumber,
                string Position, string JobID)
        {
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

            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

            if (dBOperator.RowExists(ID))
            {
                ContactsDataConverter preparer = new ContactsDataConverter();
                Contact newContact = preparer.CreateInstance(Name, Surname, MiddleName,
                    Gender, BirthDate, Phone, TaxNumber, Position, JobID);
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

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string responce = serializer.Serialize(contact);
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

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string responce = serializer.Serialize(contacts);
            return responce;
        }

        [OperationContract]
        public string GetOrganizationList()
        {
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);
            List<Organization> organizations = dBOperator.GetOrgList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string responce = serializer.Serialize(organizations);
            return responce;
        }
    }
}
