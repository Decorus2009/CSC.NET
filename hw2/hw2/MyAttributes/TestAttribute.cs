using System;

namespace Hw2.MyAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute
    {
        public Type ExpectedException { get; set; }

        public string Ignore { get; set; }

        public TestAttribute(Type expectedException = null, string ignore = null)
        {
            ExpectedException = expectedException;
            Ignore = ignore;
        }
    }
}