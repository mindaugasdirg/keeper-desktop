using System.Reflection;

namespace Keeper.Desktop.Utilities
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Injects values from MainWindow.ServiceProvider to properties with Inject property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void InjectProperties<T>(this T obj)
        {
            var window = Utilities.GetWindow();
            var type = obj.GetType();
            foreach(var field in type.GetProperties())
            {
                if (field.GetCustomAttribute(typeof(Inject), false) is null)
                    continue;
                var value = window.GetService(field.PropertyType);
                field.SetValue(obj, value);
            }
        }

        public static string AddQuotes(this string str) => string.Format("\"{0}\"", str);
    }
}
