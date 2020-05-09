using Keeper.Desktop.Views;
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

namespace Keeper.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var activitiesTab = new TabLayout();
            activitiesTab.ToolBar.Child = new ActivitiesToolbar();
            activitiesTab.Content.Child = new ActivitiesContent();
            ActivitiesFrame.Children.Add(activitiesTab);
        }
    }
}
