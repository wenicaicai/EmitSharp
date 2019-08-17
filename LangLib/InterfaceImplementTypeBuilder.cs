using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using LangLib.Utils;

namespace LangLib
{
    public interface IGetString
    {
        string Get();
    }
    public class InterfaceImplementTypeBuilder
    {
        public static Type CreateTypeImpIGetString()
        {
            var typeBuilder = new ClassTypeBuilder();
            typeBuilder.DefineType("TempAssembly", "TempAssembly", "GetClass", new[] { typeof(IGetString) });
            var getMethodBdr = typeBuilder.DefineMethod("Get", typeof(string),
                  MethodAttributes.Public
                  | MethodAttributes.HideBySig
                  | MethodAttributes.NewSlot
                  | MethodAttributes.Virtual
                  | MethodAttributes.Final, typeof(IGetString).GetMethod("Get"));
            var getMethodIl = getMethodBdr.GetILGenerator();
            getMethodIl.Emit(OpCodes.Ldstr, "Aloha");
            getMethodIl.Emit(OpCodes.Ret);
            return typeBuilder.CreateType();
        }
    }
}
