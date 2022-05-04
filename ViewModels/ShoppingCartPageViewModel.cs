using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class ShoppingCartPageViewModel : BaseViewModel
    {
        #region Список товаров в корзине

        private ObservableCollection<Goods> _Goods;
        public ObservableCollection<Goods> Goods
        {
            get => _Goods;
            set => Set(ref _Goods, value);
        }

        #endregion

        public ShoppingCartPageViewModel() {
            //Dictionary<int,int> shopCart = ApplicationSPECIAL.ShoppingCart;
            //List<Goods> 
        }
    }
}
