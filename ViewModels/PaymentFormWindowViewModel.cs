using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class PaymentFormWindowViewModel : BaseViewModel
    {
        #region Номер карты

        private ulong _CardNumber;
        public ulong CardNumber
        {
            get => _CardNumber;
            set => Set(ref _CardNumber, value);
        }

        #endregion

        #region Дата окончания работы

        private DateTime _EndDate;
        public DateTime EndDate
        {
            get => _EndDate;
            set => Set(ref _EndDate, value);
        }

        #endregion

        #region CVV код

        private int _CVVCode;
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
