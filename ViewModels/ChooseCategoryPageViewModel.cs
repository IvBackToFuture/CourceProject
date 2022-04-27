using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CourceProjectMVVMAndEntityFramework.Views;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class ChooseCategoryPageViewModel : BaseViewModel
    {
        #region Список всех категорий

        ///<summary>Список всех категорий</summary>
        private ObservableCollection<Categories> _Categories;
        ///<summary>Список всех категорий</summary>
        public ObservableCollection<Categories> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        #endregion

        #region Команды

        #region Команда перехода к товарам выбранной категории

        public ICommand ShowGoodsOnChoosenCategoryCommand { get; }
        private bool CanShowGoodsOnChoosenCategoryCommandExecute(object d) => true;
        private void OnShowGoodsOnChoosenCategoryCommandExecuted(object d)
        {
            GoodsOnCategoryPageViewModel.ChoosenCategory = (Categories)d;
            ApplicationSPECIAL.GetApp().CurrentPage = new GoodsOnCategoryPage();
        }

        #endregion

        #endregion

        public ChooseCategoryPageViewModel()
        {
            #region Команды

            #region Команда перехода к товарам выбранной категории

            ShowGoodsOnChoosenCategoryCommand = new LambdaCommand(OnShowGoodsOnChoosenCategoryCommandExecuted, CanShowGoodsOnChoosenCategoryCommandExecute);

            #endregion

            #endregion

            Categories = new ObservableCollection<Categories>(OneStopStoreEntities.GetContext().Categories);
        }
    }
}
