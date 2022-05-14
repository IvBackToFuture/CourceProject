using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base;
using CourceProjectMVVMAndEntityFramework.Views;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class ChoosingCharacteristicsForGoodsWindowViewModel : BaseViewModel
    {
        #region Выбранный товар

        /// <summary>Выбранный товар</summary>
        private Goods _CurrentGoods;
        /// <summary>Выбранный товар</summary>
        public Goods CurrentGoods
        {
            get => _CurrentGoods;
            set => Set(ref _CurrentGoods, value);
        }

        #endregion

        #region Статическое свойство выбранного товара для связки с добавлением/изменением товара

        /// <summary>Статическое свойство выбранного товара для связки с добавлением/изменением товара</summary>
        public static Goods StatCurrentGoods;

        #endregion

        #region Характеристики товара

        /// <summary>Характеристики товара</summary>
        private JObject _GoodsCharacters;
        /// <summary>Характеристики товара</summary>
        public JObject GoodsCharacters
        {
            get => _GoodsCharacters;
            set => Set(ref _GoodsCharacters, value);
        }

        #endregion

        #region Команда добавления новых характеристик

        /// <summary>Команда добавления новых характеристик</summary>
        public ICommand SaveCharacterCommand { get; }
        private bool CanSaveCharacterCommandExecute(object d) => true;
        private void OnSaveCharacterCommandExecuted(object d)
        {
            JObject obj = new JObject();
            foreach (var k in GoodsCharacters)
            {
                obj.Add(k.Key, k.Value["0"]);
            }
            CurrentGoods.JSON = obj;
            System.Diagnostics.Trace.WriteLine(obj);
            System.Diagnostics.Trace.WriteLine(GoodsCharacters);
            (d as ChoosingCharacteristicsForGoodsWindow).Close();
        }

        #endregion

        public ChoosingCharacteristicsForGoodsWindowViewModel()
        {
            CurrentGoods = StatCurrentGoods;

            GoodsCharacters = new JObject();

            if (string.IsNullOrWhiteSpace(CurrentGoods.goodsJson))
            {
                foreach (var k in CurrentGoods.Categories.JSON)
                {
                    JObject token = new JObject();
                    token.Add("1", k.Value);
                    token.Add("0", k.Value[0]);
                    token.Add("2", 0);
                    GoodsCharacters.Add(k.Key, token);
                }
            }
            else
            {
                foreach (var k in CurrentGoods.Categories.JSON) {
                    JObject token = new JObject();
                    token.Add("1", k.Value);
                    token.Add("0", CurrentGoods.JSON[k.Key]);
                    int val = 0;
                    foreach (var c in CurrentGoods.Categories.JSON[k.Key])
                    {
                        System.Diagnostics.Trace.WriteLine("_" + c + "_:_" + CurrentGoods.JSON[k.Key] + "_");
                        if (c.ToString() == CurrentGoods.JSON[k.Key]?.ToString()) break;
                        val++;
                    }
                    token.Add("2", val);
                    GoodsCharacters.Add(k.Key, token);
                }
            }

            foreach(var k in GoodsCharacters)
            {
                System.Diagnostics.Trace.WriteLine($"Объект: {k}\nИмя свойства: {k.Key}\nВыбранное значение: {k.Value["0"]}\nВсе значения: {k.Value["1"]}\nИндекс значенния: {k.Value["2"]}");
            }

            SaveCharacterCommand = new LambdaCommand(OnSaveCharacterCommandExecuted, CanSaveCharacterCommandExecute);
        }
    }
}
