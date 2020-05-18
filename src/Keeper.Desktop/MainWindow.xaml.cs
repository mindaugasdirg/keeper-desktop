using Keeper.Desktop.Navigation;
using Keeper.Desktop;
using Keeper.Desktop.Services;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Keeper.Desktop.Properties;

namespace Keeper.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public IServiceProvider ServiceProvider { get; }

        public MainWindow()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddDbContext<DatabaseContext>();
            serviceCollection.AddScoped<AccountsService>();
            serviceCollection.AddScoped<ActivitiesService>();
            serviceCollection.AddScoped<CategoriesService>();
            serviceCollection.AddScoped<DataTransactionsService>();
            serviceCollection.AddScoped<TimeEntriesService>();
            serviceCollection.AddScoped<TransactionsService>();
            serviceCollection.AddScoped<NetworkService>();
            serviceCollection.AddScoped<SecurityService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            if(Settings.Default.SynchronizationOn)
            {
                var networkService = GetService<NetworkService>();
                try
                {
                    networkService.Login();
                }
                catch
                {

                }
            }
            

            NavigationFrame.Navigate(new Uri("Views/ActivitiesTabControls/Activities.xaml", UriKind.RelativeOrAbsolute), null);
        }

        private void NavigationControl_OnItemClicked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            var clickedItem = e.InvokedItem as NavigationMenuItem;
            if (clickedItem is null || clickedItem.View is null || NavigationFrame.CurrentSource == clickedItem.View)
                return;

            NavigationFrame.Navigate(clickedItem.View, null);
            NavigationMenuControl.IsPaneOpen = false;
        }

        public T GetService<T>() => ServiceProvider.GetRequiredService<T>();

        public object GetService(Type type) => ServiceProvider.GetRequiredService(type);
    }
}
