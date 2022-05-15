using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class WorkPlacePointPageViewModel : BaseViewModel
    {
        #region Текущая точка

        private PointOfIssue _CurrentPoint;
        public PointOfIssue CurrentPoint
        {
            get => _CurrentPoint;
            set => Set(ref _CurrentPoint, value);
        }

        #endregion

        public WorkPlacePointPageViewModel()
        {
            CurrentPoint = OneStopStoreEntities.GetContext().PointOfIssue.Find(PointOfIssueWindowViewModel.PointOfIssueNumber);
            ExitAccountCommand = new LambdaCommand(OnExitAccountCommandExecuted, CanExitAccountCommandExecute);
            TakeOrderCommand = new LambdaCommand(OnTakeOrderCommandExecuted, CanTakeOrderCommandExecute);
            SaveChangesCommand = new LambdaCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
        }

        #region Команда сохранения изменений в аккаунте

        public ICommand SaveChangesCommand { get; }
        private bool CanSaveChangesCommandExecute(object d) => true;
        private void OnSaveChangesCommandExecuted(object d)
        {
            OneStopStoreEntities.GetContext().SaveChanges();
        }

        #endregion

        #region Команда выхода из точки

        public ICommand ExitAccountCommand { get; }
        private bool CanExitAccountCommandExecute(object d) => true;
        private void OnExitAccountCommandExecuted(object d)
        {
            (d as Page).NavigationService.GoBack();
        }

        #endregion

        #region Команда принятия заказа

        public ICommand TakeOrderCommand { get; }
        private bool CanTakeOrderCommandExecute(object d) => (d as Orders)?.orderStatus == 0;
        private void OnTakeOrderCommandExecuted(object d)
        {
            (d as Orders).orderStatus = 1;
            OneStopStoreEntities.GetContext().SaveChanges();
        }

        #endregion
    }
}
