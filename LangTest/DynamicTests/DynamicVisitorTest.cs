using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace LangTest.DynamicTests
{
    public class DynamicVisitorTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DynamicVisitorTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private interface IVisitable
        {
            string Accept(IVisitor visitor);
        }

        private interface IVisitor
        {
            string Visit(IVisitable visitable);
        }
        //Define Two Person Class As IVisitable
        class PersonA : IVisitable
        {
            public string Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }

        class PersonB : IVisitable
        {
            public string Accept(IVisitor visitor)
            {
                return visitor.Visit(this);
            }
        }


        private class ToStringVisitor : IVisitor
        {
            public string Visit(IVisitable visitable)
            {
                return this.Visit((dynamic)visitable);
            }

            public string Visit(PersonA visitable)
            {
                return $"{nameof(ToStringVisitor)} Visited {nameof(PersonA)}";
            }

            public string Visit(PersonB visitable)
            {
                return $"{nameof(ToStringVisitor)} Visited {nameof(PersonB)}";
            }
        }
        private class ToElementPersonVisitor : IVisitor
        {
            public string Visit(IVisitable visitable)
            {
                return this.Visit((dynamic)visitable);
            }

            public string Visit(PersonA visitable)
            {
                return $"{nameof(ToElementPersonVisitor)} Visited {nameof(PersonA)}";
            }

            public string Visit(PersonB visitable)
            {
                return $"{nameof(ToElementPersonVisitor)} Visited {nameof(PersonB)}";
            }
        }

        [Fact]
        public void DynamicVisitorPatternTest()
        {
            IVisitable personA = new PersonA();
            IVisitable personB = new PersonB();

            IVisitor toStrVisitor = new ToStringVisitor();
            IVisitor toElementVisitor = new ToElementPersonVisitor();
            _testOutputHelper.WriteLine(personA.Accept(toElementVisitor));
            _testOutputHelper.WriteLine(personA.Accept(toStrVisitor));
            _testOutputHelper.WriteLine(personB.Accept(toElementVisitor));
            _testOutputHelper.WriteLine(personB.Accept(toStrVisitor));

        }


    }
}
