using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LangTest.DynamicTests
{
    public class DynamicVisitorTest
    {
        private interface IVisitable
        {
            void Accept(IVisitor visitor);
        }

        private interface IVisitor
        {
            void Visit(IVisitable visitable);
        }
        //Define Two Person Class As IVisitable
        class PersonA : IVisitable
        {
            public void Accept(IVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        class PersonB : IVisitable
        {
            public void Accept(IVisitor visitor)
            {
                visitor.Visit(this);
            }
        }


        private class ToStringVisitor : IVisitor
        {
            public void Visit(IVisitable visitable)
            {
                this.Visit((dynamic) visitable);
            }

            public void Visit(PersonA visitable)
            {
                Console.WriteLine($"{typeof(ToStringVisitor)} Visited {typeof(PersonA)})");
            }

            public void Visit(PersonB visitable)
            {
                Console.WriteLine($"{typeof(ToStringVisitor)} Visited {typeof(PersonB)})");
            }
        }
        private class ToElementPersonVisitor : IVisitor
        {
            public void Visit(IVisitable visitable)
            {
                this.Visit((dynamic)visitable);
            }

            public void Visit(PersonA visitable)
            {
                Console.WriteLine($"{typeof(ToElementPersonVisitor)} Visited {typeof(PersonA)})");
            }

            public void Visit(PersonB visitable)
            {
                Console.WriteLine($"{typeof(ToElementPersonVisitor)} Visited {typeof(PersonB)})");
            }
        }

        [Fact]
        public void DynamicVisitorPatternTest()
        {
            var personA = new PersonA();
            var personB = new PersonB();
            
            var toStrVisitor = new ToStringVisitor();
            var toElementVisitor = new ToElementPersonVisitor();
            personA.Accept(toElementVisitor);
            personA.Accept(toStrVisitor);
            personB.Accept(toElementVisitor);
            personB.Accept(toStrVisitor);

        }


    }
}
