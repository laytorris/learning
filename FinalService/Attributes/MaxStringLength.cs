using System;

namespace Homework1.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxStringLengthAttribute : Attribute
    {
        public int Value { get; set; }

        public MaxStringLengthAttribute(int value)
        {
            Value = value;
        }
    }
}