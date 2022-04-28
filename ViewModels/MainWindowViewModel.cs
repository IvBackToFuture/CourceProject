using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using CourceProjectMVVMAndEntityFramework.Models;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Команды

        #region Комманда возвращения на предыдущую страницу

        /// <summary>Комманда возвращения на предыдущую страницу</summary>
        public ICommand MoveBackCommand { get; }
        private bool CanMoveBackCommandExecute(object d) => d is Frame frame && frame.CanGoBack;
        private void OnMoveBackCommandExecuted(object d)
        {
            Frame frame = d as Frame;
            frame.GoBack();
        }

        #endregion

        #endregion

        #region Список категорий для ComboBox'а

        private ObservableCollection<Categories> _Categories;
        public ObservableCollection<Categories> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        #endregion

        #region Выбранная категория

        private Categories _ChoosenCategory;
        public Categories ChoosenCategory
        {
            get => _ChoosenCategory;
            set => Set(ref _ChoosenCategory, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            MoveBackCommand = new LambdaCommand(OnMoveBackCommandExecuted, CanMoveBackCommandExecute);

            #endregion

            Categories = new ObservableCollection<Categories>(OneStopStoreEntities.GetContext().Categories);
            Categories AllCategory = OneStopStoreEntities.GetContext().Categories.Create();
            AllCategory.catName = "Везде";
            Categories.Insert(0, AllCategory);
        }
    }
}
