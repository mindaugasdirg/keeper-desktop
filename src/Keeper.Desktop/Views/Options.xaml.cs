using Keeper.Desktop.Properties;
using Keeper.Desktop.Services;
using Keeper.Desktop.Utilities;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Keeper.Desktop.Views
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : UserControl
    {
        public bool Synchronization { get; set; }
        public string SynchronizationCode { get; set; }

        [Inject]
        public DataTransactionsService DataTransactionsService { get; set; }
        [Inject]
        public NetworkService NetworkService { get; set; }
        [Inject]
        public SecurityService SecurityService { get; set; }

        public Options()
        {
            InitializeComponent();
            this.InjectProperties();

            Container.DataContext = this;

            Synchronization = Settings.Default.SynchronizationOn;
            SynchronizationCode = string.Format("{0}:{1}:{2}", Settings.Default.ClientKey, Settings.Default.ClientSecret, Settings.Default.SecretKey);
            SyncToggle.IsOn = Synchronization;
            if (Synchronization)
                SynchronizationInfo.Visibility = Visibility.Visible;
        }

        private async void ResetData_Click(object sender, RoutedEventArgs e)
        {
            var dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            };

            var result = await Utilities.Utilities.GetWindow()?.ShowMessageAsync("Confirm Data Reset",
                "Are you sure you want to reset data? This will delete all data across all synchronized devices.", MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
            if(result == MessageDialogResult.Affirmative)
                DataTransactionsService.ClearData();
        }

        private async void ToggleSynchronization(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            if (toggle is null || Synchronization)
                return;

            if (!toggle.IsOn)
            {
                SynchronizationInfo.Visibility = Visibility.Collapsed;
                Settings.Default.SynchronizationOn = false;
                Settings.Default.ClientKey = string.Empty;
                Settings.Default.ClientSecret = string.Empty;
                Settings.Default.SecretKey = string.Empty;
                Synchronization = false;
                Settings.Default.Save();
                return;
            }

            var dialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Create new",
                NegativeButtonText = "Connect to existing"
            };

            var result = await Utilities.Utilities.GetWindow()?.ShowMessageAsync("Setup Synchronization",
                "Do you want to create new synchronization profile or to connect to already existing?", MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
            if (result == MessageDialogResult.Affirmative)
            {
                SynchronizationInfo.Visibility = Visibility.Visible;
                Settings.Default.SynchronizationOn = true;
                Synchronization = true;
                SecurityService.CreateSecrets();
                await NetworkService.Register();
                await NetworkService.Login();
                await DataTransactionsService.Synchronize();
            }
            else if (result == MessageDialogResult.Negative)
            {
                var inputDialogSettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Connect",
                    NegativeButtonText = "Cancel"
                };
                var inputResult = await Utilities.Utilities.GetWindow()?.ShowInputAsync("Connect Synchronization Profile", "Enter Synchronization Code", inputDialogSettings);

                if (string.IsNullOrWhiteSpace(inputResult))
                {
                    Settings.Default.Reset();
                    return;
                }

                var values = inputResult.Split(":");
                if (values.Length != 3)
                    return;
                Settings.Default.SynchronizationOn = true;
                Settings.Default.ClientKey = values[0];
                Settings.Default.ClientSecret = values[1];
                Settings.Default.SecretKey = values[2];
                Settings.Default.Save();
                SynchronizationInfo.Visibility = Visibility.Visible;
                SynchronizationCode = inputResult;
                Synchronization = true;
                await NetworkService.Login();
                await DataTransactionsService.Synchronize();
            }
        }
    }
}
