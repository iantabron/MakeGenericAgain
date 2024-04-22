using System;

namespace MakeGenericAgain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FromCommandLineAttribute: Attribute
    {
        public FromCommandLineAttribute(string paramName)
        {
            ParamName = paramName;
        }

        public string ParamName { get; set; }
    }
}