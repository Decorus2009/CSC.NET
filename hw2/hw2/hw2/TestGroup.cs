using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace hw2
{
    /*
    Группа выявленных методов для тестирования и тип, содержащий данные методы
    */
    public class TestGroup
    {
        public Type Type { get; set; }
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

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append($"type: {Type}\n");
            result.Append("before class methods: \n");
            BeforeClassMethods.ToList().ForEach(mi => result.Append('\t' + mi.ToString() + '\n'));
            result.Append("before methods: \n");
            BeforeMethods.ToList().ForEach(mi => result.Append('\t' + mi.ToString() + '\n'));
            result.Append("test methods: \n");
            TestMethods.ToList().ForEach(mi => result.Append('\t' + mi.ToString() + '\n'));
            result.Append("after methods: \n");
            AfterMethods.ToList().ForEach(mi => result.Append('\t' + mi.ToString() + '\n'));
            result.Append("after class methods: \n");
            AfterClassMethods.ToList().ForEach(mi => result.Append('\t' + mi.ToString() + '\n'));
            return result.ToString();
        }
    }
}