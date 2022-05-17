using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System;
using System.Windows;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class PaymentFormWindowViewModel : BaseViewModel
    {
        #region Номер карты

        /// <summary>Номер карты</summary>
        private ulong _CardNumber;
        /// <summary>Номер карты</summary>
        public ulong CardNumber
        {
            get => _CardNumber;
            set => Set(ref _CardNumber, value);
        }

        #endregion

        #region Дата окончания работы

        /// <summary>Дата окончания работы</summary>
        private DateTime _EndDate;
        /// <summary>Дата окончания работы</summary>
        public DateTime EndDate
        {
            get => _EndDate;
            set => Set(ref _EndDate, value);
        }

        #endregion

        #region CVV код

        /// <summary>CVV код</summary>
        private int _CVVCode;
        /// <summary>CVV код</summary>
        public int CVVCode
        {
            get => _CVVCode;
            set => Set(ref _CVVCode, value);
        }

        #endregion
    
        public PaymentFormWindowViewModel()
        {
            EndDate = DateTime.Now;
            PayCommand = new LambdaCommand(OnPayCommandExecuted, CanPayCommandExecute);
        }

        #region Команда оплаты

        /// <summary>Команда оплаты</summary>
        public ICommand PayCommand { get; }
        private bool CanPayCommandExecute(object d) => CardNumber > 0 && CVVCode > 0;
        private void OnPayCommandExecuted(object d)
        {
            ShoppingCartPageViewModel.CanPay = true;
            (d as Window).Close();
        }

        #endregion
    }
}
