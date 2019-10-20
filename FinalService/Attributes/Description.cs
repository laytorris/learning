using System;

namespace Homework1.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
   public sealed class DescriptionAttribute : Attribute
    {
        public string Value { get; set; }

        public DescriptionAttribute(string value)
        {
            Value = value;
        }
    }
}