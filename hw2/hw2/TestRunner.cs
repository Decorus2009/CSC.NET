using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace hw2
{
    /*
    Прогонщик тестов
    */
    public class TestRunner
    {
        public static void Run(Dictionary<Assembly, IEnumerable<TestGroup>> testsInAssemblies)
        {
            foreach (var testsInAssembly in testsInAssemblies)
            {
                Console.WriteLine($"Running tests in {testsInAssembly.Key}...\n");
                Run(testsInAssembly.Value);
            }
        }

        public static TestResult RunTest(Type type, MethodInfo testMethod)
        {
            var (ignored, message) = CheckThatIsIgnored(testMethod);
            if (ignored)
            {
                return new TestResult(Status.Ignored, message);
            }

            var stopWatch = new Stopwatch();
            TimeSpan ts;
            string elapsedTime;
            try
            {
                stopWatch.Start();
                Run(type, testMethod);
                /* может не выполниться из-за исключения */
                stopWatch.Stop();
                ts = stopWatch.Elapsed;
                elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
                
                return new TestResult(Status.Passed, elapsedTime: elapsedTime);
            }
            catch (Exception actualException)
            {
                if (actualException.InnerException == null)
                {
                    throw; // something's wrong
                }
                ts = stopWatch.Elapsed;
                elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";

                var (expected, expectedExceptionType) = CheckIfExceptionIsExpected(testMethod);
                if (expected && expectedExceptionType == actualException.InnerException.GetType())
                {
                    return new TestResult(Status.Passed, actualException.InnerException.Message,
                        actualException.InnerException.GetType(), elapsedTime);
                }
                return new TestResult(Status.Failed, actualException.InnerException.Message,
                    actualException.InnerException.GetType());
            }
        }

        private static void Run(IEnumerable<TestGroup> testGroups) => testGroups.ToList().ForEach(Run);

        private static void Run(TestGroup testGroup)
        {
            var type = testGroup.Type;
            testGroup.BeforeClassMethods.ToList().ForEach(memberInfo => Run(type, memberInfo));
            foreach (var testMethod in testGroup.TestMethods)
            {
                testGroup.BeforeMethods.ToList().ForEach(memberInfo => Run(type, memberInfo));
                var testResult = RunTest(type, testMethod);
                testGroup.AfterMethods.ToList().ForEach(memberInfo => Run(type, memberInfo));

                Console.WriteLine($"Test method \'{testMethod}\'\n{testResult}");
            }
            testGroup.AfterClassMethods.ToList().ForEach(memberInfo => Run(type, memberInfo));
        }

        /*
        Полагаем же, что методы с [BeforeClass], [Before], [MyTest], [After], [AfterClass] без параметров? 
        */
        private static void Run(Type type, MethodInfo methodInfo) =>
            methodInfo.Invoke(Activator.CreateInstance(type), new object[] { });

        private static (bool, Type) CheckIfExceptionIsExpected(MemberInfo memberInfo)
        {
            foreach (var customAttributeData in memberInfo.CustomAttributes)
            {
                if (customAttributeData.NamedArguments == null)
                {
                    continue;
                }
                foreach (var customAttributeNamedArgument in customAttributeData.NamedArguments)
                {
                    if (customAttributeNamedArgument.MemberName == "ExpectedException")
                    {
                        return (true, customAttributeNamedArgument.TypedValue.Value as Type);
                    }
                }
            }
            return (false, null);
        }

        private static (bool, string) CheckThatIsIgnored(MemberInfo memberInfo)
        {
            foreach (var customAttributeData in memberInfo.CustomAttributes)
            {
                if (customAttributeData.NamedArguments == null)
                {
                    continue;
                }
                foreach (var customAttributeNamedArgument in customAttributeData.NamedArguments)
                {
                    if (customAttributeNamedArgument.MemberName == "Ignore")
                    {
                        return (true, customAttributeNamedArgument.TypedValue.Value as string);
                    }
                }
            }
            return (false, null);
        }
    }
}