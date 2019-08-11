using System;
using System.Reflection;
using System.Reflection.Emit;

namespace LangLib
{
    public class EmitGetSetClassBuilder
    {
        public static Type BuildType()
        {
            var aName = new AssemblyName("DynamicAssemblyExample");
            var ab = AssemblyBuilder.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndCollect);

            // 对于单模块的程序集, 模块名称通常为程序集名称加拓展名
            var mb =
                ab.DefineDynamicModule(aName.Name + ".dll");

            // 由模块定义一个类
            var tb = mb.DefineType(
                "MyDynamicType",
                 TypeAttributes.Public);

            // 增加一个int32类型的字段
            var fbNumber = tb.DefineField(
                "m_number",
                typeof(int),
                FieldAttributes.Private);

            // 定义一个构造函数, 接受一个int型参数, 并将其赋值给m_number字段
            Type[] parameterTypes = { typeof(int) };
            var ctor1 = tb.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                parameterTypes);

            ILGenerator ctor1IL = ctor1.GetILGenerator();
            // 对于一个构造函数, 参数0是新实例的引用, 
            // 在调用其基类的构造函数之前将其放到堆栈上
            // 指定基类(System.Object)的构造函数
            // 通过传递默认的构造函数参数的类的数组(Type.EmptyTypes)来完成
            ctor1IL.Emit(OpCodes.Ldarg_0);//将Arg0放到计算堆栈上
            ctor1IL.Emit(OpCodes.Call,
                typeof(object).GetConstructor(Type.EmptyTypes));//调用基类的构造函数
            ctor1IL.Emit(OpCodes.Ldarg_0);//object的引用放入ES
            ctor1IL.Emit(OpCodes.Ldarg_1);//int类型参数ES
            ctor1IL.Emit(OpCodes.Stfld, fbNumber);//给m_number字段赋值, 值为ES栈顶
            ctor1IL.Emit(OpCodes.Ret);

            //定义一个默认的构造函数
            var ctor0 = tb.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                Type.EmptyTypes);
            var ctor0IL = ctor0.GetILGenerator();
            // For a constructor, argument zero is a reference to the new
            // instance. Push it on the stack before pushing the default
            // value on the stack, then call constructor ctor1.
            ctor0IL.Emit(OpCodes.Ldarg_0);//将自身放入ES当中
            ctor0IL.Emit(OpCodes.Ldc_I4_S, 42);//将42放入S当中
            ctor0IL.Emit(OpCodes.Call, ctor1);//调用int类型参数的构造函数
            ctor0IL.Emit(OpCodes.Ret);

            //定义一个叫做Number的属性, 操作字段m_number
            // 因为属性可没有参数, 所以传入一个控制, 如果你不这么定义, 你必须定义成Type.EmptyTypes
            var pbNumber = tb.DefineProperty(
                "Number",
                PropertyAttributes.HasDefault,
                typeof(int),
                null);

            // 属性的get,set方法需要一些特别的属性
            var getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // 定义get_number方法
            var mbNumberGetAccessor = tb.DefineMethod(
                "get_Number",
                getSetAttr,
                typeof(int),
                Type.EmptyTypes);
            var numberGetIL = mbNumberGetAccessor.GetILGenerator();
            //对一个实例的属性而言, 第一个参数是实例, 第二个参数是字段. 
            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);//将实例的fbNumber字段放在ES上[L(oa)df(ie)ld], 此时ES只剩下number字段的值
            numberGetIL.Emit(OpCodes.Ret);//返回

            //定义一个set
            var mbNumberSetAccessor = tb.DefineMethod(
                "set_Number",
                getSetAttr,
                null,
                new Type[] { typeof(int) });

            var numberSetIL = mbNumberSetAccessor.GetILGenerator();
            numberSetIL.Emit(OpCodes.Ldarg_0);//将实例放到ES上
            numberSetIL.Emit(OpCodes.Ldarg_1);//将值放在ES上
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);//将值存储进fbNumber当中
            numberSetIL.Emit(OpCodes.Ret);

            // 为属性设置get,set方法
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);

            //定义一个public方法, 接受一个int类型参数, 将其转换为
            var mathMethod = tb.DefineMethod(
                "MyMethod",
                MethodAttributes.Public,
                typeof(int),
                new[] { typeof(int) });

            var mathIL = mathMethod.GetILGenerator();
            // To retrieve the private instance field, load the instance it
            // belongs to (argument zero). After loading the field, load the 
            // argument one and then multiply. Return from the method with 
            // the return value (the product of the two numbers) on the 
            // execution stack.
            mathIL.Emit(OpCodes.Ldarg_0);//将实例放入ES
            mathIL.Emit(OpCodes.Ldfld, fbNumber);//读取实例的fbNumber参数
            mathIL.Emit(OpCodes.Ldarg_1);//读取第二个参数
            mathIL.Emit(OpCodes.Mul);//乘法
            mathIL.Emit(OpCodes.Ret);

            // Finish the type.
            var t = tb.CreateType();
            return t;
        }
    }
}
