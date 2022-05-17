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

        /// <summary>Текущий аккаунт</summary>
        private Users _CurrentUser;
        /// <summary>Текущий аккаунт</summary>
        public Users CurrentUser
        {
            get => _CurrentUser;
            set => Set(ref _CurrentUser, value);
        }

        #endregion

        #region Команда добавления нового товара

        /// <summary>Команда добавления нового товара</summary>
        public ICommand CreateNewGoodsCommand { get; }
        private bool CanCreateNewGoodsCommandExecute(object d) => true;
        private void OnCreateNewGoodsCommandExecuted(object d)
        {
            AddNewGoodsPageViewModel.Page = d as AccountPage;
            (d as AccountPage).NavigationService.Navigate(new AddNewGoodsPage());
        }

        #endregion

        #region Команда обновления товара

        /// <summary>Команда обновления товара</summary>
        public ICommand UpdateGoodsCommand { get; }
        private bool CanUpdateGoodsCommandExecute(object d) => true;
        private void OnUpdatewGoodsCommandExecuted(object d)
        {
            AddNewGoodsPageViewModel.StatGoods = ((dynamic)d).Goods as Goods;
            AddNewGoodsPageViewModel.Page = ((dynamic)d).Page as AccountPage;
            (((dynamic)d).Page as AccountPage).NavigationService.Navigate(new AddNewGoodsPage());
        }

        #endregion

        #region Команда отката заказа

        /// <summary>Команда отката заказа</summary>
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

        #region Команда снятия с продажи товара

        /// <summary>Команда снятия с продажи товара</summary>
        public ICommand WithdrawFromSaleCommand { get; }
        private bool CanWithdrawFromSaleCommandExecute(object d) => (d as Goods)?.goodsCount > 0;
        private void OnWithdrawFromSaleCommandExecuted(object d)
        {
            Goods goods = d as Goods;
            goods.goodsCount = 0;
            OneStopStoreEntities.GetContext().SaveChanges();
            CurrentUser = null;
            CurrentUser = OneStopStoreEntities.GetContext().Users.Find(ApplicationSPECIAL._CurrentUserId);
        }

        #endregion

        #region Команда сохранения изменений

        /// <summary>Команда сохранения изменений</summary>
        public ICommand SaveChangesCommand { get; }
        private bool CanSaveChangesCommandExecute(object d) => true;
        private void OnSaveChangesCommandExecuted(object d)
        {
            OneStopStoreEntities.GetContext().SaveChanges();
        }

        #endregion

        #region Команда выхода из аккаунта

        /// <summary>Команда выхода из аккаунта</summary>
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
            UpdateGoodsCommand = new LambdaCommand(OnUpdatewGoodsCommandExecuted, CanUpdateGoodsCommandExecute);
            WithdrawFromSaleCommand = new LambdaCommand(OnWithdrawFromSaleCommandExecuted, CanWithdrawFromSaleCommandExecute);
        }
    }
}
