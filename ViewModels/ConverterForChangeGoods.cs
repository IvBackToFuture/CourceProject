using CourceProjectMVVMAndEntityFramework.Models;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    /// <summary>Конвертер для товаров</summary>
    class ConverterForChangeGoods : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object val = new { Goods = values[0] as Goods, Page = values[1] as Page };
            return val;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return (object[])value;
        }
    }
}
