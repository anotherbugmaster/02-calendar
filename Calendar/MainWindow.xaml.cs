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
            //TODO: get rid of magic dateString
            var dateString = "25.12.2013";
            //TODO: refactor all the mess below!
            var daysBlocks = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(Grid))
                                        .Select(index => VisualTreeHelper.GetChild(Grid, index) as TextBlock)
                                        .Where(item => item != null && item.Text == "0")
                                        .Select((item, index) => new {TextBlock = item, Index = index});

            DateTime now;
            if (dateString != null)
                now = DateTime.ParseExact(dateString, "dd.mm.yyyy", CultureInfo.InvariantCulture);
            else
                now = DateTime.Now;

            CurrentMonth.Text = now.ToString("MMMM yyyy");
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime firstDayOfYear = new DateTime(now.Year, 1, 1);

            foreach (var item in daysBlocks)
            {
                item.TextBlock.Tag = item.Index;
                var dayNumber = "";
                //TODO: fix magic constants!
                if (item.Index > (int)firstDayOfMonth.DayOfWeek - 2 && 
                    item.Index <= DateTime.DaysInMonth(now.Year, now.Month) + (int)firstDayOfMonth.DayOfWeek - 2)
                    dayNumber = (item.Index - (int)firstDayOfMonth.DayOfWeek + 2).ToString();
                item.TextBlock.Text = dayNumber;
            }

            var weeksBlocks = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(Grid))
                                        .Select(index => VisualTreeHelper.GetChild(Grid, index) as TextBlock)
                                        .Where(item => item != null && item.Text == "w")
                                        .Select((item, index) => new { TextBlock = item, Index = index });

            foreach (var item in weeksBlocks)
            {
                item.TextBlock.Tag = item.Index;
                item.TextBlock.FontStyle = FontStyles.Italic;
                var weekNumber = ((firstDayOfMonth.DayOfYear - (int)firstDayOfYear.DayOfWeek) / 7 + item.Index) % 52 + 1;
                item.TextBlock.Text = weekNumber.ToString();
            }

            //Sorry, little circle.
            Grid.SetRow(UglyCircle, (now.Day + (int)firstDayOfMonth.DayOfWeek) / 7 + 2);
            Grid.SetColumn(UglyCircle, (int)now.DayOfWeek);

            Grid.SetZIndex(UglyCircle, 0);
        }
    }
}
