using System;
using System.Collections.Generic;

namespace hw3
{
    internal class Program
    {
        class Base
        {
        }

        class Derived : Base
        {
        }

        public static void Main(string[] args)
        {
//            var b = Option<Base>.Some(new Base());
//            var d = (Derived) b.Value;

            var d = Option<Derived>.Some(new Derived());
            var b = (Base) d.Value;

        }
    }
}