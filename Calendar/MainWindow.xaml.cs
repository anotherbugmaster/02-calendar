﻿using System;
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
            string[] args = Environment.GetCommandLineArgs();
            var time = new DateTime();
            if (args.Length > 1)
                time = DateTime.ParseExact(args[1], "dd.MM.yyyy", CultureInfo.CurrentCulture);
            else
                time = DateTime.Now;
            //TODO: refactor all the mess below!
            var daysBlocks = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(Grid))
                                        .Select(index => VisualTreeHelper.GetChild(Grid, index) as TextBlock)
                                        .Where(item => item != null && item.Text == "0")
                                        .Select((item, index) => new {TextBlock = item, Index = index});

            CurrentMonth.Text = time.ToString("MMMM yyyy");
            DateTime firstDayOfMonth = new DateTime(time.Year, time.Month, 1);
            DateTime firstDayOfYear = new DateTime(time.Year, 1, 1);

            foreach (var item in daysBlocks)
            {
                item.TextBlock.Tag = item.Index;
                var dayNumber = "";
                //TODO: fix magic constants!
                if (item.Index > GetRusDayOfWeek(firstDayOfMonth.DayOfWeek) - 1 && 
                    item.Index <= DateTime.DaysInMonth(time.Year, time.Month) + GetRusDayOfWeek(firstDayOfMonth.DayOfWeek) - 1)
                    dayNumber = (item.Index - GetRusDayOfWeek(firstDayOfMonth.DayOfWeek) + 1).ToString();
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
                var weekNumber = ((firstDayOfMonth.DayOfYear - GetRusDayOfWeek(firstDayOfYear.DayOfWeek)) / 7 + item.Index) % 52 + 1;
                item.TextBlock.Text = weekNumber.ToString();
            }

            //Sorry, little circle.
            Grid.SetRow(UglyCircle, (GetRusDayOfWeek(firstDayOfMonth.DayOfWeek) + time.Day - 1) / 7 + 2);
            Grid.SetColumn(UglyCircle, GetRusDayOfWeek(time.DayOfWeek) + 1);

            Grid.SetZIndex(UglyCircle, 0);
        }

        private int GetRusDayOfWeek(DayOfWeek day)
        {
            return (7 + (int) day - 1) % 7;
        }
    }
}
