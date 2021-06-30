using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ApoUI
{
    /// <summary>
    /// Converts list of int to LiveChart's series collection
    /// </summary>
    public class ListToSeries : BaseConverter<ListToSeries>
    {
        protected ChartValues<int> ChartValues;
        protected ColumnSeries ColumnSeries;
        protected SeriesCollection Series;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SeriesCollection();
            //var list = (List<int>)value;
            var list = (int[])value;
            ChartValues = new ChartValues<int>();
            for (int i = 0; i < 256; i++)
            {
                ChartValues.Add(list[i]);
            }
            //foreach (int item in list)
            //{
            //    ChartValues.Add(item);
            //}            
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
