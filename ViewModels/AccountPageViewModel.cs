using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class AccountPageViewModel : BaseViewModel
    {

        #region Текущий аккаунт
        
        private Users _CurrentUser;
        public Users CurrentUser
        {
            get => _CurrentUser;
            set => Set(ref _CurrentUser, value);
        }

        #endregion

        #region Команда добавления нового товара

        public ICommand CreateNewGoodsCommand { get; }
        private bool CanCreateNewGoodsCommandExecute(object d) => true;
        private void OnCreateNewGoodsCommandExecuted(object d)
        {
            (d as AccountPage).NavigationService.Navigate(new AddNewGoodsPage());
        }

        #endregion

        #region Команда отката заказа

        public ICommand BreakOrder { get; }
        private bool CanBreakOrderExecute(object d) => (d as Orders)?.orderStatus == 0;
        private void OnBreakOrderExecuted(object d)
        {
            Orders order = d as Orders;
            foreach(Orders_Goods og in order.Orders_Goods)
            {
                og.Goods.goodsCount += og.goodsCount;
                og.Goods.InShoppingCart = false;
            }
            OneStopStoreEntities.GetContext().Orders.Remove(order);
            OneStopStoreEntities.GetContext().SaveChanges();
            CurrentUser = null;
            CurrentUser = OneStopStoreEntities.GetContext().Users.Find(ApplicationSPECIAL._CurrentUserId);
        }

        #endregion

        #region Команда сохранения изменений

        public ICommand SaveChangesCommand { get; }
        private bool CanSaveChangesCommandExecute(object d) => true;
        private void OnSaveChangesCommandExecuted(object d)
        {
            OneStopStoreEntities.GetContext().SaveChanges();
        }

        #endregion

        #region Команда выхода из аккаунта

        public ICommand ExitAccountCommand { get; }
        private bool CanExitAccountCommandExecute(object d) => true;
        private void OnExitAccountCommandExecuted(object d)
        {
            ApplicationSPECIAL._CurrentUserId = 0;
            (d as AccountPage).NavigationService.GoBack();
        }

        #endregion

        public AccountPageViewModel()
        {
            CurrentUser = OneStopStoreEntities.GetContext().Users.Find(ApplicationSPECIAL._CurrentUserId);
            SaveChangesCommand = new LambdaCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
            ExitAccountCommand = new LambdaCommand(OnExitAccountCommandExecuted, CanExitAccountCommandExecute);
            BreakOrder = new LambdaCommand(OnBreakOrderExecuted, CanBreakOrderExecute);
            CreateNewGoodsCommand = new LambdaCommand(OnCreateNewGoodsCommandExecuted, CanCreateNewGoodsCommandExecute);
        }
    }
}
