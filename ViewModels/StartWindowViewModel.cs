using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class StartWindowViewModel : BaseViewModel
    {
        public StartWindowViewModel()
        {
            MoveOnPointOfIssueWindow = new LambdaCommand(OnMoveOnPointOfIssueWindowExecuted, CanMoveOnPointOfIssueWindowExecute);
            MoveOnUserWindow = new LambdaCommand(OnMoveOnUserWindowExecuted, CanMoveOnUserWindowExecute);
        }

        #region Команда перехода к окну пользователя

        public ICommand MoveOnUserWindow { get; }
        private bool CanMoveOnUserWindowExecute(object d) => true;
        private void OnMoveOnUserWindowExecuted(object d)
        {
            new MainWindow().Show();
            (d as Window).Close();
        }

        #endregion

        #region Команда перехода к окну ответственного за точку

        public ICommand MoveOnPointOfIssueWindow { get; }
        private bool CanMoveOnPointOfIssueWindowExecute(object d) => true;
        private void OnMoveOnPointOfIssueWindowExecuted(object d)
        {
            new PointOfIssueWindow().Show();
            (d as Window).Close();
        }

        #endregion
    }
}
