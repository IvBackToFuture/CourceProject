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

        private Page _CurrentPage;
        public Page CurrentPage
        {
            get => _CurrentPage;
            set => Set(ref _CurrentPage, value);
        }

        #endregion

        public ApplicationSPECIAL()
        {
            CurrentPage = new ChooseCategoryPage();
            SPECIAL = this;
        }

        #region Поле и метод для получения объекта данного класса

        private static ApplicationSPECIAL SPECIAL;
        public static ApplicationSPECIAL GetApp()
        {
            return SPECIAL;
        }

        #endregion
    }
}
