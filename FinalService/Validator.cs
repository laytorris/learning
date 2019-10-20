using System;
using Homework1.Attributes;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Homework1
{
    public class Validator
    {
        private List<Exception> _ExceptionList
  ;

        public Validator()
        {
            _ExceptionList = new List<Exception>();
        }

        public List<Exception> ExceptionList {
            get
            {
                return _ExceptionList;
            }
        }

        public bool ValidateFields(Contact contact)
        {
            Type typeinfo = typeof(Contact);
            return (NameIsCorrect(contact.Name, typeinfo)
                & SurnameIsCorrect(contact.Surname, typeinfo)
                & MiddleNameIsCorrect(contact.MiddleName, typeinfo)
                & BirthDateIsCorrect(contact.BirthDate, typeinfo)
                & PhoneNumberIsCorrect(contact.Phone, typeinfo));
           
        }
        private bool NameIsCorrect(string name, Type typeinfo)
        {
            var member = typeinfo.GetMember(nameof(Contact.Name));
            if (member.Length > 0)
            {
                var attribute = Attribute.GetCustomAttribute(member[0], typeof(MaxStringLengthAttribute));
                if (attribute != null && name.Length > ((MaxStringLengthAttribute)attribute).Value)
                {
                    _ExceptionList.Add(new Exception("The name is incorrect"));
                    return false;
                }
            }
            return true;
        }

        private bool SurnameIsCorrect(string surname, Type typeinfo)
        {
            var member = typeinfo.GetMember(nameof(Contact.Surname));
            if (member.Length > 0)
            {
                var attribute = Attribute.GetCustomAttribute(member[0], typeof(MaxStringLengthAttribute));
                if (attribute != null && surname.Length > ((MaxStringLengthAttribute)attribute).Value)
                {
                    _ExceptionList.Add(new Exception("The surname is incorrect"));
                    return false;
                }
            }
           return true;
            
        }

        private bool MiddleNameIsCorrect(string middleName, Type typeinfo)
        {

            var member = typeinfo.GetMember(nameof(Contact.MiddleName));
            if (member.Length > 0)
            {
                var attribute = Attribute.GetCustomAttribute(member[0], typeof(MaxStringLengthAttribute));
                if (attribute != null && middleName.Length > ((MaxStringLengthAttribute)attribute).Value)
                {
                    _ExceptionList.Add(new Exception("The middle name is incorrect"));
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private bool BirthDateIsCorrect(DateTime? birthDate, Type typeinfo)
        {
            var member = typeinfo.GetMember(nameof(Contact.BirthDate));
            if (member.Length > 0)
            {
                var attribute = Attribute.GetCustomAttribute(member[0], typeof(MinBirthDateAttribute));
                if (attribute != null && birthDate < ((MinBirthDateAttribute)attribute).Value)
                {
                    _ExceptionList.Add(new Exception("The birthdate is incorrect"));
                    return false;
                }
              
            }
            return true;
        }

        private bool PhoneNumberIsCorrect(string phone, Type typeinfo)
        {
            var member = typeinfo.GetMember(nameof(Contact.Phone));
            if (member.Length > 0)
            {
                var attribute = Attribute.GetCustomAttribute(member[0], typeof(PhoneRegularExpressionAttribute));
                if (attribute != null && !Regex.IsMatch(phone, ((PhoneRegularExpressionAttribute)attribute).Value))
                {
                  _ExceptionList.Add(new Exception("The phone is incorrect"));
                }
            }
            return true;
        }
    }
}
