using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace LangLib
{
    public class EmitEnumBuilder
    {
        public static Type BuildDynamicEnum()
        {
            var assBdr =
                AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("TempAssembly"), AssemblyBuilderAccess.Run);
            var moduleBdr = assBdr.DefineDynamicModule(assBdr.FullName);
            var enumBdr = moduleBdr.DefineEnum("Elevation", TypeAttributes.Public, typeof(int));
            enumBdr.DefineLiteral("Low", 0);
            enumBdr.DefineLiteral("High", 1);
            Type finished = enumBdr.CreateTypeInfo();
            return finished;
        }
    }
}
