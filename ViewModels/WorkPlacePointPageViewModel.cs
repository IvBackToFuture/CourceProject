using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Word = Microsoft.Office.Interop.Word;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class WorkPlacePointPageViewModel : BaseViewModel
    {
        #region Текущая точка

        private PointOfIssue _CurrentPoint;
        public PointOfIssue CurrentPoint
        {
            get => _CurrentPoint;
            set => Set(ref _CurrentPoint, value);
        }

        #endregion

        #region Список всех категорий

        private ObservableCollection<Categories> _Categories;
        public ObservableCollection<Categories> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        #endregion

        public WorkPlacePointPageViewModel()
        {
            Categories = new ObservableCollection<Categories>(OneStopStoreEntities.GetContext().Categories);
            CurrentPoint = OneStopStoreEntities.GetContext().PointOfIssue.Find(PointOfIssueWindowViewModel.PointOfIssueNumber);
            ExitAccountCommand = new LambdaCommand(OnExitAccountCommandExecuted, CanExitAccountCommandExecute);
            TakeOrderCommand = new LambdaCommand(OnTakeOrderCommandExecuted, CanTakeOrderCommandExecute);
            SaveChangesCommand = new LambdaCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
            DeleteCategoryCommand = new LambdaCommand(OnDeleteCategoryCommandExecuted, CanDeleteCategoryCommandExecute);
            AddNewCategoryCommand = new LambdaCommand(OnAddNewCategoryCommandExecuted, CanAddNewCategoryCommandExecute);
            GetOrderBlankCommand = new LambdaCommand(OnGetOrderBlankCommandExecuted, CanGetOrderBlankCommandExecute);
        }

        #region Команда формирования бланка заказа

        public ICommand GetOrderBlankCommand { get; }
        private bool CanGetOrderBlankCommandExecute(object d) => true;
        private void OnGetOrderBlankCommandExecuted(object d)
        {
            Orders CurrentOrder = d as Orders;

            var application = new Word.Application();
            Word.Document document = application.Documents.Add();

            Word.Paragraph firstParagraph = document.Paragraphs.Add();
            Word.Range firstRange = firstParagraph.Range;
            firstRange.Font.Bold = 1;
            firstRange.Font.Size = 24;
            firstRange.Font.Name = "Times New Roman";
            firstRange.Text = "Бланк заказа";
            firstRange.InsertParagraphAfter();

            Word.Paragraph secondParagraph = document.Paragraphs.Add();
            Word.Range secondRange = secondParagraph.Range;
            secondRange.Font.Size = 18;
            secondRange.Font.Name = "Times New Roman";
            secondRange.Text = $"Номер заказа: {CurrentOrder.orderNumber.ToString("000000")}";
            secondRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table detailsTable = document.Tables.Add(tableRange, 22 , 6); //,CurrentOrder.Orders_Goods.Count() + 2,
            detailsTable.Columns[1].Width = 50;
            detailsTable.Columns[2].Width = 60;
            detailsTable.Columns[3].Width = 150;
            detailsTable.Columns[4].Width = 50;
            detailsTable.Columns[5].Width = 70;
            detailsTable.Columns[6].Width = 80;

            detailsTable.Borders.InsideLineStyle = detailsTable.Borders.OutsideLineStyle
                = Word.WdLineStyle.wdLineStyleSingle;
            detailsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            string[] mas = new string[] { "№", "Код", "Наименование", "Кол-во штук", "Цена, руб.", "Стоимость, руб." };
            for (int i = 0; i < mas.Length; i++)
            {
                cellRange = detailsTable.Cell(1, i + 1).Range;
                cellRange.Font.Name = "Times New Roman";
                cellRange.Text = mas[i];
            }

            detailsTable.Rows[1].Range.Bold = 1;
            detailsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            dynamic[] countPriceCost = new dynamic[3] { 0, 0, 0 }; 

            List<Orders_Goods> OG = CurrentOrder.Orders_Goods.ToList();
            for (int i = 0; i < CurrentOrder.Orders_Goods.Count; i++)
            {
                var currentOrders_Goods = OG[i];

                for (int j = 0; j < 6; j++)
                {
                    cellRange = detailsTable.Cell(i + 2, j + 1).Range;
                    cellRange.Font.Name = "Times New Roman";
                    string text = string.Empty;
                    switch(j)
                    {
                        case 0:
                            text = (i + 1).ToString();
                            break;
                        case 1:
                            text = currentOrders_Goods.goodsNumber.ToString("00000000");
                            break;
                        case 2:
                            text = currentOrders_Goods.Goods.goodsName;
                            break;
                        case 3:
                            text = currentOrders_Goods.goodsCount.ToString();
                            countPriceCost[0] += currentOrders_Goods.goodsCount;
                            break;
                        case 4:
                            text = currentOrders_Goods.Goods.goodsCost.ToString("F2");
                            countPriceCost[1] += currentOrders_Goods.Goods.goodsCost;
                            break;
                        case 5:
                            text = (currentOrders_Goods.goodsCount * currentOrders_Goods.Goods.goodsCost).ToString("F2");
                            countPriceCost[2] += (currentOrders_Goods.goodsCount * currentOrders_Goods.Goods.goodsCost);
                            break;
                    }
                    cellRange.Text = text;
                }
            }

            for (int j = 0; j < 3; j++)
            {
                cellRange = detailsTable.Cell(22, j + 1).Range;
                cellRange.Borders.InsideLineStyle = cellRange.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleNone;
                if (j == 2)
                {
                    cellRange.Font.Size = 18;
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Text = "Итого";
                }
                cellRange = detailsTable.Cell(22 - 1, j + 1).Range;
                cellRange.Borders.InsideLineStyle = cellRange.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            }
            for (int j = 0; j < 3; j++)
            {
                cellRange = detailsTable.Cell(22, j + 4).Range;
                cellRange.Font.Name = "Times New Roman";
                if (j != 0)
                    cellRange.Text = countPriceCost[j].ToString("F2");
                else
                    cellRange.Text = countPriceCost[j].ToString();
                cellRange.Borders.InsideLineStyle = cellRange.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            }

            document.Paragraphs.Add();

            Word.Paragraph thirdParagraph = document.Paragraphs.Add();
            Word.Range thirdRange = thirdParagraph.Range;
            thirdRange.Font.Size = 18;
            thirdRange.Font.Name = "Times New Roman";
            thirdRange.Text = "Дата __________________ Подпись __________________";

            application.Visible = true;

        }

        #endregion

        #region Команда добавления новой категории

        public ICommand AddNewCategoryCommand { get; }
        private bool CanAddNewCategoryCommandExecute(object d) => true;
        private void OnAddNewCategoryCommandExecuted(object d)
        {
            AddNewCategoryWindowViewModel.WorkPlace = d as Page;
            new AddNewCategoryWindow().ShowDialog();
        }

        #endregion

        #region Команда удаления категории

        public ICommand DeleteCategoryCommand { get; }
        private bool CanDeleteCategoryCommandExecute(object d) => true;
        private void OnDeleteCategoryCommandExecuted(object d)
        {
            Categories cat = d as Categories;
            Categories.Remove(cat);
            OneStopStoreEntities.GetContext().Categories.Remove(cat);
            OneStopStoreEntities.GetContext().SaveChanges();
        }

        #endregion

        #region Команда сохранения изменений в аккаунте

        public ICommand SaveChangesCommand { get; }
        private bool CanSaveChangesCommandExecute(object d) => true;
        private void OnSaveChangesCommandExecuted(object d)
        {
            OneStopStoreEntities.GetContext().SaveChanges();
        }

        #endregion

        #region Команда выхода из точки

        public ICommand ExitAccountCommand { get; }
        private bool CanExitAccountCommandExecute(object d) => true;
        private void OnExitAccountCommandExecuted(object d)
        {
            (d as Page).NavigationService.GoBack();
        }

        #endregion

        #region Команда принятия заказа

        public ICommand TakeOrderCommand { get; }
        private bool CanTakeOrderCommandExecute(object d) => (d as Orders)?.orderStatus == 0;
        private void OnTakeOrderCommandExecuted(object d)
        {
            (d as Orders).orderStatus = 1;
            OneStopStoreEntities.GetContext().SaveChanges();
        }

        #endregion
    }
}
