using System;
using System.Reflection;
using System.Reflection.Emit;
using LangLib;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var type = EmitAttributeBuilder.BuildTypeWithCustomAttributesOnMethod();
            dynamic instance = Activator.CreateInstance(type);

            instance.HelloWorld();
        }
    }
}