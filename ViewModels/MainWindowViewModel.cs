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

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Команды

        #region Комманда выхода из приложения

        /// <summary>Комманда выхода из приложения</summary>
        public ICommand ExitAppCommand { get; }
        private bool CanExitAppCommandExecute(object d) => true;
        private void OnExitAppCommandExecuted(object d) => Application.Current.Shutdown();

        #endregion

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

        #region Текущая страница

        private Page _CurrentPage;
        public Page CurrentPage
        {
            get => _CurrentPage;
            set => Set(ref _CurrentPage, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            ExitAppCommand = new LambdaCommand(OnExitAppCommandExecuted, CanExitAppCommandExecute);
            MoveBackCommand = new LambdaCommand(OnMoveBackCommandExecuted, CanMoveBackCommandExecute);

            #endregion

            CurrentPage = new ChooseCategoryPage();
        }
    }
}
