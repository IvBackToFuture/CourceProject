using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class AddNewCategoryWindowViewModel : BaseViewModel
    {
        #region Новая категория

        /// <summary>Новая категория</summary>
        private Categories _Category;
        /// <summary>Новая категория</summary>
        public Categories Category
        {
            get => _Category;
            set => Set(ref _Category, value);
        }

        #endregion

        #region Строка с именем нового свойства

        /// <summary>Строка с именем нового свойства</summary>
        private string _NewCharater;
        /// <summary>Строка с именем нового свойства</summary>
        public string NewCharacter
        {
            get => _NewCharater;
            set => Set(ref _NewCharater, value);
        }

        #endregion

        #region Строка с новым значением

        /// <summary>Строка с новым значением</summary>
        private string _NewValue;
        /// <summary>Строка с новым значением</summary>
        public string NewValue
        {
            get => _NewValue;
            set => Set(ref _NewValue, value);
        }

        #endregion

        #region JObject для формирования характеристик

        /// <summary>JObject для формирования характеристик</summary>
        private JObject _Properties;
        /// <summary>JObject для формирования характеристик</summary>
        public JObject Properties
        {
            get => _Properties;
            set => Set(ref _Properties, value);
        }

        #endregion

        #region JProperty для формирования значений характеристик

        /// <summary>JProperty для формирования значений характеристик</summary>
        private JProperty _ChoosenProperty;
        /// <summary>JProperty для формирования значений характеристик</summary>
        public JProperty ChoosenProperty
        {
            get => _ChoosenProperty;
            set => Set(ref _ChoosenProperty, value);
        }

        #endregion

        #region JValue для удаления значений

        /// <summary>JValue для удаления значений</summary>
        private JValue _ChoosenValue;
        /// <summary>JValue для удаления значений</summary>
        public JValue ChoosenValue
        {
            get => _ChoosenValue;
            set => Set(ref _ChoosenValue, value);
        }

        #endregion

        #region Сообщение

        /// <summary>Сообщение</summary>
        private string _Message;
        /// <summary>Сообщение</summary>
        public string Message
        {
            get => _Message;
            set => Set(ref _Message, value);
        }

        #endregion

        #region Статическое свойство для добавления новой категории

        /// <summary>Статическое свойство для добавления новой категории</summary>
        public static Page WorkPlace;

        #endregion

        public AddNewCategoryWindowViewModel()
        {
            Properties = new JObject();
            Category = new Categories();
            AddNewCategoryCommand = new LambdaCommand(OnAddNewCategoryCommandExecuted, CanAddNewCategoryCommandExecute);
            AddNewCharacterCommand = new LambdaCommand(OnAddNewCharacterCommandExecuted, CanAddNewCharacterCommandExecute);
            AddNewValueCommand = new LambdaCommand(OnAddNewValueCommandExecuted, CanAddNewValueCommandExecute);
            RemoveCharacterCommand = new LambdaCommand(OnRemoveCharacterCommandExecuted, CanRemoveCharacterCommandExecute);
            RemoveValueCommand = new LambdaCommand(OnRemoveValueCommandExecuted, CanRemoveValueCommandExecute);
        }

        #region Команда добавления нового свойства

        /// <summary>Команда добавления нового свойства</summary>
        public ICommand AddNewCharacterCommand { get; }
        private bool CanAddNewCharacterCommandExecute(object d) => !string.IsNullOrWhiteSpace(NewCharacter) && !Properties.ContainsKey(NewCharacter);
        private void OnAddNewCharacterCommandExecuted(object d)
        {
            Properties.Add(new JProperty(NewCharacter, new JArray()));
            NewCharacter = null;
        }

        #endregion

        #region Команда удаления свойства

        /// <summary>Команда удаления свойства</summary>
        public ICommand RemoveCharacterCommand { get; }
        private bool CanRemoveCharacterCommandExecute(object d) => ChoosenProperty != null;
        private void OnRemoveCharacterCommandExecuted(object d)
        {
            Properties.Remove(ChoosenProperty.Name);
        }

        #endregion

        #region Команда добавления нового значения

        /// <summary>Команда добавления нового значения</summary>
        public ICommand AddNewValueCommand { get; }
        private bool CanAddNewValueCommandExecute(object d) => !string.IsNullOrWhiteSpace(NewValue) && ChoosenProperty != null &&
            !(ChoosenProperty.Value as JArray).Select(x => x.ToString()).Contains(NewValue);
        private void OnAddNewValueCommandExecuted(object d)
        {
            (ChoosenProperty.Value as JArray).Add(NewValue);
            NewValue = null;
        }

        #endregion

        #region Команда удаления значения

        /// <summary>Команда удаления значения</summary>
        public ICommand RemoveValueCommand { get; }
        private bool CanRemoveValueCommandExecute(object d) => ChoosenValue != null;
        private void OnRemoveValueCommandExecuted(object d)
        {
            ChoosenValue.Remove();
        }

        #endregion

        #region Команда сохранения новой категории

        /// <summary>Команда сохранения новой категории</summary>
        public ICommand AddNewCategoryCommand { get; }
        private bool CanAddNewCategoryCommandExecute(object d) => !string.IsNullOrWhiteSpace(Category.catName?.Trim()) && Properties.Count > 0 && Properties.Values<JProperty>().ToList().All(x => (x.Value as JArray).Count() > 2);
        private void OnAddNewCategoryCommandExecuted(object d)
        {
            if (OneStopStoreEntities.GetContext().Categories.Select(x => x.catName).Contains(Category.catName))
            {
                Message = "Категория с таким именем уже существует";
            }
            else
            {
                Category.JSON = Properties;
                OneStopStoreEntities.GetContext().Categories.Add(Category);
                OneStopStoreEntities.GetContext().SaveChanges();
                ((dynamic)WorkPlace.DataContext).Categories.Add(Category);
                (d as Window).Close();
            }
        }

        #endregion
    }
}
