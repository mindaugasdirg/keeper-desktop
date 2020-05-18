using Keeper.Desktop.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Keeper.Desktop.Services
{
    public class FormHandlerService<T> where T : new()
    {
        public Action<T> AttributeSetter { get; set; }

        private DataTransactionsService dataTransactionsService;
        private T formTarget = new T();
        private string objectName;
        private bool addingNew;
        private Flyout form;
        private Action refreshList;
        private Predicate<T> validateObj;

        public FormHandlerService(string _objectName, Flyout _form, DataTransactionsService _dataTransactionsService, Action _refreshList, Predicate<T> _validateObj)
        {
            if (string.IsNullOrWhiteSpace(_objectName)) throw new ArgumentException(nameof(_objectName));
            if (_form is null) throw new ArgumentNullException(nameof(_form));
            if (_dataTransactionsService is null) throw new ArgumentNullException(nameof(_dataTransactionsService));
            if (_refreshList is null) throw new ArgumentNullException(nameof(_refreshList));
            if (_validateObj is null) throw new ArgumentNullException(nameof(_validateObj));

            objectName = _objectName;
            form = _form;
            dataTransactionsService = _dataTransactionsService;
            refreshList = _refreshList;
            validateObj = _validateObj;
        }

        public void Edit(T selected)
        {
            if (selected is null)
                return;

            formTarget = selected;
            addingNew = false;
            OpenForm();
        }

        public void New(object sender, RoutedEventArgs e)
        {
            formTarget = new T();
            addingNew = true;
            OpenForm();
        }

        private void OpenForm()
        {
            form.Header = string.Format("{0} {1}", addingNew ? "Create" : "Edit", objectName);
            form.DataContext = formTarget;
            form.IsOpen = true;
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            if(!(AttributeSetter is null))
                AttributeSetter(formTarget);
            if (!validateObj(formTarget))
                return;

            dataTransactionsService.HandleDataTransaction(new DataTransaction()
            {
                Action = addingNew ? DataTransaction.ActionType.Create : DataTransaction.ActionType.Edit,
                Data = formTarget
            });

            refreshList();
            form.IsOpen = false;
        }

        public async void Delete(T selected)
        {
            if (selected is null)
                return;

            if (await ConfirmDelete("Are you sure?", string.Format("Are you sure that you want to delete {0} category", objectName)))
            {
                dataTransactionsService.HandleDataTransaction(new DataTransaction()
                {
                    Action = DataTransaction.ActionType.Delete,
                    Data = selected
                });
                refreshList();
            }
        }

        private async Task<bool> ConfirmDelete(string title, string message)
        {
            var dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            };

            var result = await Utilities.Utilities.GetWindow()?.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
            return result == MessageDialogResult.Affirmative;
        }
    }
}
