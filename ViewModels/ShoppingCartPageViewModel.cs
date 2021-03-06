using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class ShoppingCartPageViewModel : BaseViewModel
    {
        #region Список товаров в корзине

        /// <summary>Список товаров в корзине</summary>
        private ObservableCollection<Goods> _Goods;
        /// <summary>Список товаров в корзине</summary>
        public ObservableCollection<Goods> Goods
        {
            get => _Goods;
            set => Set(ref _Goods, value);
        }

        #endregion

        #region Итоговая стоимость

        /// <summary>Итоговая стоимость</summary>
        private double _FinalPrice;
        /// <summary>Итоговая стоимость</summary>
        public double FinalPrice
        {
            get => _FinalPrice;
            set => Set(ref _FinalPrice, value);
        }

        #endregion

        #region Сообщение для пользователя

        /// <summary>Сообщение для пользователя</summary>
        private string _Message;
        /// <summary>Сообщение для пользователя</summary>
        public string Message
        {
            get => _Message;
            set => Set(ref _Message, value);
        }

        #endregion

        #region Список пунктов доставки

        /// <summary>Список пунктов доставки</summary>
        private ObservableCollection<PointOfIssue> _Points;
        /// <summary>Список пунктов доставки</summary>
        public ObservableCollection<PointOfIssue> Points
        {
            get => _Points;
            set => Set(ref _Points, value);
        }

        #endregion

        #region Выбранный пункт доставки

        /// <summary>Выбранный пункт доставки</summary>
        private PointOfIssue _ChosenPoint;
        /// <summary>Выбранный пункт доставки</summary>
        public PointOfIssue ChoosenPoint
        {
            get => _ChosenPoint;
            set => Set(ref _ChosenPoint, value);
        }

        #endregion

        #region Статическое свойство для подтверждения заказа

        /// <summary>Статическое свойство для подтверждения заказа</summary>
        public static bool CanPay { get; set; }

        #endregion

        public ShoppingCartPageViewModel()
        {
            List<Goods> AllGoods = OneStopStoreEntities.GetContext().Goods.ToList();
            Goods = new ObservableCollection<Goods>(AllGoods.Where(x => x.InShoppingCart));
            Points = new ObservableCollection<PointOfIssue>(OneStopStoreEntities.GetContext().PointOfIssue);
            FinalPrice = Goods.Sum(x => x.goodsCost * x.CountInShoppingCart);

            AddCountGoodsInShoppingCart = new LambdaCommand(OnAddCountGoodsInShoppingCartExecuted, CanAddCountGoodsInShoppingCartExecute);
            RemoveCountGoodsInShoppingCart = new LambdaCommand(OnRemoveCountGoodsInShoppingCartExecuted, CanRemoveCountGoodsInShoppingCartExecute);
            DeleteGoodsFromShoppingCart = new LambdaCommand(OnDeleteGoodsFromShoppingCartExecuted, CanDeleteGoodsFromShoppingCartExecute);
            CreateOrder = new LambdaCommand(OnCreateOrderExecuted, CanCreateOrderExecute);
        }

        #region Команда увеличения кол-ва товара на единицу

        /// <summary>Команда увеличения кол-ва товара на единицу</summary>
        public ICommand AddCountGoodsInShoppingCart { get; }
        private bool CanAddCountGoodsInShoppingCartExecute(object d) => (d as Goods)?.CountInShoppingCart < (d as Goods)?.goodsCount;
        private void OnAddCountGoodsInShoppingCartExecuted(object d)
        {
            (d as Goods).CountInShoppingCart += 1;
            FinalPrice += (d as Goods).goodsCost;
        }

        #endregion

        #region Команда уменьшения кол-ва товара на единицу

        /// <summary>Команда уменьшения кол-ва товара на единицу</summary>
        public ICommand RemoveCountGoodsInShoppingCart { get; }
        private bool CanRemoveCountGoodsInShoppingCartExecute(object d) => (d as Goods)?.CountInShoppingCart > 1;
        private void OnRemoveCountGoodsInShoppingCartExecuted(object d)
        {
            (d as Goods).CountInShoppingCart -= 1;
            FinalPrice -= (d as Goods).goodsCost;
        }

        #endregion

        #region Команда удаления товара из корзины

        /// <summary>Команда удаления товара из корзины</summary>
        public ICommand DeleteGoodsFromShoppingCart { get; }
        private bool CanDeleteGoodsFromShoppingCartExecute(object d) => (d as Goods)?.InShoppingCart ?? true;
        private void OnDeleteGoodsFromShoppingCartExecuted(object d)
        {
            Goods goods = d as Goods;
            FinalPrice -= goods.goodsCost * goods.CountInShoppingCart;
            goods.InShoppingCart = false;
            Goods.Remove(goods);
        }

        #endregion

        #region Команда оформления заказа

        /// <summary>Команда оформления заказа</summary>
        public ICommand CreateOrder { get; }
        private bool CanCreateOrderExecute(object d) => Goods.Count() > 0;
        private void OnCreateOrderExecuted(object d)
        {
            if (ApplicationSPECIAL._CurrentUserId == 0)
            {
                Message = "Требуется осуществить вход в аккаунт";
            }
            else
            {
                Users user = OneStopStoreEntities.GetContext().Users.Find(ApplicationSPECIAL._CurrentUserId);
                if (string.IsNullOrWhiteSpace(user.userSurname) ||
                    string.IsNullOrWhiteSpace(user.userFirstname) ||
                    string.IsNullOrWhiteSpace(user.userLocation) ||
                    string.IsNullOrWhiteSpace(user.userPhone) ||
                    string.IsNullOrWhiteSpace(user.userMail))
                    Message = "Требуется заполнить обязательные поля на вкладке аккаунт";
                else
                {
                    new PaymentFormWindow().ShowDialog();
                    if (CanPay)
                    {
                        Orders currentOrder = new Orders();
                        currentOrder.orderDate = DateTime.Now.Date;
                        currentOrder.buyerNumber = ApplicationSPECIAL._CurrentUserId;
                        currentOrder.orderStatus = 0;
                        currentOrder.PointOfIssue = ChoosenPoint;
                        foreach (Goods goods in Goods.ToList())
                        {
                            currentOrder.Orders_Goods.Add(new Orders_Goods { goodsNumber = goods.goodsNumber, goodsCount = goods.CountInShoppingCart, orderNumber = currentOrder.orderNumber });
                            goods.goodsCount -= goods.CountInShoppingCart;
                            goods.InShoppingCart = false;
                            Goods.Remove(goods);
                        }
                        OneStopStoreEntities.GetContext().Orders.Add(currentOrder);
                        OneStopStoreEntities.GetContext().SaveChanges();
                        Message = $"Покупка завершена. Заказ был отправлен по адресу: {ChoosenPoint}";
                        CanPay = false;
                    }
                    else
                    {
                        Message = "Покупка не была окончена";
                    }
                }
            }
        }
      
        #endregion
    }
}
