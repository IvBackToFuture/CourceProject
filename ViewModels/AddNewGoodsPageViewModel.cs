using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class AddNewGoodsPageViewModel : BaseViewModel, IDropTarget
    {
        #region Текущий товар

        /// <summary>Текущий товар</summary>
        private Goods _CurrentGoods;
        /// <summary>Текущий товар</summary>
        public Goods CurrentGoods
        {
            get => _CurrentGoods;
            set => Set(ref _CurrentGoods, value);
        }

        #endregion

        #region Список категорий

        /// <summary>Список категорий</summary>
        private ObservableCollection<Categories> _Categories;
        /// <summary>Список категорий</summary>
        public ObservableCollection<Categories> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        #endregion

        #region Выбранная категория

        /// <summary>Выбранная категория</summary>
        private Categories _ChoosenCategory;
        /// <summary>Выбранная категория</summary>
        public Categories ChoosenCategory
        {
            get => _ChoosenCategory;
            set
            {
                Set(ref _ChoosenCategory, value);
                if (CurrentGoods.Categories != ChoosenCategory)
                {
                    CurrentGoods.Categories = ChoosenCategory;
                    CurrentGoods.goodsJson = null;
                }
            }
        }

        #endregion

        #region Страница для обновления

        /// <summary>Страница для обновления</summary>
        public static AccountPage Page { get; set; }

        #endregion

        #region Статическое свойство для передачи товара

        /// <summary>Статическое свойство для передачи товара</summary>
        public static Goods StatGoods { get; set; }

        #endregion

        public AddNewGoodsPageViewModel()
        {
            if (StatGoods == null)
                CurrentGoods = new Goods();
            else
            {
                CurrentGoods = StatGoods;
                CurrentImage = CurrentGoods.goodsPicture;
            }
            StatGoods = null;
            Categories = new ObservableCollection<Categories>(OneStopStoreEntities.GetContext().Categories);
            SetFullCharacterGoodsCommand = new LambdaCommand(OnSetFullCharacterGoodsCommandExecuted, CanSetFullCharacterGoodsCommandExecute);
            AddNewGoodsCommand = new LambdaCommand(OnAddNewGoodsCommandExecuted, CanAddNewGoodsCommandExecute);
        }

        #region Команда открытия окна установки дополнительных свойств

        /// <summary>Команда открытия окна установки дополнительных свойств</summary>
        public ICommand SetFullCharacterGoodsCommand { get; }
        private bool CanSetFullCharacterGoodsCommandExecute(object d) => true;
        private void OnSetFullCharacterGoodsCommandExecuted(object d)
        {
            ChoosingCharacteristicsForGoodsWindowViewModel.StatCurrentGoods = CurrentGoods;
            ChoosingCharacteristicsForGoodsWindow window = new ChoosingCharacteristicsForGoodsWindow();
            window.ShowDialog();
        }

        #endregion

        #region Команда добавления нового товара

        /// <summary>Команда добавления нового товара</summary>
        public ICommand AddNewGoodsCommand { get; }
        private bool CanAddNewGoodsCommandExecute(object d) => CurrentImage != null && CurrentGoods.goodsJson != null && CurrentGoods.goodsCount > 0 && CurrentGoods.goodsName?.Count() > 5 && CurrentGoods.goodsCost > 0;
        private void OnAddNewGoodsCommandExecuted(object d)
        {
            CurrentGoods.goodsPicture = CurrentImage;
            if (!(CurrentGoods.userNumber > 0))
            {
                CurrentGoods.userNumber = ApplicationSPECIAL._CurrentUserId;
                OneStopStoreEntities.GetContext().Goods.Add(CurrentGoods);
            }
            OneStopStoreEntities.GetContext().SaveChanges();
            ((AccountPageViewModel)(Page.DataContext)).CurrentUser = null;
            ((AccountPageViewModel)(Page.DataContext)).CurrentUser = OneStopStoreEntities.GetContext().Users.Find(ApplicationSPECIAL._CurrentUserId);
            (d as AddNewGoodsPage).NavigationService.GoBack();
        }

        #endregion

        #region Реализация IDropTarget

        public void DragEnter(IDropInfo dropInfo) { }

        public void DragLeave(IDropInfo dropInfo) { }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            var dataObject = dropInfo.Data as IDataObject;
            dropInfo.Effects = dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var dataObject = dropInfo.Data as DataObject;
            if (dataObject != null && dataObject.ContainsFileDropList())
            {
                var files = dataObject.GetFileDropList();
                foreach (var file in files)
                {
                    try
                    {
                        CurrentImage = File.ReadAllBytes(file);
                        break;
                    }
                    catch(Exception ex)
                    {
                        //MessageBox.Show($"{ex.Data}", $"{ex.Message}");
                    }
                }
            }
        }

        #endregion

        #region Картинка

        /// <summary>Картинка</summary>
        private byte[] _CurrentImage;
        /// <summary>Картинка</summary>
        public byte[] CurrentImage
        {
            get => _CurrentImage;
            set
            {
                if (IsValidImage(value))
                    Set(ref _CurrentImage, value);
            }
        }

        #endregion

        #region Проверка на соответствие массива байтов картинке

        /// <summary>Проверка на соответствие массива байтов картинке</summary>
        /// <param name="bytes">Массив байтов подлежащих проверке</param>
        /// <returns>Можно ли создать из массива байтов картинку</returns>
        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes)) {
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = ms;
                    imageSource.EndInit();
                    Image image = new Image() { Source = imageSource };
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
