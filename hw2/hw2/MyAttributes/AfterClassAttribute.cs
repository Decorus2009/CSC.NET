using System;

namespace Hw2.MyAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterClassAttribute : Attribute
    {
    }
}