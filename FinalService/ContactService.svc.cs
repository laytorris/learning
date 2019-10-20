using Homework1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
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

            if (dBOperator.RowExists(ID, "@dbo.Contact"))
            {
                ContactsDataConverter preparer = new ContactsDataConverter();
                Contact newContact = preparer.CreateInstance(Name, Surname, MiddleName,
                    Gender, BirthDate, Phone, TaxNumber, Position, JobID);
                dBOperator.UpdateContact(newContact);
            }
            else
            {
                throw new WebFaultException<string>("Contacts doesn't exist", HttpStatusCode.BadRequest);
            }
        }

        [OperationContract]
        public void DeleteContact(string id)
        {
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);

            if (dBOperator.RowExists(id, "@dbo.Contact"))
            {

                dBOperator.DeleteContact(id);
            }
            else
            {
                throw new WebFaultException<string>("Contacts doesn't exist", HttpStatusCode.BadRequest);
            }
        }

        [OperationContract]
        public string GetContact(string  id)
        {
            string db = ConfigurationManager.AppSettings["DBConnectionString"];
            ContactsDBOperator dBOperator = new ContactsDBOperator(db);
            Contact contact = dBOperator.GetById(id);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string responce = serializer.Serialize(contact);
            return responce;
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
