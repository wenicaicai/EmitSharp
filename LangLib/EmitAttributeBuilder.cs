using System;
using System.Reflection;
using System.Reflection.Emit;

namespace LangLib
{
    public class EmitAttributeBuilder
    {
        public static Type BuildTypeWithCustomAttributesOnMethod()
        {
            var myAsmName = new AssemblyName("MyAssembly");
            var assBuilder = AssemblyBuilder.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.Run);
            var mb = assBuilder.DefineDynamicModule("MyModule");

            var cusTypeBuilder = mb.DefineType("MyType", TypeAttributes.Public);

            //将定义好的属性施加到自定义类上
            var ctorParams = new[] {typeof(string)};
            var attrCtorInfo = typeof(ClassCreatorAttribute).GetConstructor(ctorParams);

            var myCustomAttributeBdr = new CustomAttributeBuilder(attrCtorInfo, new[] {"LC95"});
            cusTypeBuilder.SetCustomAttribute(myCustomAttributeBdr);

            //生成一个DateLastUpdatedAttribute属性
            var myMethodBuilder = cusTypeBuilder.DefineMethod("HelloWorld",
                MethodAttributes.Public,
                null,
                new Type[] { });

            ctorParams = new Type[] { typeof(string) };
            attrCtorInfo = typeof(DateLastUpdatedAttribute).GetConstructor(ctorParams);

            CustomAttributeBuilder myCABuilder2 = new CustomAttributeBuilder(
                attrCtorInfo,
                new object[] { DateTime.Now.ToString() });

            myMethodBuilder.SetCustomAttribute(myCABuilder2);

            ILGenerator myIL = myMethodBuilder.GetILGenerator();

            myIL.EmitWriteLine("Hello, world!");
            myIL.Emit(OpCodes.Ret);

            return cusTypeBuilder.CreateType();
        }
    }
}