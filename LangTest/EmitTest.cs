using System;
using System.Reflection;
using LangLib;
using Xunit;
using Xunit.Abstractions;

namespace Tests

{
    public class EmitTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public EmitTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public void Setup()
        {
        }


        [Fact]
        public void EmitGetSetTest()
        {
            var t = EmitGetSetClassBuilder.BuildType();
            dynamic o1 = Activator.CreateInstance(t);
            _testOutputHelper.WriteLine($"o1.Number: {o1.Number}");
            o1.Number = 128;
            _testOutputHelper.WriteLine($"o1.Number: {o1.Number}");
            _testOutputHelper.WriteLine($"MyMethod(22): {o1.MyMethod(22)}");


            dynamic o2 = Activator.CreateInstance(t, 5280);
            _testOutputHelper.WriteLine($"o2.Number: {o2.Number}");
        }
        [Fact]
        public void EmitAttributeTest()
        {
            var myType = EmitAttributeBuilder.BuildTypeWithCustomAttributesOnMethod();

            object myInstance = Activator.CreateInstance(myType);

            object[] customAttrs = myType.GetCustomAttributes(true);

            _testOutputHelper.WriteLine("Custom Attributes for Type 'MyType':");

            object attrVal = null;

            foreach (object customAttr in customAttrs) 
            {
                attrVal = typeof(ClassCreatorAttribute).InvokeMember("Creator", 
                    BindingFlags.GetProperty,
                    null, customAttr, new object[] { });
                _testOutputHelper.WriteLine("-- Attribute: [{0} = \"{1}\"]", customAttr, attrVal);
            }

            _testOutputHelper.WriteLine("Custom Attributes for Method 'HelloWorld()' in 'MyType':");

            var num = myType.GetMember("HelloWorld");
            customAttrs = myType.GetMember("HelloWorld")[0].GetCustomAttributes(true);	

            foreach (object customAttr in customAttrs) 
            {
                attrVal = typeof(DateLastUpdatedAttribute).InvokeMember("DateUpdated", 
                    BindingFlags.GetProperty,
                    null, customAttr, new object[] { });
                _testOutputHelper.WriteLine("-- Attribute: [{0} = \"{1}\"]", customAttr, attrVal);
            }

            _testOutputHelper.WriteLine("---");


        }
    }
}