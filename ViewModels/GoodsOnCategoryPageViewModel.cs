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

        #region Статическое своство для связки Выбора-демонстрации категории товаров

        public static Categories ChoosenCategory { get; set; }

        #endregion

        public GoodsOnCategoryPageViewModel()
        {
            Category = ChoosenCategory;
            Json = (JObject)JsonConvert.DeserializeObject(Category.catJson);
        }
    }
}
