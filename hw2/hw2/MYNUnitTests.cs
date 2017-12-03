using System;
using Hw2.MyAttributes;

namespace Hw2
{
    /*
    Набор простых тестовых методов 
    */
    public class MyNUnitTests
    {
        [BeforeClass]
        public void BeforeClass()
        {
        }

        [Before]
        public void Before()
        {
        }


        [Test]
        public void MyTestSimpleEmpty()
        {
        }

        [Test]
        public void MyTestSimpleNonEmpty()
        {
            const int a = 1;
            const int b = 2;
            var c = a + b;
        }

        [Test(ExpectedException = typeof(NullReferenceException))]
        public void MyTestExpectedNullReferenceException()
        {
            string s = null;
            var undefinedLength = s.Length;
        }

        [Test(ExpectedException = typeof(IndexOutOfRangeException))]
        public void MyTestExpectedIndexOutOfRangeException()
        {
            var array = new[] {1};
            var element = array[1];
        }

        [Test]
        public void MyTestUnexpectedNullReferenceException()
        {
            string s = null;
            var undefinedLength = s.Length;
        }

        [Test]
        public void MyTestUnexpectedIndexOutOfRangeException()
        {
            var array = new[] {1};
            var element = array[1];
        }

        [Test(Ignore = "Never invoke this method if you want to live")]
        public void MyTestIgnored()
        {
            Console.WriteLine("It should never be printed");
        }

        [Test(ExpectedException = typeof(Exception), Ignore = "Ignore even this test")]
        public void MyTestIgnoredWithExpectedException()
        {
            Console.WriteLine("It also shouldn't be printed");
        }

        [Test(Ignore = "Ignore even this test")]
        public void MyTestIgnoredWithUnexpectedException()
        {
            Console.WriteLine("It also shouldn't be printed");
            throw new NullReferenceException();
        }

        
        [After]
        public void After()
        {
        }

        [AfterClass]
        public void AfterClass()
        {
        }
    }
}