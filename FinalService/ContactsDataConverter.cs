using Homework1;
using System;
using System.Data.SqlClient;

namespace FinalService
{
    public class ContactsDataConverter
    {
        public Contact CreateInstance(string Name, string Surname, string MiddleName,
                string Gender, string BirthDate, string Phone, string TaxNumber,
                string Position, string JobID)
        {
            char charGender = Convert.ToChar(Gender);

            DateTime? birthDate;

            try
            {
                birthDate = Convert.ToDateTime(BirthDate);
            }
            catch (FormatException)
            {
                birthDate = null;
            }

            Organization job = new Organization();
            if (JobID != null)
            {
                
                job.ID = Convert.ToInt32(JobID);
                job.Name = "random";
           
            }
            Contact newContact = new Contact(Name, Surname, MiddleName, charGender,
                birthDate, Phone, TaxNumber, Position, job);
            return newContact;
        }

        internal Contact ContactFromDataReader(SqlDataReader reader)
        {
            Contact result = new Contact();
            result.ID = (int)FieldByName(reader, "ID");
            result.Name = (string)FieldByName(reader, "Name");
            result.Name.Replace(" ", String.Empty);
            result.Surname = (string)FieldByName(reader, "Surname");
            result.Surname.Replace(" ", String.Empty);
            result.MiddleName = (string)FieldByName(reader, "MiddleName");
            result.MiddleName.Replace(" ", String.Empty);
            var birthDate = FieldByName(reader, "BirthDate");
            if ((birthDate != null) && (birthDate != DBNull.Value))
            {
                result.BirthDate = Convert.ToDateTime(FieldByName(reader, "BirthDate"));
            }
           
            result.Gender = Convert.ToChar(FieldByName(reader, "Gender"));
            result.Phone = (string)FieldByName(reader, "Phone");
            result.Phone.Replace(" ", String.Empty);
            result.TaxNumber = (string)FieldByName(reader, "TaxNumber");
            result.TaxNumber.Replace(" ", String.Empty);
            result.Position = (string)FieldByName(reader, "Position");
            result.Position.Replace(" ", String.Empty);
            var JobID = (FieldByName(reader, "OrganizationID"));
            if ((JobID!= null)&&(JobID != DBNull.Value)){
                Organization job = new Organization();
                job.ID = (int)JobID;
                job.Name = "random";
                result.Job = job;
            }
            return result;
        }
        internal Organization OrganizationFromDataReader(SqlDataReader reader)
        {
            Organization result = new Organization();
            result.ID = (int)FieldByName(reader, "ID");
            result.Name = (string)FieldByName(reader, "Name");
            result.Name.Replace(" ", String.Empty);
            result.Phone = (string)FieldByName(reader, "Phone");
            result.Phone.Replace(" ", String.Empty);
            return result;
        }
        private object FieldByName(SqlDataReader reader, string ColumnName)
        {
            return reader.GetValue(reader.GetOrdinal(ColumnName));
        }
    }

}