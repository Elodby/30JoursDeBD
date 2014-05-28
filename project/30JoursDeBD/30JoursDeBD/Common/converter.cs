using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace _30JoursDeBD.Common
{
    public class ListViewLargerSizeFitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ListView lol = ((ListView)value);
            double val = lol.ActualWidth - lol.Margin.Left - lol.Margin.Right - lol.Padding.Left - lol.Padding.Right - 20;
            return (val < 0) ? lol.ActualWidth : val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}