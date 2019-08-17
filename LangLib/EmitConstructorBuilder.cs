using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using LangLib.Utils;

namespace LangLib
{
    public class EmitConstructorBuilder
    {
        public static Type DynamicPointTypeGen()
        {
            var classTypeBuilder = new ClassTypeBuilder();
            classTypeBuilder.DefineType("MyDynamicAssembly", "PointModule", "Point");
            classTypeBuilder.DefineField("x", typeof(int), FieldAttributes.Public);
            classTypeBuilder.DefineField("y", typeof(int), FieldAttributes.Public);
            classTypeBuilder.DefineField("z", typeof(int), FieldAttributes.Public);

            classTypeBuilder.DefineConstructor0("x", "y", "z");

            classTypeBuilder.GetFieldMethodImp("x", "GetX", MethodAttributes.Public);
            classTypeBuilder.GetFieldMethodImp("y", "GetY", MethodAttributes.Public);
            classTypeBuilder.GetFieldMethodImp("z", "GetZ", MethodAttributes.Public);
        

            return classTypeBuilder.CreateType();
        }
    }
}
