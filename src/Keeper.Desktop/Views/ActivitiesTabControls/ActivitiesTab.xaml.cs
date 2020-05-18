using Keeper.Desktop.Models;
using Keeper.Desktop.Services;
using Keeper.Desktop.Utilities;
using MahApps.Metro.Controls.Dialogs;
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
using System.Windows.Threading;

namespace Keeper.Desktop.Views.ActivitiesTabControls
{
    /// <summary>
    /// Interaction logic for ActivitiesTab.xaml
    /// </summary>
    public partial class ActivitiesTab : UserControl
    {
        [Inject]
        public DataTransactionsService DataTransactionsService { get; set; }
        [Inject]
        public ActivitiesService ActivitiesService { get; set; }
        [Inject]
        public TimeEntriesService TimeEntriesService { get; set; }
        [Inject]
        public CategoriesService CategoriesService { get; set; }

        private List<Activity> activities = new List<Activity>();
        private List<TimeEntry> timeEntries = new List<TimeEntry>();
        private List<Category> validCategories = new List<Category>();
        private FormHandlerService<Activity> activityFormHandler;
        private FormHandlerService<TimeEntry> timeEntryFormHandler;
        private DispatcherTimer timer;
        private DateTime startTime;
        private Activity bindedActivity;

        public ActivitiesTab()
        {
            InitializeComponent();
            this.InjectProperties();

            activityFormHandler = new FormHandlerService<Activity>("Activity", ActivityForm, DataTransactionsService, UpdateList, ValidateActivity);
            NewActivityButton.Click += activityFormHandler.New;
            EditActivityButton.Click += (object sender, RoutedEventArgs e) => activityFormHandler.Edit(ActivitiesDataGridControl.SelectedItem as Activity);
            DeleteActivityButton.Click += (object sender, RoutedEventArgs e) => activityFormHandler.Delete(ActivitiesDataGridControl.SelectedItem as Activity);
            SaveActivity.Click += activityFormHandler.Save;

            timeEntryFormHandler = new FormHandlerService<TimeEntry>("Time Entry", ActivityForm, DataTransactionsService, UpdateList, t => true);
            //EditTimeEntryButton.Click += (object sender, RoutedEventArgs e) => activityFormHandler.Edit(ActivitiesDataGridControl.SelectedItem as Activity);
            DeleteTimeEntryButton.Click += (object sender, RoutedEventArgs e) => timeEntryFormHandler.Delete(TimeEntriesDataGridControl.SelectedItem as TimeEntry);
            //SaveActivity.Click += activityFormHandler.Save;

            UpdateList();

            CategoryScope.ItemsSource = validCategories;
            ActivityPicker.SelectedValue = bindedActivity;
        }

        private void UpdateList()
        {
            Utilities.Utilities.UpdateList(activities, ActivitiesService);
            Utilities.Utilities.UpdateList(validCategories, CategoriesService.GetActivityOrAllCategories());
            Utilities.Utilities.UpdateList(timeEntries, TimeEntriesService);
            var startedActivity = TimeEntriesService.GetStartedActivity();
            SetTimerToolBar(!(startedActivity is null));
            bindedActivity = startedActivity ?? activities.FirstOrDefault();
            ActivitiesDataGridControl.ItemsSource = null;
            ActivitiesDataGridControl.ItemsSource = activities;
            TimeEntriesDataGridControl.ItemsSource = null;
            TimeEntriesDataGridControl.ItemsSource = timeEntries;
            ActivityPicker.ItemsSource = activities;
        }

        private bool ValidateActivity(Activity activity) => !string.IsNullOrWhiteSpace(activity.Name);

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => UpdateList();

        private void StartTimer_Click(object sender, RoutedEventArgs e)
        {
            var selected = ActivityPicker.SelectedItem as Activity;
            if (selected is null)
                return;

            bindedActivity = selected;
            DataTransactionsService.HandleDataTransaction(new DataTransaction()
            {
                Action = DataTransaction.ActionType.Create,
                Data = new TimeEntry()
                {
                    Activity = bindedActivity,
                    StartTime = DateTime.Now
                }
            });

            startTime = DateTime.Now;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
            UpdateList();
        }

        private void StopTimer_Click(object sender, RoutedEventArgs e)
        {
            var timeEntry = TimeEntriesService.GetStartedTimeEntry();
            if (timeEntry is null)
                throw new Exception("Time entry already has end time.");

            timeEntry.StopTime = DateTime.Now;

            DataTransactionsService.HandleDataTransaction(new DataTransaction()
            {
                Action = DataTransaction.ActionType.Edit,
                Data = timeEntry
            });

            timer.Stop();
            UpdateList();
        }

        private void SetTimerToolBar(bool timerRunning)
        {
            if(timerRunning)
            {
                StopTimer.Visibility = Visibility.Visible;
                TimerLabel.Visibility = Visibility.Visible;
                StartTimer.Visibility = Visibility.Collapsed;
                ActivityPicker.Visibility = Visibility.Collapsed;
            }
            else
            {
                StopTimer.Visibility = Visibility.Collapsed;
                TimerLabel.Visibility = Visibility.Collapsed;
                StartTimer.Visibility = Visibility.Visible;
                ActivityPicker.Visibility = Visibility.Visible;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            TimerLabel.Content = DateTime.Now.Subtract(startTime).ToString("hh\\:mm\\:ss");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (e.AddedItems[0] as ComboBoxItem).Content;
            if (selected is null)
                return;
            if(selected.Equals("Activities"))
            {
                ActivitiesDataGridControl.Visibility = Visibility.Visible;
                ActivityCrudActions.Visibility = Visibility.Visible;
                TimeEntriesDataGridControl.Visibility = Visibility.Collapsed;
                TimeEntryCrudActions.Visibility = Visibility.Collapsed;
            }
            else
            {
                ActivitiesDataGridControl.Visibility = Visibility.Collapsed;
                ActivityCrudActions.Visibility = Visibility.Collapsed;
                TimeEntriesDataGridControl.Visibility = Visibility.Visible;
                TimeEntryCrudActions.Visibility = Visibility.Visible;
            }
            UpdateList();
        }
    }
}
