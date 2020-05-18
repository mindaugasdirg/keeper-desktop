using System;

namespace Keeper.Desktop.Utilities
{
    /// <summary>
    /// Inject attribute marks that property value should be injected from the window's service provider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class Inject : Attribute { }
}
