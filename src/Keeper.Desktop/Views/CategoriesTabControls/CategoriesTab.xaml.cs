using Keeper.Desktop.Models;
using Keeper.Desktop.Services;
using Keeper.Desktop.Utilities;
using MahApps.Metro.Controls.Dialogs;
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

namespace Keeper.Desktop.Views.CategoriesTabControls
{
    /// <summary>
    /// Interaction logic for CategoriesTab.xaml
    /// </summary>
    public partial class CategoriesTab : UserControl
    {
        [Inject]
        public DataTransactionsService DataTransactionsService { get; set; }
        [Inject]
        public CategoriesService CategoriesService { get; set; }

        private List<Category> categories = new List<Category>();
        private FormHandlerService<Category> formHandler;

        public CategoriesTab()
        {
            InitializeComponent();
            this.InjectProperties();

            formHandler = new FormHandlerService<Category>("Category", CategoryForm, DataTransactionsService, UpdateList, ValidateCategory);

            NewButton.Click += formHandler.New;
            EditButton.Click += (object sender, RoutedEventArgs e) => formHandler.Edit(DataGridControl.SelectedItem as Category);
            DeleteButton.Click += (object sender, RoutedEventArgs e) => formHandler.Delete(DataGridControl.SelectedItem as Category);
            SaveCategory.Click += formHandler.Save;

            UpdateList();
        }

        private void UpdateList()
        {
            Utilities.Utilities.UpdateList(categories, CategoriesService);
            DataGridControl.ItemsSource = null;
            DataGridControl.ItemsSource = categories;
        }

        private bool ValidateCategory(Category category) => !string.IsNullOrWhiteSpace(category.Name);
    }
}
