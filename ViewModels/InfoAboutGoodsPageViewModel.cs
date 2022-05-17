using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class InfoAboutGoodsPageViewModel : BaseViewModel
    {
        #region Выбранный товар

        /// <summary>Выбранный товар</summary>
        private Goods _ChoosenGoods;
        /// <summary>Выбранный товар</summary>
        public Goods ChoosenGoods
        {
            get => _ChoosenGoods;
            set => Set(ref _ChoosenGoods, value);
        }

        #endregion

        #region Статическое свойство для передачи товара

        /// <summary>Статическое свойство для передачи товара</summary>
        public static Goods StatGoods { get; set; }

        #endregion

        public InfoAboutGoodsPageViewModel()
        {
            ChoosenGoods = StatGoods;
            AddToShoppingCart = new LambdaCommand(OnAddToShoppingCartExecuted, CanAddToShoppingCartExecute);
        }

        #region Команда добавления в корзину

        /// <summary>Команда добавления в корзину</summary>
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
