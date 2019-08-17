using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace LangLib.Utils
{
    public class ClassTypeBuilder
    {
        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _moduleBuilder;
        private TypeBuilder _typeBuilder;
        readonly Dictionary<string, FieldBuilder> _fieldBuilders = new Dictionary<string, FieldBuilder>();
        public ClassTypeBuilder DefineType(string assemblyName, string moduleName, string typeName)
        {
            _assemblyBuilder =
                AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(moduleName);
            _typeBuilder = _moduleBuilder.DefineType(typeName, TypeAttributes.Public);
            return this;
        }

        public ClassTypeBuilder DefineField(string fieldName, Type fieldType, FieldAttributes fieldAttributes)
        {
            _fieldBuilders[fieldName] = _typeBuilder.DefineField(fieldName, fieldType, fieldAttributes);
            return this;
        }

        public void GetFieldMethodImp(string fieldName, string methodName, MethodAttributes methodAttributes)
        {
            var getFieldMethod = _typeBuilder.DefineMethod(methodName, methodAttributes, _fieldBuilders[fieldName].FieldType, null);
            var il = getFieldMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, _fieldBuilders[fieldName]);
            il.Emit(OpCodes.Ret);
        }

        public ClassTypeBuilder DefineConstructor0(params string[] fieldNames)
        {
            var objType = typeof(object);
            var fieldTypes = fieldNames.Select(name => _fieldBuilders[name].FieldType).ToArray();

            var constructorBdr = _typeBuilder.DefineConstructor(MethodAttributes.Public,
                CallingConventions.Standard, fieldTypes);
            var ctorIl = constructorBdr.GetILGenerator();
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Call, objType.GetConstructor(Type.EmptyTypes));

            for (var argIndex = 1; argIndex <= fieldNames.Length; argIndex++)
            {
                var fieldName = fieldNames[argIndex - 1];
                ctorIl.Emit(OpCodes.Ldarg_0);
                ctorIl.Emit(OpCodes.Ldarg, argIndex);
                ctorIl.Emit(OpCodes.Stfld, _fieldBuilders[fieldName]);
            }
            ctorIl.Emit(OpCodes.Ret);
            return this;
        }

        public Type CreateType()
        {
            return _typeBuilder.CreateType();
        }
    }
}
