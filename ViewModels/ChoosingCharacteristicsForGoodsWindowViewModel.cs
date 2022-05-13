using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class ChoosingCharacteristicsForGoodsWindowViewModel : BaseViewModel
    {
        #region Выбранный товар

        private Goods _CurrentGoods;
        public Goods CurrentGoods
        {
            get => _CurrentGoods;
            set => Set(ref _CurrentGoods, value);
        }

        #endregion

        #region Статическое свойство выбранного товара для связки с добавлением/изменением товара

        public static Goods StatCurrentGoods;

        #endregion

        public ChoosingCharacteristicsForGoodsWindowViewModel()
        {
            CurrentGoods = StatCurrentGoods;
        }
    }
}
