using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hw2
{
    /*
    Группа выявленных методов для тестирования и тип, содержащий данные методы
    */
    public class TestGroup
    {
        public Type Type { get; }
        public IEnumerable<MethodInfo> BeforeClassMethods { get; set; }
        public IEnumerable<MethodInfo> BeforeMethods { get; set; }
        public IEnumerable<MethodInfo> TestMethods { get; set; }
        public IEnumerable<MethodInfo> AfterMethods { get; set; }
        public IEnumerable<MethodInfo> AfterClassMethods { get; set; }

        public TestGroup(
            Type type,
            IEnumerable<MethodInfo> beforeClassMethods, 
            IEnumerable<MethodInfo> beforeMethods, 
            IEnumerable<MethodInfo> testMethods, 
            IEnumerable<MethodInfo> afterMethods, 
            IEnumerable<MethodInfo> afterClassMethods)
        {
            Type = type;
            BeforeClassMethods = beforeClassMethods;
            BeforeMethods = beforeMethods;
            TestMethods = testMethods;
            AfterMethods = afterMethods;
            AfterClassMethods = afterClassMethods;
        }
    }
}