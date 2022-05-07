using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
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
using System.Windows.Controls;
using System.Windows.Input;

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

        #region Словарь для поиска по свойствам для JSON

        Dictionary<string, List<string>> PropertiesSeacrh = new Dictionary<string, List<string>>();

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
            ChangeSearchDictionaryCommand = new LambdaCommand(OnChangeSearchDictionaryCommandExecuted, CanChangeSearchDictionaryCommandExecute);
        }

        #region Команда изменения словаря по CheckBox'ам

        public ICommand ChangeSearchDictionaryCommand { get; }
        private bool CanChangeSearchDictionaryCommandExecute(object d) => true;
        private void OnChangeSearchDictionaryCommandExecuted(object d)
        {
            CheckBox CurrentCheckBox = d as CheckBox;
            string value = (CurrentCheckBox.DataContext as JValue).ToString();
            string property = ((CurrentCheckBox.DataContext as JValue).Parent.Parent as JProperty).Name.ToString();
            //System.Windows.MessageBox.Show(((CurrentCheckBox.DataContext as JValue).Parent.Parent as JProperty).Name.ToString());
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
            
            foreach (string keys in PropertiesSeacrh.Keys)
            {
                System.Diagnostics.Trace.WriteLine(keys);
                foreach(string values in PropertiesSeacrh[keys])
                {
                    System.Diagnostics.Trace.WriteLine($"   {values}");
                }
            }
            System.Diagnostics.Trace.WriteLine("");
        }

        #endregion

        public void ResearhGoods()
        {
            List<Goods> result = Goods.ToList();
            foreach(var key in PropertiesSeacrh.Keys)
            {
                List<Goods> search = Goods.ToList();
                foreach(var val in PropertiesSeacrh[key])
                {
                    
                }
            }
        }
    }
}
