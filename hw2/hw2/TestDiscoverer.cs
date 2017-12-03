using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hw2.MyAttributes;

namespace Hw2
{
    /*
    Проход по всем сборкам и нахождение методов с атрибутами из MyNUnit 
    */
    public class TestDiscoverer
    {
        public static Dictionary<Assembly, IEnumerable<TestGroup>> DiscoverIn(string path)
        {
            var testsInAssemblies = new Dictionary<Assembly, IEnumerable<TestGroup>>();

            foreach (var file in Directory.EnumerateFiles(path, ".", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe")))
            {
                var assembly = Assembly.LoadFrom(file);
                var testGroups = DiscoverIn(assembly);
                testsInAssemblies[assembly] = testGroups;
            }
            return testsInAssemblies;
        }

        public static IEnumerable<TestGroup> DiscoverIn(Assembly assembly)
        {
            var testGroups = new List<TestGroup>();
            foreach (var type in assembly.ExportedTypes)
            {
                var beforeClassMethods = GetMethodsWithAttribute<BeforeClassAttribute>(type);
                var beforeMethods = GetMethodsWithAttribute<BeforeAttribute>(type);
                var testMethods = GetMethodsWithAttribute<TestAttribute>(type);
                var afterMethods = GetMethodsWithAttribute<AfterAttribute>(type);
                var afterClassMethods = GetMethodsWithAttribute<AfterClassAttribute>(type);
                /*
                Райдер говорит, про "possible multiple enumeration", но
                кажется, по testMethods я пробегаю 1 раз в Any()
                Если неправ, поправьте, сохраню testMethods в лист  
                */
                if (testMethods.Any())
                {
                    testGroups.Add(new TestGroup(type, beforeClassMethods, beforeMethods,
                        testMethods, afterMethods, afterClassMethods));
                }
            }
            return testGroups;
        }

        private static IEnumerable<MethodInfo> GetMethodsWithAttribute<TAttribute>(Type type) =>
            type.GetMethods().Where(m => m.GetCustomAttributes(typeof(TAttribute), true).Length > 0);
    }
}