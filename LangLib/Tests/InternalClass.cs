using System;
using System.Collections.Generic;
using System.Text;

namespace LangLib.Tests
{
    public class PublicClass
    {
        public dynamic GetAnInternalObject()
        {
            return new InternalClass();
        }
    }

    internal class InternalClass
    {

    }
}
