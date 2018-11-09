using System;

namespace Xaki
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LocalizedAttribute : Attribute
    {
        public LocalizedAttribute() : base()
        {
        }
    }
}
