using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ApoUI
{
    /// <summary>
    /// Converts (sorted) dictionary to LiveChart's series collection
    /// </summary>
    public class DictionaryToSeries : BaseConverter<DictionaryToSeries>
    {
        protected ChartValues<int> ChartValues;
        protected ColumnSeries ColumnSeries;
        protected SeriesCollection Series;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SeriesCollection();
            var dictionary = (SortedDictionary<int, int>)value;
            ChartValues = new ChartValues<int>();
            foreach (var item in dictionary)
            {
                ChartValues.Add(item.Value);
            }            
            Series = new SeriesCollection();
            Series.Add(new ColumnSeries { Values = ChartValues, ColumnPadding = 0, });
            return Series;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
