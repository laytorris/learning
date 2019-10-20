using System;

namespace Homework1.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneRegularExpressionAttribute : Attribute
    {
        public string Value { get; set; }

        public PhoneRegularExpressionAttribute(string value)
        {
            Value = value;
        }
    }
}