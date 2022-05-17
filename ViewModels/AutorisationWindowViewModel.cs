using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class AutorisationWindowViewModel : BaseViewModel
    {
        #region Авторизация

        #region Логин авторизации

        /// <summary>Логин авторизации</summary>
        private string _ALogin;
        /// <summary>Логин авторизации</summary>
        public string ALogin
        {
            get => _ALogin;
            set => Set(ref _ALogin, value);
        }

        #endregion

        #region Пароль авторизации

        /// <summary>Пароль авторизации</summary>
        private string _APassword;
        /// <summary>Пароль авторизации</summary>
        public string APassword
        {
            get => _APassword;
            set => Set(ref _APassword, value);
        }

        #endregion

        #region Сообщение об ошибках

        /// <summary>Сообщение об ошибках</summary>
        private string _AErrorMessage;
        /// <summary>Сообщение об ошибках</summary>
        public string AErrorMessage
        {
            get => _AErrorMessage;
            set => Set(ref _AErrorMessage, value);
        }

        #endregion

        #endregion

        #region Регистрация

        #region Логин регистрации
        
        /// <summary>Логин регистрации</summary>
        private string _RLogin;
        /// <summary>Логин регистрации</summary>
        public string RLogin
        {
            get => _RLogin;
            set => Set(ref _RLogin, value);
        }

        #endregion

        #region Первый пароль регистрации

        /// <summary>Первый пароль регистрации</summary>
        private string _RFPassword;
        /// <summary>Первый пароль регистрации</summary>
        public string RFPassword
        {
            get => _RFPassword;
            set => Set(ref _RFPassword, value);
        }

        #endregion

        #region Второй пароль регистрации

        /// <summary>Второй пароль регистрации</summary>
        private string _RSPassword;
        /// <summary>Второй пароль регистрации</summary>
        public string RSPassword
        {
            get => _RSPassword;
            set => Set(ref _RSPassword, value);
        }

        #endregion

        #region Сообщение об ошибках

        /// <summary>Сообщение об ошибках</summary>
        private string _RErrorMessage;
        /// <summary>Сообщение об ошибках</summary>
        public string RErrorMessage
        {
            get => _RErrorMessage;
            set => Set(ref _RErrorMessage, value);
        }

        #endregion

        #endregion

        #region Команды

        #region Команда входа

        /// <summary>Команда входа</summary>
        public ICommand AutorisationCommand { get; }
        private bool CanAutorisationCommandExecute(object d) => true;
        private void OnAutorisationCommandExecuted(object d)
        {
            if (string.IsNullOrWhiteSpace(ALogin))
            {
                AErrorMessage = "Введите логин";
            }
            else if (string.IsNullOrWhiteSpace(APassword))
            {
                AErrorMessage = "Введите пароль";
            }
            else if (!OneStopStoreEntities.GetContext().Users.Select(x => x.userLogin).Contains(ALogin))
            {
                AErrorMessage = "Такой логин не существует";
            }
            else if (OneStopStoreEntities.GetContext().Users.Where(x => x.userLogin == ALogin).First().userPassword != APassword)
            {
                AErrorMessage = "Не правильный пароль";
            }
            else
            {
                AErrorMessage = "";
                ApplicationSPECIAL._CurrentUserId = OneStopStoreEntities.GetContext().Users.Where(x => x.userLogin == ALogin).First().userNumber;
                (d as AutorisationWindow).Close();
            }
        }

        #endregion

        #region Команда регистрации и входа 

        /// <summary>Команда регистрации и входа</summary>
        public ICommand RegistrateCommand { get; }
        private bool CanRegistrateCommandExecute(object d) => true;
        private void OnRegistrateCommandExecuted(object d)
        {
            if (string.IsNullOrWhiteSpace(RLogin))
            {
                RErrorMessage = "Введите логин";
            }
            else if (string.IsNullOrWhiteSpace(RFPassword))
            {
                RErrorMessage = "Введите пароль";
            }
            else if (RLogin.Contains(' ') || RFPassword.Contains(' '))
            {
                RErrorMessage = "Логин и пароль не должны содеражать пробелы";
            }
            else if (RLogin.Length < 6)
            {
                RErrorMessage = "Минимальный логин - 6 символов";
            }
            else if (!LoginStandart(RLogin))
            {
                RErrorMessage = "Логин должен содержать\n1 заглавную букву, 1 строчную букву и 1 цифру";
            }
            else if (RFPassword.Length < 6)
            {
                RErrorMessage = "Минимальный пароль - 6 символов";
            }
            else if (ContainsOtherSymbol(RFPassword))
            {
                RErrorMessage = "Пароль может содержать только символы латиницы и цифры";
            }
            else if (OneStopStoreEntities.GetContext().Users.Select(x => x.userLogin).Contains(RLogin))
            {
                RErrorMessage = "Такой логин уже существует";
            }
            else if (RFPassword != RSPassword)
            {
                RErrorMessage = "Пароли отличаются";
            }
            else
            {
                RErrorMessage = "";
                Users user = new Users() { userLogin = RLogin, userPassword = RFPassword, userRegistrationDate = DateTime.Now.Date };
                OneStopStoreEntities.GetContext().Users.Add(user);
                OneStopStoreEntities.GetContext().SaveChanges();
                ApplicationSPECIAL._CurrentUserId = OneStopStoreEntities.GetContext().Users.Where(x => x.userLogin == RLogin).First().userNumber;
                (d as AutorisationWindow).Close();
            }
        }

        #region Дополнительные методы для проверки логинов и паролей

        /// <summary>Проверка чистая ли строка (содержит только латинские буквы и цифры)</summary>
        /// <param name="s">Строка, которая проверяется</param>
        /// <returns>Результат проверки (true - содержит, false - нет)</returns>
        public bool ContainsOtherSymbol(string s)
        {
            foreach (char c in s)
            {
                if (!(c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9'))
                    return true;
            }
            return false;
        }

        /// <summary>Проверка подходит ли строка по "стандартам"(есть символы большие, маленькие и цифры)</summary>
        /// <param name="s">Строка, которая проверяется</param>
        /// <returns>Результат проверки (true - подходит, false - нет)</returns>
        public bool LoginStandart(string s)
        {
            bool hasBigChar = false;
            bool hasSmallChar = false;
            bool hasNum = false;
            foreach (char c in s)
            {
                if (c >= 'A' && c <= 'Z') hasBigChar = true;
                if (c >= 'a' && c <= 'z') hasSmallChar = true;
                if (c >= '0' && c <= '9') hasNum = true;
            }
            return hasBigChar && hasSmallChar && hasNum;
        }

        #endregion

        #endregion

        #endregion

        public AutorisationWindowViewModel()
        {
            AutorisationCommand = new LambdaCommand(OnAutorisationCommandExecuted, CanAutorisationCommandExecute);
            RegistrateCommand = new LambdaCommand(OnRegistrateCommandExecuted, CanRegistrateCommandExecute);
        }
    }
}
