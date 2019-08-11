using System;

namespace LangLib
{
    public class ClassCreatorAttribute : Attribute
    {
        private string creator;

        public string Creator
        {
            get { return creator; }
        }

        public ClassCreatorAttribute(string name)
        {
            this.creator = name;
        }
    }

    public class DateLastUpdatedAttribute : Attribute
    {
        private string dateUpdated;
        public string DateUpdated
        {
            get
            {
                return dateUpdated;
            }
        }

        public DateLastUpdatedAttribute(string theDate)
        {
            this.dateUpdated = theDate;
        } 
    }
}