using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourceProjectMVVMAndEntityFramework.Views
{
    /// <summary>
    /// Логика взаимодействия для GoodsOnCategoryPage.xaml
    /// </summary>
    public partial class GoodsOnCategoryPage : Page
    {
        public GoodsOnCategoryPage()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string str = (sender as TextBox).Text;
            if (!( e.Text[0] >= '0' && e.Text[0] <= '9' || e.Text[0] == '.' && str.Count() > 0 && !str.Contains('.')))
                e.Handled = true;
        }
    }
}
