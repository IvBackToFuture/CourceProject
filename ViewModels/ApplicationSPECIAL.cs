using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class ApplicationSPECIAL : BaseViewModel
    {
        #region Текущая страница во фрейме

        /// <summary>Текущая страница во фрейме</summary>
        private Page _CurrentPage;
        /// <summary>Текущая страница во фрейме</summary>
        public Page CurrentPage
        {
            get => _CurrentPage;
            set => Set(ref _CurrentPage, value);
        }

        #endregion

        #region Номер текущего пользователя из бд

        /// <summary>Номер текущего пользователя из бд</summary>
        public static int _CurrentUserId { get; set; } = 0;

        #endregion

        public ApplicationSPECIAL()
        {
            CurrentPage = new ChooseCategoryPage(); //При генерации окна сгенерируется и первая страница
            SPECIAL = this;                         //Объект класса запишется в статическое поле,
                                                    //для возможности дальшейшего изменения выводимой страницы
        }

        #region Корзина для покупок

        /// <summary>Корзина для покупок. Ключ - номер товара. Значение - кол-во</summary>
        public static Dictionary<int, int> ShoppingCart = new Dictionary<int, int>();

        #endregion

        #region Поле и метод для получения объекта данного класса

        /// <summary>Поле для получения объекта данного класса</summary>
        private static ApplicationSPECIAL SPECIAL;
        /// <summary>Метод для получения объекта данного класса</summary>
        /// <returns>Возвращает поле объекта класса ApplicationSPECIAL</returns>
        public static ApplicationSPECIAL GetApp()
        {
            return SPECIAL;
        }

        #endregion
    }
}
