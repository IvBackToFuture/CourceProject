using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
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
