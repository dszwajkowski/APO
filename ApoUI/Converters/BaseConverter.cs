using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ApoUI
{
    public abstract class BaseConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        #region Private Members
        private static T Converter = null;
        #endregion

        #region MarkupExtension Methods

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter ?? (Converter = new T());
        }

        #endregion

        #region IValueConverter Methods

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
