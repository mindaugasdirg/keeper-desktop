using Keeper.Desktop.Models;
using Keeper.Desktop.Services;
using Keeper.Desktop.Utilities;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TransactionsTab.xaml
    /// </summary>
    public partial class TransactionsTab : UserControl
    {
        [Inject]
        public DataTransactionsService DataTransactionsService { get; set; }
        [Inject]
        public AccountsService AccountsService { get; set; }
        [Inject]
        public CategoriesService CategoriesService { get; set; }
        [Inject]
        public TransactionsService TransactionsService { get; set; }

        private List<Transaction> transactions = new List<Transaction>();
        private List<Account> accounts = new List<Account>();
        private List<Category> validCategories = new List<Category>();
        private FormHandlerService<Transaction> formHandler;
        private Account activeAccount;

        public TransactionsTab()
        {
            InitializeComponent();
            this.InjectProperties();

            formHandler = new FormHandlerService<Transaction>("Activity", TransactionForm, DataTransactionsService, UpdateList, ValidateTransaction);
            formHandler.AttributeSetter = value => { value.Account = activeAccount; };
            NewButton.Click += formHandler.New;
            EditButton.Click += (object sender, RoutedEventArgs e) => formHandler.Edit(TransactionsDataGridControl.SelectedItem as Transaction);
            DeleteButton.Click += (object sender, RoutedEventArgs e) => formHandler.Delete(TransactionsDataGridControl.SelectedItem as Transaction);
            SaveButton.Click += formHandler.Save;

            AccountPicker.SelectedIndex = 0;
            UpdateList();
        }

        private void UpdateList()
        {
            Utilities.Utilities.UpdateList(accounts, AccountsService);
            Utilities.Utilities.UpdateList(validCategories, CategoriesService.GetTransactionOrAllCategories());
            AccountPicker.ItemsSource = accounts;
            activeAccount = AccountPicker.SelectedItem as Account;
            if(!(activeAccount is null))
                Utilities.Utilities.UpdateList(transactions, TransactionsService.GetAccountTransactions(activeAccount));
            TransactionsDataGridControl.ItemsSource = null;
            TransactionsDataGridControl.ItemsSource = transactions;
            CategoryPicker.ItemsSource = validCategories;
        }

        private bool ValidateTransaction(Transaction transaction) => !transaction.Amount.Equals(0) && !string.IsNullOrWhiteSpace(transaction.Description) && !(transaction.Account is null);

        private void AccountPicker_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateList();
    }
}
