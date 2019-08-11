using System;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;

namespace Tests
{
    public class EmitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var aName = new AssemblyName("DynamicAssemblyExample");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(aName.Name + ".dll");
            var typeBuilder = moduleBuilder.DefineType("MyDynamicType", TypeAttributes.Public);
            var fbNumber = typeBuilder.DefineField("m_number", typeof(int), FieldAttributes.Private);
            Type[] parameterTypes = {typeof(int)};
            var ctor1 = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);

            ILGenerator ctor1IL = ctor1.GetILGenerator();
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Ldarg_1);
            ctor1IL.Emit(OpCodes.Stfld, fbNumber);
            ctor1IL.Emit(OpCodes.Ret);

            ConstructorBuilder ctor0 = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            ILGenerator ctor0IL = ctor0.GetILGenerator();
            ctor0IL.Emit(OpCodes.Ldarg_0);
            ctor0IL.Emit(OpCodes.Ldc_I4_S, 42);
            ctor0IL.Emit(OpCodes.Call, ctor1);
            ctor0IL.Emit(OpCodes.Ret);

            PropertyBuilder pbNumber = typeBuilder.DefineProperty("Number", PropertyAttributes.HasDefault, typeof(int), null);
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            MethodBuilder mbNumberGetAccessor = typeBuilder.DefineMethod("get_Number", getSetAttr, typeof(int), Type.EmptyTypes);

            ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
            numberGetIL.Emit(OpCodes.Ldarg_0, fbNumber);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            numberGetIL.Emit(OpCodes.Ret);

            MethodBuilder mbNumberSetAccessor = typeBuilder.DefineMethod(
                "set_Number",
                getSetAttr,
                null,
                new Type[] {typeof(int)});

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            numberSetIL.Emit(OpCodes.Ret);

            // Last, map the "get" and "set" accessor methods to the 
            // PropertyBuilder. The property is now complete. 
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);

            // Define a method that accepts an integer argument and returns
            // the product of that integer and the private field m_number. This
            // time, the array of parameter types is created on the fly.
            MethodBuilder meth = typeBuilder.DefineMethod(
                "MyMethod",
                MethodAttributes.Public,
                typeof(int),
                new Type[] {typeof(int)});

            ILGenerator methIL = meth.GetILGenerator();
            // To retrieve the private instance field, load the instance it
            // belongs to (argument zero). After loading the field, load the 
            // argument one and then multiply. Return from the method with 
            // the return value (the product of the two numbers) on the 
            // execution stack.
            methIL.Emit(OpCodes.Ldarg_0);
            methIL.Emit(OpCodes.Ldfld, fbNumber);
            methIL.Emit(OpCodes.Ldarg_1);
            methIL.Emit(OpCodes.Mul);
            methIL.Emit(OpCodes.Ret);

            Type t = typeBuilder.CreateType();
            MethodInfo mi = t.GetMethod("MyMethod");
            PropertyInfo pi = t.GetProperty("Number");

            // Create an instance of MyDynamicType using the default 
            // constructor. 
            object o1 = Activator.CreateInstance(t);

            // Display the value of the property, then change it to 127 and 
            // display it again. Use null to indicate that the property
            // has no index.
            Console.WriteLine("o1.Number: {0}", pi.GetValue(o1, null));
            pi.SetValue(o1, 127, null);
            Console.WriteLine("o1.Number: {0}", pi.GetValue(o1, null));

            // Call MyMethod, passing 22, and display the return value, 22
            // times 127. Arguments must be passed as an array, even when
            // there is only one.
            object[] arguments = {22};
            Console.WriteLine("o1.MyMethod(22): {0}",
                mi.Invoke(o1, arguments));

            // Create an instance of MyDynamicType using the constructor
            // that specifies m_Number. The constructor is identified by
            // matching the types in the argument array. In this case, 
            // the argument array is created on the fly. Display the 
            // property value.
            object o2 = Activator.CreateInstance(t,
                new object[] {5280});
            Console.WriteLine("o2.Number: {0}", pi.GetValue(o2, null));
        }
    }
}