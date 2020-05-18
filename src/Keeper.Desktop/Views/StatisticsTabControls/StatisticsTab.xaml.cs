using Keeper.Desktop.Models;
using Keeper.Desktop.Services;
using Keeper.Desktop.Utilities;
using LiveCharts;
using LiveCharts.Wpf;
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

namespace Keeper.Desktop.Views.StatisticsTabControls
{
    /// <summary>
    /// Interaction logic for StatisticsTab.xaml
    /// </summary>
    public partial class StatisticsTab : UserControl
    {
        [Inject]
        public TransactionsService TransactionsService { get; set; }
        [Inject]
        public TimeEntriesService TimeEntriesService { get; set; }
        [Inject]
        public AccountsService AccountsService { get; set; }
        public SeriesCollection SpendByDay { get; set; } = new SeriesCollection();
        public SeriesCollection MonthsActivities { get; set; } = new SeriesCollection();
        public string[] Labels { get; set; }
        public Func<decimal, string> Formatter { get; set; }
        public Func<double, string> DurationFormatter { get; set; }
        public List<Account> Accounts { get; set; }

        public StatisticsTab()
        {
            InitializeComponent();
            this.InjectProperties();

            DataContext = this;

            Accounts = AccountsService.GetAll();

            ShowActivityReport();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (e.AddedItems[0] as ComboBoxItem).Content as string;

            if(!(MonthsActivitiesChart is null))
                MonthsActivitiesChart.Visibility = selected == "Activities" ? Visibility.Visible : Visibility.Collapsed;
            if (!(SpendByDayChart is null)) 
                SpendByDayChart.Visibility = selected == "Finances" ? Visibility.Visible : Visibility.Collapsed;

            switch (selected)
            {
                case "Activities":
                    ShowActivityReport();
                    break;
                case "Finances":
                    ShowFinanceReport();
                    break;
            }
        }

        private void ShowActivityReport()
        {
            var durations = TimeEntriesService.GetMonthsActivities();
            MonthsActivities.Clear();
            foreach(var duration in durations)
            {
                MonthsActivities.Add(new ColumnSeries()
                {
                    Title = duration.Activity.Name,
                    Values = new ChartValues<double>() { duration.TotalDuration }
                });
            }
            Labels = durations.Select(d => d.Activity.Name).ToArray();
            DurationFormatter = value => TimeSpan.FromSeconds(value).ToString("dd\\.hh\\:mm\\:ss");
        }

        private void ShowFinanceReport()
        {
            SpendByDay.Clear();
            var days = TransactionsService.GetSpendByDay(Accounts[0]);
            SpendByDay.Add(new ColumnSeries()
            {
                Title = Accounts[0].Name,
                Values = new ChartValues<decimal>(days.Select(d => Math.Round(d.Sum, 2)))
            });
            Labels = days.Select(i => i.Day.ToString()).ToArray();
            Formatter = value => value.ToString("N2");
        }
    }
}
