using System;

namespace Homework1.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MinBirthDateAttribute : Attribute
    {
        public DateTime Value { get; set; }

        public MinBirthDateAttribute(string value)
        {
            Value = DateTime.Parse(value);
        }
    }
}