using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Keeper.Desktop.Navigation
{
    public class NavigationMenuItems
    {
        public ObservableCollection<NavigationMenuItem> Items { get; set; } = new ObservableCollection<NavigationMenuItem>();
        public ObservableCollection<NavigationMenuItem> Options { get; set; } = new ObservableCollection<NavigationMenuItem>();

        public NavigationMenuItems()
        {
            Items.Add(new NavigationMenuItem() { IconKey = PackIconMaterialLightKind.Briefcase, Label = "Activities", View = new Uri("Views/ActivitiesTabControls/Activities.xaml", UriKind.RelativeOrAbsolute) });
            Items.Add(new NavigationMenuItem() { IconKey = PackIconMaterialLightKind.CurrencyEur, Label = "Finances", View = new Uri("Views/FinanceTabControls/Finances.xaml", UriKind.RelativeOrAbsolute) });
            Items.Add(new NavigationMenuItem() { IconKey = PackIconMaterialLightKind.Label, Label = "Categories", View = new Uri("Views/CategoriesTabControls/CategoriesTab.xaml", UriKind.RelativeOrAbsolute) });
            Items.Add(new NavigationMenuItem() { IconKey = PackIconMaterialLightKind.ChartBar, Label = "Statistics", View = new Uri("Views/StatisticsTabControls/StatisticsTab.xaml", UriKind.RelativeOrAbsolute) });
            Options.Add(new NavigationMenuItem() { IconKey = PackIconMaterialLightKind.Cog, Label = "Options", View = new Uri("Views/Options.xaml", UriKind.RelativeOrAbsolute) });
        }
    }
}
