using System;

namespace Xaki
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class LocalizedAttribute : Attribute
    {
        public LocalizedAttribute() : base()
        {
        }
    }
}
