using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using CourceProjectMVVMAndEntityFramework.Views;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class AutorizationPageForPointOfIssueViewModel : BaseViewModel
    {
        #region Вход

        #region Логин для входа

        /// <summary>Логин для входа</summary>
        private string _ILogin;
        /// <summary>Логин для входа</summary>
        public string ILogin
        {
            get => _ILogin;
            set => Set(ref _ILogin, value);
        }

        #endregion

        #region Пароль для входа

        /// <summary>Пароль для входа</summary>
        private string _IPassword;
        /// <summary>Пароль для входа</summary>
        public string IPassword
        {
            get => _IPassword;
            set => Set(ref _IPassword, value);
        }

        #endregion

        #region Сообщение для входа

        /// <summary>Сообщение для входа</summary>
        private string _IMessage;
        /// <summary>Сообщение для входа</summary>
        public string IMessage
        {
            get => _IMessage;
            set => Set(ref _IMessage, value);
        }

        #endregion

        #endregion

        #region Информация для новой точки

        #region Фамилия ответственного

        /// <summary>Фамилия ответственного</summary>
        private string _Surname;
        /// <summary>Фамилия ответственного</summary>
        public string Surname
        {
            get => _Surname;
            set => Set(ref _Surname, value);
        }

        #endregion

        #region Имя ответственного

        /// <summary>Имя ответственного</summary>
        private string _Firstname;
        /// <summary>Имя ответственного</summary>
        public string Firstname
        {
            get => _Firstname;
            set => Set(ref _Firstname, value);
        }

        #endregion

        #region Отчество ответственного

        /// <summary>Отчество ответственного</summary>
        private string _Secondname;
        /// <summary>Отчество ответственного</summary>
        public string Secondname
        {
            get => _Secondname;
            set => Set(ref _Secondname, value);
        }

        #endregion

        #region Местоположение точки

        /// <summary>Местоположение точки</summary>
        private string _Location;
        /// <summary>Местоположение точки</summary>
        public string Location
        {
            get => _Location;
            set => Set(ref _Location, value);
        }

        #endregion

        #region Сообщение для открытия новой точки

        /// <summary>Сообщение для открытия новой точки</summary>
        private string _OpenMessage;
        /// <summary>Сообщение для открытия новой точки</summary>
        public string OpenMessage
        {
            get => _OpenMessage;
            set => Set(ref _OpenMessage, value);
        }

        #endregion

        #endregion

        public AutorizationPageForPointOfIssueViewModel()
        {
            AutorizatePointOfIssueCommand = new LambdaCommand(OnAutorizatePointOfIssueCommandExecuted, CanAutorizatePointOfIssueCommandExecute);
            OpenNewPointOfIssueCommand = new LambdaCommand(OnOpenNewPointOfIssueCommandExecuted, CanOpenNewPointOfIssueCommandExecute);
        }

        #region Команда входа в аккаунт

        /// <summary>Команда входа в аккаунт</summary>
        public ICommand AutorizatePointOfIssueCommand { get; }
        private bool CanAutorizatePointOfIssueCommandExecute(object d) => true;
        private void OnAutorizatePointOfIssueCommandExecuted(object d)
        {
            if (string.IsNullOrWhiteSpace(ILogin))
            {
                IMessage = "Введите логин";
            }
            else if (string.IsNullOrWhiteSpace(IPassword))
            {
                IMessage = "Введите пароль";
            }
            else if (!OneStopStoreEntities.GetContext().PointOfIssue.Select(x => x.pointLogin).Contains(ILogin))
            {
                IMessage = "Такой логин не существует";
            }
            else if (OneStopStoreEntities.GetContext().PointOfIssue.Where(x => x.pointLogin == ILogin).First().pointPassword != IPassword)
            {
                IMessage = "Не правильный пароль";
            }
            else
            {
                IMessage = "";
                PointOfIssueWindowViewModel.PointOfIssueNumber = OneStopStoreEntities.GetContext().PointOfIssue.Where(x => x.pointLogin == ILogin).First().pointNumber;
                (d as Page).NavigationService.Navigate(new WorkPlacePointPage());
            }
        }

        #endregion

        #region Открытие новой точки

        /// <summary>Открытие новой точки</summary>
        public ICommand OpenNewPointOfIssueCommand { get; }
        private bool CanOpenNewPointOfIssueCommandExecute(object d) => true;
        private void OnOpenNewPointOfIssueCommandExecuted(object d)
        {
            if (string.IsNullOrWhiteSpace(Surname) || Surname.Length < 3 ||
                string.IsNullOrWhiteSpace(Firstname) || Firstname.Length < 3 ||
                string.IsNullOrWhiteSpace(Location) || Location.Length < 10 ||
                !(string.IsNullOrEmpty(Secondname) || Secondname.Length >= 3))
            {
                OpenMessage = "Укажите недостающие обязательные данные";
            }
            else
            {
                PointOfIssue point = new PointOfIssue() { pointOwnerSurname = Surname, pointOwnerFirstname = Firstname, pointOwnerSecondname = Secondname, pointLocation = Location };
                point.pointLogin = GenerateLogin();
                point.pointPassword = GeneratePassword();
                OneStopStoreEntities.GetContext().PointOfIssue.Add(point);
                OneStopStoreEntities.GetContext().SaveChanges();
                MessageBox.Show($"Данные для входа:\nЛогин: {point.pointLogin}\nПароль: {point.pointPassword}\nИх можно поменять в меню аккаунт", "Личный логин и пароль", MessageBoxButton.OK);
                PointOfIssueWindowViewModel.PointOfIssueNumber = point.pointNumber;
                (d as Page).NavigationService.Navigate(new WorkPlacePointPage());
            }
        }

        #endregion

        #region Генераторы логина и пароля

        /// <summary>Конвертер кириллицы в латиницу</summary>
        /// <param name="s">Строка для конвертирования</param>
        /// <returns>Результат конвертирования обрезанный до 30 символов</returns>
        private string ConverterKirInLat(string s)
        {

            StringBuilder ret = new StringBuilder();
            string[] rus = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й",

                      "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц",

                      "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
            string[] eng = { "A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", "Y", 

                      "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "KH", "TS",

                      "CH", "SH", "SHCH", null, "Y", null, "E", "YU", "YA" };

            int len = s.Length < 12 ? s.Length : 12;
            for (int j = 0; j < len; j++)
                for (int i = 0; i < rus.Length; i++)
                    if (s.Substring(j, 1).ToUpper() == rus[i])
                        ret.Append(eng[i]);

            return ret.ToString();
        }

        /// <summary>Генератор логина</summary>
        /// <returns>Возвращает логин</returns>
        private string GenerateLogin()
        {
            Random ran = new Random();
            string surname = ConverterKirInLat(Surname);
            string firstname = ConverterKirInLat(Firstname);
            string newLogin;
            do {
                newLogin = surname + firstname + ran.Next(100000, 1000000).ToString();
            } while (OneStopStoreEntities.GetContext().PointOfIssue.Select(x => x.pointLogin).Contains(newLogin));
            return newLogin;
        }

        /// <summary>Генератор пароля</summary>
        /// <returns>Возвращает пароль</returns>
        private string GeneratePassword()
        {
            Random ran = new Random();
            char[] password = new char[20];
            for (int i = 0; i < 20; i++)
            {
                if ((i + 1) % 7 == 0)
                    password[i] = '-';
                else
                {
                    int j = ran.Next(3);
                    if (j == 0) password[i] = (char)(48 + ran.Next(10));
                    else if (j == 1) password[i] = (char)(65 + ran.Next(26));
                    else if (j == 2) password[i] = (char)(97 + ran.Next(26));
                }
            }
            return new string(password);
        }

        #endregion
    }
}
