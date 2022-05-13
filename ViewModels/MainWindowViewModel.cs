using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CourceProjectMVVMAndEntityFramework.Models;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Команды

        #region Комманда возвращения на предыдущую страницу

        /// <summary>Комманда возвращения на предыдущую страницу</summary>
        public ICommand MoveBackCommand { get; }
        private bool CanMoveBackCommandExecute(object d) => d is Frame frame && frame.CanGoBack;
        private void OnMoveBackCommandExecuted(object d)
        {
            Frame frame = d as Frame;
            frame.GoBack();
        }

        #endregion

        #region Команда перехода в окну аторизации/регистрации

        public ICommand AutoRegWindowOpenCommand { get; }
        private bool CanAutoRegWindowOpenCommandExecute(object d) => true;
        private void OnAutoRegWindowOpenCommandExecuted(object d)
        {
            if (ApplicationSPECIAL._CurrentUserId == 0)
                new AutorisationWindow().ShowDialog();
            else
            {
                if ((d as Frame).Content.GetType() != typeof(AccountPage))
                    (d as Frame).Navigate(new AccountPage());
            }
        }

        #endregion

        #region Команда перехода к окну корзины пользователя

        /// <summary>Команда перехода к окну корзины пользователя</summary>
        public ICommand OpenShoppingCartCommand { get; }
        private bool CanOpenShoppingCartCommandExecute(object d) => true;
        private void OnOpenShoppingCartCommandExecuted(object d)
        {
            if ((d as Frame).Content.GetType() != typeof(ShoppingCartPage))
                (d as Frame).Navigate(new ShoppingCartPage());
        }

        #endregion

        #region Комманда поиска по наименованию

        /// <summary>Комманда поиска по наименованию</summary>
        public ICommand FindOnNameCommand { get; }
        private bool CanFindOnNameCommandExecute(object d) => true;
        private void OnFindOnNameCommandExecuted(object d)
        {
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                GoodsOnCategoryPageViewModel.ChoosenCategory = ChoosenCategory;
                GoodsOnCategoryPageViewModel._SearchStr = SearchString;
                if ((d as Frame).Content.GetType() != typeof(GoodsOnCategoryPage))
                    (d as Frame).Navigate(new GoodsOnCategoryPage());
                else
                    (((d as Frame).Content as GoodsOnCategoryPage).DataContext as GoodsOnCategoryPageViewModel).DoResearch();
                //Refresh()

                //ApplicationSPECIAL.GetApp().CurrentPage = new GoodsOnCategoryPage();
            }
        }

        #endregion

        #endregion

        #region Список категорий для ComboBox'а

        private ObservableCollection<Categories> _Categories;
        public ObservableCollection<Categories> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        #endregion

        #region Выбранная категория

        private Categories _ChoosenCategory;
        public Categories ChoosenCategory
        {
            get => _ChoosenCategory;
            set => Set(ref _ChoosenCategory, value);
        }

        #endregion

        #region Поисковая строка

        private string _SearchString;
        public string SearchString
        {
            get => _SearchString;
            set => Set(ref _SearchString, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            MoveBackCommand = new LambdaCommand(OnMoveBackCommandExecuted, CanMoveBackCommandExecute);
            FindOnNameCommand = new LambdaCommand(OnFindOnNameCommandExecuted, CanFindOnNameCommandExecute);
            AutoRegWindowOpenCommand = new LambdaCommand(OnAutoRegWindowOpenCommandExecuted, CanAutoRegWindowOpenCommandExecute);
            OpenShoppingCartCommand = new LambdaCommand(OnOpenShoppingCartCommandExecuted, CanOpenShoppingCartCommandExecute);

            #endregion

            Categories = new ObservableCollection<Categories>(OneStopStoreEntities.GetContext().Categories);
            Categories AllCategory = OneStopStoreEntities.GetContext().Categories.Create();
            AllCategory.catName = "Везде";
            Categories.Insert(0, AllCategory);
        }
    }
}
