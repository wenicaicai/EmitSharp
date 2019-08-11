namespace LangLib
{
    public class EmitGetSetClassOrigin
    {
        private int m_number;

        public EmitGetSetClassOrigin() : this(42)
        {
        }
        public EmitGetSetClassOrigin(int initNumber)
        {
            m_number = initNumber;
        }

        public int Number
        {
            get { return m_number; }
            set { m_number = value; }
        }

        public int MyMethod(int multiplier)
        {
            return m_number * multiplier;
        }
    }
}