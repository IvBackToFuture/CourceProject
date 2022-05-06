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
using System.Windows.Shapes;

namespace CourceProjectMVVMAndEntityFramework.Views
{
    /// <summary>
    /// Логика взаимодействия для AutorisationWindow.xaml
    /// </summary>
    public partial class AutorisationWindow : Window
    {
        public AutorisationWindow()
        {
            InitializeComponent();
        }

        private void APassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).APassword = APassword.Password;
        }

        private void RFPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).RFPassword = RFPassword.Password;
        }

        private void RSPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).RSPassword = RSPassword.Password;
        }
    }
}
