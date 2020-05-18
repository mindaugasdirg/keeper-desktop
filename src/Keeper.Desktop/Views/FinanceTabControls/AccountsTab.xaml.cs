using Keeper.Desktop.Models;
using Keeper.Desktop.Services;
using Keeper.Desktop.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Keeper.Desktop.Views.FinanceTabControls
{
    /// <summary>
    /// Interaction logic for AccountsTab.xaml
    /// </summary>
    public partial class AccountsTab : UserControl
    {
        [Inject]
        public DataTransactionsService DataTransactionsService { get; set; }
        [Inject]
        public AccountsService AccountsService { get; set; }

        private List<Account> accounts = new List<Account>();
        private readonly List<string> currencies = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(c => new RegionInfo(c.LCID).ISOCurrencySymbol).OrderBy(c => c).Distinct().ToList();
        private FormHandlerService<Account> formHandler;

        public AccountsTab()
        {
            InitializeComponent();
            this.InjectProperties();

            formHandler = new FormHandlerService<Account>("Account", AccountForm, DataTransactionsService, UpdateList, ValidateCategory);

            NewButton.Click += formHandler.New;
            EditButton.Click += (object sender, RoutedEventArgs e) => formHandler.Edit(DataGridControl.SelectedItem as Account);
            DeleteButton.Click += (object sender, RoutedEventArgs e) => formHandler.Delete(DataGridControl.SelectedItem as Account);
            SaveButton.Click += formHandler.Save;

            CurrencyField.ItemsSource = currencies;

            UpdateList();
        }

        private void UpdateList()
        {
            Utilities.Utilities.UpdateList(accounts, AccountsService);
            DataGridControl.ItemsSource = null;
            DataGridControl.ItemsSource = accounts;
        }

        private bool ValidateCategory(Account account) => !string.IsNullOrWhiteSpace(account.Name) && !string.IsNullOrWhiteSpace(account.Currency);
    }
}
