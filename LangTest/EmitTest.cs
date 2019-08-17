using System;
using System.Diagnostics;
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
        public void EmitInterfaceImpTest()
        {
            var type = InterfaceImplementTypeBuilder.CreateTypeImpIGetString();
            var instance = Activator.CreateInstance(type);
            var inf = (IGetString) instance;
            var result = inf.Get(); 
            Assert.Equal("Aloha", result);
        }


        [Fact]
        public void EmitEnumBuilderTest()
        {
            var type = EmitEnumBuilder.BuildDynamicEnum();
            foreach (var value in Enum.GetValues(type))
            {
                _testOutputHelper.WriteLine($"{type},{value}={((int)value).ToString()}");
            }
        }


        [Fact]
        public void EmitConstructorTest()
        {
            var pointType = EmitConstructorBuilder.DynamicPointTypeGen();
            dynamic point = Activator.CreateInstance(pointType, 1, 2, 3);
            var xField = point.GetX();
            var yField = point.GetY();
            var zField = point.GetZ();
            _testOutputHelper.WriteLine($"xField : {xField}, yField : {yField}, zField : {zField}");
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