using System;
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
            var datesBlocks = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(Grid))
                                        .Select(index => VisualTreeHelper.GetChild(Grid, index) as TextBlock)
                                        .Where(item => item != null && item.Text == "0")
                                        .Select((item, index) => new {TextBlock = item, Index = index});
            foreach (var item in datesBlocks)
            {
                item.TextBlock.Tag = item.Index;
                item.TextBlock.Text = (item.Index + 1).ToString();
            }
            DateTime date = DateTime.Now;
            CurrentMonth.Text = date.ToString("MMMM yyyy");
        }
    }
}
