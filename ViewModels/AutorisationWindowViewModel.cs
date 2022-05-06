using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class AutorisationWindowViewModel : BaseViewModel
    {
        #region Логин авторизации

        private string _ALogin;
        public string ALogin
        {
            get => _ALogin;
            set => Set(ref _ALogin, value);
        }

        #endregion

        #region Пароль авторизации

        private string _APassword;
        public string APassword
        {
            get => _APassword;
            set => Set(ref _APassword, value);
        }

        #endregion

        #region Логин регистрации

        private string _RLogin;
        public string RLogin
        {
            get => _RLogin;
            set => Set(ref _RLogin, value);
        }

        #endregion
      
        #region Первый пароль регистрации

        private string _RFPassword;
        public string RFPassword
        {
            get => _RFPassword;
            set => Set(ref _RFPassword, value);
        }

        #endregion

        #region Второй пароль регистрации

        private string _RSPassword;
        public string RSPassword
        {
            get => _RSPassword;
            set => Set(ref _RSPassword, value);
        }

        #endregion


    }
}
