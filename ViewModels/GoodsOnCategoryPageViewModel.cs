using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class GoodsOnCategoryPageViewModel : BaseViewModel
    {
        #region Выбранная категория для View

        private Categories _Category;
        public Categories Category
        {
            get => _Category;
            set => Set(ref _Category, value);
        }

        #endregion

        #region Свойства категории для поиска

        private JObject _Json;
        public JObject Json
        {
            get => _Json;
            set => Set(ref _Json, value);
        }

        #endregion

        #region Список товаров

        private ObservableCollection<Goods> _Goods;
        public ObservableCollection<Goods> Goods
        {
            get => _Goods;
            set => Set(ref _Goods, value);
        }

        #endregion

        #region Статическое своство для связки Выбора-демонстрации категории товаров

        public static Categories ChoosenCategory { get; set; }

        #endregion

        #region Статическое свойство для связки поисковой строки и демонстрации товаров 

        public static string _SearchStr { get; set; }

        #endregion

        public GoodsOnCategoryPageViewModel()
        {
            string SearchStr = _SearchStr ?? "";
            Category = ChoosenCategory;
            if (Category.catJson != null)
            {
                Goods = new ObservableCollection<Goods>(OneStopStoreEntities.GetContext().Goods.Where(
                    x => x.catNumber == Category.catNumber && x.goodsName.ToLower().Contains(SearchStr)));
                Json = (JObject)JsonConvert.DeserializeObject(Category.catJson);
            }
            else
            {
                Goods = new ObservableCollection<Goods>(OneStopStoreEntities.GetContext().Goods.Where(
                    x => x.goodsName.ToLower().Contains(SearchStr)));
            }
        }
    }
}
