using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class InfoAboutGoodsPageViewModel : BaseViewModel
    {
        #region Выбранный товар

        private Goods _ChoosenGoods;
        public Goods ChoosenGoods
        {
            get => _ChoosenGoods;
            set => Set(ref _ChoosenGoods, value);
        }

        #endregion

        #region Статическое свойство для передачи товара

        public static Goods StatGoods { get; set; }

        #endregion

        public InfoAboutGoodsPageViewModel()
        {
            ChoosenGoods = StatGoods;
            AddToShoppingCart = new LambdaCommand(OnAddToShoppingCartExecuted, CanAddToShoppingCartExecute);
        }

        #region Команда добавления в корзину

        public ICommand AddToShoppingCart { get; }
        private bool CanAddToShoppingCartExecute(object d) => !ChoosenGoods.InShoppingCart;
        private void OnAddToShoppingCartExecuted(object d)
        {
            ChoosenGoods.InShoppingCart = true;
            ChoosenGoods.CountInShoppingCart += 1;
        }

        #endregion
    }
}
