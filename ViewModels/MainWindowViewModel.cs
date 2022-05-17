using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CourceProjectMVVMAndEntityFramework.Models;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        
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

        /// <summary>Команда перехода в окну аторизации/регистрации</summary>
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

        #region Список категорий для ComboBox'а

        /// <summary>Список категорий для ComboBox'а</summary>
        private ObservableCollection<Categories> _Categories;
        /// <summary>Список категорий для ComboBox'а</summary>
        public ObservableCollection<Categories> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        #endregion

        #region Выбранная категория

        /// <summary>Выбранная категория</summary>
        private Categories _ChoosenCategory;
        /// <summary>Выбранная категория</summary>
        public Categories ChoosenCategory
        {
            get => _ChoosenCategory;
            set => Set(ref _ChoosenCategory, value);
        }

        #endregion

        #region Поисковая строка

        /// <summary>Поисковая строка</summary>
        private string _SearchString;
        /// <summary>Поисковая строка</summary>
        public string SearchString
        {
            get => _SearchString;
            set => Set(ref _SearchString, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            MoveBackCommand = new LambdaCommand(OnMoveBackCommandExecuted, CanMoveBackCommandExecute);
            FindOnNameCommand = new LambdaCommand(OnFindOnNameCommandExecuted, CanFindOnNameCommandExecute);
            AutoRegWindowOpenCommand = new LambdaCommand(OnAutoRegWindowOpenCommandExecuted, CanAutoRegWindowOpenCommandExecute);
            OpenShoppingCartCommand = new LambdaCommand(OnOpenShoppingCartCommandExecuted, CanOpenShoppingCartCommandExecute);

            Categories = new ObservableCollection<Categories>(OneStopStoreEntities.GetContext().Categories);
            Categories AllCategory = OneStopStoreEntities.GetContext().Categories.Create();
            AllCategory.catName = "Везде";
            Categories.Insert(0, AllCategory);
        }
    }
}
