using System;
using System.Collections.Generic;
using System.Text;
using LangLib.Tests;
using Microsoft.CSharp.RuntimeBinder;
using Xunit;
using Xunit.Abstractions;

namespace LangTest.DynamicTests
{
    public class DynamicTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DynamicTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private class Foo<T>
        {
            public T Value { get; }

            public Foo(T value)
            {
                Value = value;
            }
        }

        private interface IFoo<T>
        {
            T Value { get; }
        }

        private interface IBar<T>
        {
            T Value { get; }
        }

        private class FooImp : IFoo<string>, IBar<string>
        {
            string IFoo<string>.Value => "Foo";
            string IBar<string>.Value => "Bar";
        }


        [Fact]
        public void DynamicGetTest()
        {
            dynamic obj = new Foo<string>("Test");
            try
            {
                _testOutputHelper.WriteLine(obj.Value);
                Assert.True(true);
            }
            catch (RuntimeBinderException e)
            {
                _testOutputHelper.WriteLine(e.Message);
                Assert.True(false);
            }
        }

        [Fact]
        public void DynamicCannotWorkOnClassWithExplicitInterfaceTest()
        {
            dynamic obj = new FooImp();
            Assert.Throws<RuntimeBinderException>(() => { _testOutputHelper.WriteLine(obj.Value); });
        }

        [Fact]
        public void DynamicCannotWorkOnClassUnreachableTest()
        {
            dynamic obj = new PublicClass();
            var exp = Assert.Throws<RuntimeBinderException>(() =>
            {
                obj.GetUnreachable();
            });
            _testOutputHelper.WriteLine(exp.ToString());
        }


    }
}
