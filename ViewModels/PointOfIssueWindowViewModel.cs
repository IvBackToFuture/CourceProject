using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class PointOfIssueWindowViewModel : BaseViewModel
    {
        #region Текущая страница

        public Page CurrentPage { get; }

        #endregion

        #region Номер текущей точки

        public static int PointOfIssueNumber { get; set; }

        #endregion

        public PointOfIssueWindowViewModel()
        {
            CurrentPage = new AutorizationPageForPointOfIssue();
            PointOfIssueNumber = 0;
        }
    }
}
