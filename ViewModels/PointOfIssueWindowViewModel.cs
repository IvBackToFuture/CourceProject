using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System.Windows.Controls;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class PointOfIssueWindowViewModel : BaseViewModel
    {
        #region Текущая страница

        /// <summary>Текущая страница</summary>
        public Page CurrentPage { get; }

        #endregion

        #region Номер текущей точки

        /// <summary>Номер текущей точки</summary>
        public static int PointOfIssueNumber { get; set; }

        #endregion

        public PointOfIssueWindowViewModel()
        {
            CurrentPage = new AutorizationPageForPointOfIssue();
            PointOfIssueNumber = 0;
        }
    }
}
