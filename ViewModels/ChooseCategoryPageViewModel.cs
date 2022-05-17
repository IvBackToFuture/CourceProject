using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System.Collections.ObjectModel;
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

        #region Команда перехода к товарам выбранной категории

        /// <summary>Команда перехода к товарам выбранной категории</summary>
        public ICommand ShowGoodsOnChoosenCategoryCommand { get; }
        private bool CanShowGoodsOnChoosenCategoryCommandExecute(object d) => true;
        private void OnShowGoodsOnChoosenCategoryCommandExecuted(object d)
        {
            GoodsOnCategoryPageViewModel.ChoosenCategory = (Categories)d;
            ApplicationSPECIAL.GetApp().CurrentPage = new GoodsOnCategoryPage();
        }

        #endregion

        public ChooseCategoryPageViewModel()
        {
            ShowGoodsOnChoosenCategoryCommand = new LambdaCommand(OnShowGoodsOnChoosenCategoryCommandExecuted, CanShowGoodsOnChoosenCategoryCommandExecute);

            Categories = new ObservableCollection<Categories>(OneStopStoreEntities.GetContext().Categories);
        }
    }
}
