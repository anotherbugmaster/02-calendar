using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeBehaviour();
        }

        private void InitializeBehaviour()
        {
            var time = ParseConsoleDate();
            DrawCalendar(time);
        }

        private int GetRusDayOfWeek(DayOfWeek day)
        {
            return (7 + (int) day - 1) % 7;
        }

        private void PlaceDateCircle(DateTime firstDayOfMonth, DateTime time)
        {
            //Sorry, little circle.
            Grid.SetRow(UglyCircle, (GetRusDayOfWeek(firstDayOfMonth.DayOfWeek) + time.Day - 1) / 7 + 2);
            Grid.SetColumn(UglyCircle, GetRusDayOfWeek(time.DayOfWeek) + 1);
            Grid.SetZIndex(UglyCircle, 0);
        }

        private void PlaceDateBlocks(DateTime firstDayOfMonth, DateTime time)
        {
            var daysBlocks = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(Grid))
                                        .Select(index => VisualTreeHelper.GetChild(Grid, index) as TextBlock)
                                        .Where(item => item != null && item.Text == "0")
                                        .Select((item, index) => new { TextBlock = item, Index = index });

            foreach (var block in daysBlocks)
            {
                block.TextBlock.Text = SetDateBlockText(block.Index, firstDayOfMonth, time);
            }
        }

        private void PlaceWeekBlocks(DateTime firstDayOfMonth, DateTime firstDayOfYear)
        {
            var weeksBlocks = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(Grid))
                                        .Select(index => VisualTreeHelper.GetChild(Grid, index) as TextBlock)
                                        .Where(item => item != null && item.Text == "w")
                                        .Select((item, index) => new { TextBlock = item, Index = index });

            foreach (var item in weeksBlocks)
            {
                item.TextBlock.FontStyle = FontStyles.Italic;
                var weekNumber = ((firstDayOfMonth.DayOfYear - GetRusDayOfWeek(firstDayOfYear.DayOfWeek)) / 7 + item.Index) % 52 + 1;
                item.TextBlock.Text = weekNumber.ToString();
            }
        }

        private void DrawCalendar(DateTime time)
        {
            CurrentMonth.Text = time.ToString("MMMM yyyy");

            DateTime firstDayOfMonth = new DateTime(time.Year, time.Month, 1);
            DateTime firstDayOfYear = new DateTime(time.Year, 1, 1);

            PlaceDateBlocks(firstDayOfMonth, time);
            PlaceWeekBlocks(firstDayOfMonth, firstDayOfYear);
            PlaceDateCircle(firstDayOfMonth, time);
        }

        private DateTime ParseConsoleDate()
        {
            string[] args = Environment.GetCommandLineArgs();
            var time = new DateTime();
            if (args.Length > 1)
                time = DateTime.ParseExact(args[1], "dd.MM.yyyy", CultureInfo.CurrentCulture);
            else
                time = DateTime.Now;
            return time;
        }

        private string SetDateBlockText(int dateBlockIndex, DateTime firstDayOfMonth, DateTime time)
        {
            var dayNumber = "";

            if (dateBlockIndex > GetRusDayOfWeek(firstDayOfMonth.DayOfWeek) - 1 &&
                dateBlockIndex <= DateTime.DaysInMonth(time.Year, time.Month) + GetRusDayOfWeek(firstDayOfMonth.DayOfWeek) - 1)
                dayNumber = (dateBlockIndex - GetRusDayOfWeek(firstDayOfMonth.DayOfWeek) + 1).ToString();
            return dayNumber;
        }
    }
}
