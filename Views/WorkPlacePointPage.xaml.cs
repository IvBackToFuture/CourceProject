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
    /// Логика взаимодействия для WorkPlacePointPage.xaml
    /// </summary>
    public partial class WorkPlacePointPage : Page
    {
        public WorkPlacePointPage()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(e.Text[0] >= 'а' && e.Text[0] <= 'я' || e.Text[0] >= 'А' && e.Text[0] <= 'Я'))
                e.Handled = true;
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            string str = (sender as TextBox).Text;
            if (!(e.Text[0] >= 'а' && e.Text[0] <= 'я' || e.Text[0] >= 'А' && e.Text[0] <= 'Я' ||
                !(e.Text[0] >= '0' && e.Text[0] <= '9') ||
                !(!str.Contains('.') && e.Text[0] == '.' && str.Trim().Count() > 0) ||
                !(e.Text[0] == ' ')
                ))
                e.Handled = true;
        }

        private void TextBox_PreviewTextInput_2(object sender, TextCompositionEventArgs e)
        {
            string str = (sender as TextBox).Text;
            if (!(e.Text[0] >= 'а' && e.Text[0] <= 'я' || e.Text[0] >= 'А' && e.Text[0] <= 'Я' ||
                e.Text[0] >= '0' && e.Text[0] <= '9'))
                e.Handled = true;
        }
    }
}
