using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    /// <summary>Конвертер для JSON</summary>
    class ConverterForJSONCharacters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new JValue(value);
        }
    }
}
