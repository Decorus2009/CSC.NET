using System;

namespace Hw2.MyAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeAttribute : Attribute
    {
    }
}