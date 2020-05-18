using Keeper.Desktop.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Keeper.Desktop.Utilities
{
    public static class Utilities
    {
        public static MainWindow GetWindow() => Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        public static void UpdateList<T>(List<T> list, ModelsService<T> service) where T : class => UpdateList(list, service.GetAll());

        public static void UpdateList<T>(List<T> list, IEnumerable<T> newValues) where T : class
        {
            list.Clear();
            list.AddRange(newValues);
        }
    }
}
