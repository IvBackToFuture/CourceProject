using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class GoodsOnCategoryPageViewModel : BaseViewModel
    {

        #region Минимальная и максимальная цены

        /// <summary>Минимальная цена</summary>
        private double _MinPrice;
        /// <summary>Минимальная цена</summary>
        public double MinPrice
        {
            get => _MinPrice;
            set
            {
                Set(ref _MinPrice, value);
                CostWasChanged = true;
            }
        }

        /// <summary>Максимальная цена</summary>
        private double _MaxPrice;
        /// <summary>Максимальная цена</summary>
        public double MaxPrice
        {
            get => _MaxPrice;
            set
            {
                Set(ref _MaxPrice, value);
                CostWasChanged = true;
            }
        }

        /// <summary>Были ли изменены максимальная и минимальная цены</summary>
        private bool CostWasChanged;

        #endregion

        #region Выбранная категория для View

        /// <summary>Выбранная категория для View</summary>
        private Categories _Category;
        /// <summary>Выбранная категория для View</summary>
        public Categories Category
        {
            get => _Category;
            set => Set(ref _Category, value);
        }

        #endregion

        #region Список товаров для страницы

        /// <summary>Список товаров для страницы</summary>
        private List<Goods> GoodsForPage;

        #endregion

        #region Список товаров для отображения

        /// <summary>Список товаров для отображения</summary>
        private ObservableCollection<Goods> _Goods;
        /// <summary>Список товаров для отображения</summary>
        public ObservableCollection<Goods> Goods
        {
            get => _Goods;
            set => Set(ref _Goods, value);
        }

        #endregion

        #region Словарь для поиска по свойствам для JSON

        /// <summary>Словарь для поиска по свойствам для JSON</summary>
        Dictionary<string, List<string>> PropertiesSeacrh = new Dictionary<string, List<string>>();

        #endregion

        #region Статическое своство для связки Выбора-демонстрации категории товаров

        /// <summary>Статическое своство для связки Выбора-демонстрации категории товаров</summary>
        public static Categories ChoosenCategory { get; set; }

        #endregion

        #region Статическое свойство для связки поисковой строки и демонстрации товаров 

        /// <summary>Статическое свойство для связки поисковой строки и демонстрации товаров</summary>
        public static string _SearchStr { get; set; }

        #endregion

        public GoodsOnCategoryPageViewModel()
        {
            string SearchStr = _SearchStr?.ToLower() ?? "";
            Category = ChoosenCategory;

            if (Category.catJson != null)
            {
                GoodsForPage = OneStopStoreEntities.GetContext().Goods
                    .Where(x => x.catNumber == Category.catNumber
                    && x.goodsName.ToLower().Contains(SearchStr) && x.goodsCount > 0).ToList();
            }
            else
            {
                GoodsForPage = OneStopStoreEntities.GetContext().Goods
                    .Where(x => x.goodsName.ToLower().Contains(SearchStr) && x.goodsCount > 0).ToList();
            }
            if (GoodsForPage.Count > 0)
            {
                MinPrice = GoodsForPage.Min(x => x.goodsCost);
                MaxPrice = GoodsForPage.Max(x => x.goodsCost);
            }

            Goods = new ObservableCollection<Goods>(GoodsForPage);

            ChangeSearchDictionaryCommand = new LambdaCommand(OnChangeSearchDictionaryCommandExecuted, CanChangeSearchDictionaryCommandExecute);
            AddToShoppingCart = new LambdaCommand(OnAddToShoppingCartExecuted, CanAddToShoppingCartExecute);
            ShowGoodsCharactersCommand = new LambdaCommand(OnShowGoodsCharactersCommandExecuted, CanShowGoodsCharactersCommandExecute);
            FindOnCostCommand = new LambdaCommand(OnFindOnCostCommandExecuted, CanFindOnCostCommandExecute);
        }

        #region Метод повторного поиска

        /// <summary>Метод повторного поиска</summary>
        public void DoResearch()
        {
            string SearchStr = _SearchStr?.ToLower() ?? "";
            Category = ChoosenCategory;

            if (Category.catJson != null)
            {
                GoodsForPage = OneStopStoreEntities.GetContext().Goods
                    .Where(x => x.catNumber == Category.catNumber
                    && x.goodsName.ToLower().Contains(SearchStr) && x.goodsCount > 0).ToList();
            }
            else
            {
                GoodsForPage = OneStopStoreEntities.GetContext().Goods
                    .Where(x => x.goodsName.ToLower().Contains(SearchStr) && x.goodsCount > 0).ToList();
            }
            Goods = new ObservableCollection<Goods>(GoodsForPage);
        }

        #endregion

        #region Команда открытия страницы просмотра товара

        /// <summary>Команда открытия страницы просмотра товара</summary>
        public ICommand ShowGoodsCharactersCommand { get; }
        private bool CanShowGoodsCharactersCommandExecute(object d) => true;
        private void OnShowGoodsCharactersCommandExecuted(object d)
        {
            InfoAboutGoodsPageViewModel.StatGoods = ((dynamic)d).Goods as Goods;
            (((dynamic)d).Page as GoodsOnCategoryPage).NavigationService.Navigate(new InfoAboutGoodsPage());
        }

        #endregion

        #region Команда поиска по цене

        /// <summary>Команда поиска по цене</summary>
        public ICommand FindOnCostCommand { get; }
        private bool CanFindOnCostCommandExecute(object d) => true;
        private void OnFindOnCostCommandExecuted(object d)
        {
            if (CostWasChanged)
                Goods = new ObservableCollection<Goods>(GoodsForPage.Where(x => x.goodsCost >= MinPrice && x.goodsCost <= MaxPrice));
        }

        #endregion

        #region Команда добавления в корзину

        /// <summary>Команда добавления в корзину</summary>
        public ICommand AddToShoppingCart { get; }
        private bool CanAddToShoppingCartExecute(object d) => (d is null) ? false : !(d as Goods).InShoppingCart;
        private void OnAddToShoppingCartExecuted(object d)
        {
            (d as Goods).InShoppingCart = true;
            (d as Goods).CountInShoppingCart += 1;
        }

        #endregion

        #region Команда изменения словаря по CheckBox'ам

        /// <summary>Команда изменения словаря по CheckBox'ам</summary>
        public ICommand ChangeSearchDictionaryCommand { get; }
        private bool CanChangeSearchDictionaryCommandExecute(object d) => true;
        private void OnChangeSearchDictionaryCommandExecuted(object d)
        {
            CheckBox CurrentCheckBox = d as CheckBox;
            string value = (CurrentCheckBox.DataContext as JValue).ToString();
            string property = ((CurrentCheckBox.DataContext as JValue).Parent.Parent as JProperty).Name.ToString();
            if ((bool)CurrentCheckBox.IsChecked)
            {
                if (PropertiesSeacrh.Keys.Contains(property))
                    PropertiesSeacrh[property].Add(value);
                else
                    PropertiesSeacrh.Add(property, new List<string> { value });
            }
            else
            {
                PropertiesSeacrh[property].Remove(value);
                if (PropertiesSeacrh[property].Count == 0)
                {
                    PropertiesSeacrh.Remove(property);
                }
            }
            ResearhGoods();
        }

        #endregion

        #region Метод обновления выводимого списка товаров

        /// <summary>Метод обновления выводимого списка товаров</summary>
        public void ResearhGoods()
        {
            List<Goods> result = GoodsForPage.ToList();
            foreach(var key in PropertiesSeacrh.Keys)
            {
                List<Goods> search = new List<Goods>();
                foreach (var val in PropertiesSeacrh[key])
                {
                    List<Goods> search2 = GoodsForPage.Where(x => x.JSON[key].ToString() == val).ToList();
                    if (search2.Count > 0)
                        search.AddRange(search2);
                }
                result = (from r in result
                join s in search.Distinct() on r.goodsNumber equals s.goodsNumber
                select r).ToList();
            }
            Goods = new ObservableCollection<Goods>(result);
        }

        #endregion
    }
}
