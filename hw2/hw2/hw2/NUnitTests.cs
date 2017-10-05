using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace hw2
{
    /*
    Нормальный класс для тестирования своей тестирующей системы
    */
    [TestFixture]
    public class NUnitTests
    {
        private Assembly MyNUnitAssembly { get; set; }
        private IEnumerable<TestGroup> TestGroups { get; set; }

        [SetUp]
        public void InitAssembly()
        {
            /*
            Путь к файлу сборки данного проекта. Сборка содержит тип MyNUnitTests 
            с методами с атрибутами [Test], [Before], [After], [BeforeClass], [AfterClass].
            Почему-то не получилось указать через относительный путь.
            Нужно руками указывать его абсолютный путь в ФС. 
            */
            const string file = @"/Users/decorus/Dropbox/CSC/Courses/2017-2/C#/repo/CSC.NET/hw2/hw2/hw2/bin/Debug/hw2.exe";
            MyNUnitAssembly = Assembly.LoadFrom(file);
            TestGroups = TestDiscoverer.DiscoverIn(MyNUnitAssembly);
        }

        [Test]
        public void TestNumberOfTestGroups()
        {
            Assert.AreEqual(1, TestGroups.ToList().Count());
        }

        [Test]
        public void TestNumberOfDiscoveredBeforeAndAfterClassMethods()
        {
            var numberOfBeforeClassMethods = TestGroups.ToList()[0].BeforeClassMethods.Count();
            var numberOfAfterClassMethods = TestGroups.ToList()[0].AfterClassMethods.Count();
            Assert.AreEqual(1, numberOfBeforeClassMethods);
            Assert.AreEqual(1, numberOfAfterClassMethods);
        }

        [Test]
        public void TestNumberOfDiscoveredBeforeAndAfterMethods()
        {
            var numberOfBeforeMethods = TestGroups.ToList()[0].BeforeMethods.Count();
            var numberOfAfterMethods = TestGroups.ToList()[0].AfterMethods.Count();
            Assert.AreEqual(2, numberOfBeforeMethods);
            Assert.AreEqual(1, numberOfAfterMethods);
        }

        [Test]
        public void TestNumberOfDiscoveredTestMethods()
        {
            var numberOfTestMethods = TestGroups.ToList()[0].TestMethods.Count();
            Assert.AreEqual(9, numberOfTestMethods);
        }

        [Test]
        public void TestDiscoveredTestMethodsNames()
        {
            var testMethods = TestGroups.ToList()[0].TestMethods.ToList();
            Assert.AreEqual("MyTestSimpleEmpty", testMethods[0].Name);
            Assert.AreEqual("MyTestSimpleNonEmpty", testMethods[1].Name);
            Assert.AreEqual("MyTestExpectedNullReferenceException", testMethods[2].Name);
            Assert.AreEqual("MyTestExpectedIndexOutOfRangeException", testMethods[3].Name);
            Assert.AreEqual("MyTestUnexpectedNullReferenceException", testMethods[4].Name);
            Assert.AreEqual("MyTestUnexpectedIndexOutOfRangeException", testMethods[5].Name);
            Assert.AreEqual("MyTestIgnored", testMethods[6].Name);
            Assert.AreEqual("MyTestIgnoredWithExpectedException", testMethods[7].Name);
        }

        [Test]
        public void TestThat_MyTestSimpleEmpty_MethodPasses()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestSimpleEmpty");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Passed, testResult.Status);
            Assert.IsNull(testResult.CaughtException);
            Assert.IsNull(testResult.Message);
        }

        [Test]
        public void TestThat_MyTestSimpleNonEmpty_MethodPasses()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestSimpleNonEmpty");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Passed, testResult.Status);
            Assert.IsNull(testResult.CaughtException);
            Assert.IsNull(testResult.Message);
        }

        [Test]
        public void TestThat_MyTestExpectedNullReferenceException_MethodPasses()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestExpectedNullReferenceException");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Passed, testResult.Status);
            Assert.AreEqual(typeof(NullReferenceException), testResult.CaughtException);
            Assert.IsNotNull(testResult.Message);
        }

        [Test]
        public void TestThat_MyTestExpectedIndexOutOfRangeException_MethodPasses()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestExpectedIndexOutOfRangeException");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Passed, testResult.Status);
            Assert.AreEqual(typeof(IndexOutOfRangeException), testResult.CaughtException);
            Assert.IsNotNull(testResult.Message);
        }

        [Test]
        public void TestThat_MyTestUnexpectedNullReferenceException_MethodFails()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestUnexpectedNullReferenceException");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Failed, testResult.Status);
            Assert.AreEqual(typeof(NullReferenceException), testResult.CaughtException);
            Assert.IsNotNull(testResult.Message);
        }

        [Test]
        public void TestThat_MyTestUnexpectedIndexOutOfRangeException_MethodFails()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestUnexpectedIndexOutOfRangeException");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Failed, testResult.Status);
            Assert.AreEqual(typeof(IndexOutOfRangeException), testResult.CaughtException);
            Assert.IsNotNull(testResult.Message);
        }

        [Test]
        public void TestThat_MyTestIgnored_MethodIsIgnored()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestIgnored");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Ignored, testResult.Status);
            Assert.IsNull(testResult.CaughtException);
            Assert.IsNotNull(testResult.Message);
        }

        [Test]
        public void TestThat_MyTestIgnoredWithExpectedException_MethodIsIgnoredAndHasNoCaughtException()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestIgnoredWithExpectedException");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Ignored, testResult.Status);
            Assert.IsNull(testResult.CaughtException);
            Assert.IsNotNull(testResult.Message);
        }

        [Test]
        public void TestThat_MyTestIgnoredWithUnexpectedException_MethodIsIgnoredAndDoesNotThrow()
        {
            var type = TestGroups.ToList()[0].Type;
            var testMethods = TestGroups.ToList()[0].TestMethods;

            var testToRun = testMethods.ToList()
                .Find(methodInfo => methodInfo.Name == "MyTestIgnoredWithUnexpectedException");
            var testResult = TestRunner.RunTest(type, testToRun);

            Assert.AreEqual(Status.Ignored, testResult.Status);
            Assert.IsNull(testResult.CaughtException);
            Assert.IsNotNull(testResult.Message);
        }
    }
}