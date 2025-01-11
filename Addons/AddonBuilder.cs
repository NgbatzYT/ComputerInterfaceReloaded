using System;

namespace ComputerInterfaceReloaded.Addons
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CIReloadedAttribute : Attribute
    {
        public string Version { get; }
        public string Name { get; }

        public CIReloadedAttribute(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}