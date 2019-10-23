using System;
using System.ComponentModel.DataAnnotations;
using Homework1.Attributes;

namespace Homework1
{
    [Description("Class description")]
    public sealed class Contact : ICloneable
    {
        public Contact()
        {

        }
        public Contact(string name, string surname, string middleName,
         char gender, DateTime? birthDate, string phone,
         string taxNumber, string position, Organization organization)
        {
            Name = name;
            Surname = surname;
            MiddleName = middleName;
            Gender = gender;
            BirthDate = birthDate;
            Phone = phone;
            TaxNumber = taxNumber;
            Position = position;
            Job = organization;
        }

        public Contact(string name, string surname, string middleName,
            char gender, DateTime? birthDate, string phone,
            string taxNumber, string position, Organization organization, int id)
        {
            Name = name;
            Surname = surname;
            MiddleName = middleName;
            Gender = gender;
            BirthDate = birthDate;
            Phone = phone;
            TaxNumber = taxNumber;
            Position = position;
            Job = organization;
            ID = id;
        }
        private int _ID;
        private string _Surname;
        private string _Name;
        private string _TaxNumber;


        [MaxStringLength(100)]
        [Required(ErrorMessage = "Surname is required.")]
        public string Surname {
            get
            {
                return _Surname;
            }
            set
            {
                if (value == null)
                    throw new Exception("Surname does not accept null values");
                else
                   _Surname = value;
            }
        }
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {

                _ID = value;
            }
        }
        [MaxStringLength(50)]
        [Required(ErrorMessage = "Name is required.")]
        public string Name {
            get
            {
                return _Name;
            }
            set
            {
                if (value == null)
                    throw new Exception("Name does not accept null values");
                else
                    _Name = value;
            }
        }

        [MaxStringLength(100)]
        public string MiddleName { get; set; }

        public char Gender { get; set; }

        [PhoneRegularExpression(@"^\s*\+?\s*([0-9][\s-]*){10,}$")]
        public string Phone { get; set; }

        [MinBirthDate("2017-01-01")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Tax number is required.")]
        public string TaxNumber {
            get
            {
                return _TaxNumber;
            }
            set
            {
                if (value == null)
                    throw new Exception("Tax number does not accept null values");
                else
                    _TaxNumber = value;
            }
        }

        public string Position { get; set; }

        public Organization Job { get; set; }

        public object Clone()
        {
            Contact copy = (Contact)this.MemberwiseClone();
            Organization organisation = new Organization(this.Job?.Name, this.Job?.Phone);
            copy.Job = organisation;
            return copy;
        }

        public override string ToString() {
            return string.Concat(ID, " ", Surname, " ", Name, " ",
                MiddleName, " ", Gender, " ", Phone, " ",
                BirthDate.ToString(), " ", TaxNumber, " ", Job?.Name, " ", Position);
        }

        public string ToString(char delimiter)
        {
            return string.Concat(ID, delimiter, Surname, delimiter, Name, delimiter,
                MiddleName, delimiter, Gender, delimiter, Phone, delimiter, 
                BirthDate.ToString(), delimiter, TaxNumber, delimiter, Job?.Name, delimiter, Position);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType()) return false;

            Contact p = (Contact)obj;
            return (Name.Equals(Name, StringComparison.CurrentCulture)) 
                && (Surname.Equals(p.Surname))
                && (MiddleName.Equals (p.MiddleName))
                && (BirthDate.Equals (p.BirthDate))
                && (Phone.Equals (p.Phone))
                && (TaxNumber.Equals (p.TaxNumber))
                && (Gender.Equals(p.Gender))
                && (Job.Equals(p.Job))
                && (Position.Equals(p.Position));
        }

        public override int GetHashCode()
        {
           return TaxNumber.GetHashCode();
        }
    }
}