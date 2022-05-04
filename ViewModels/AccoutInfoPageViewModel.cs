using CourceProjectMVVMAndEntityFramework.Models;
using CourceProjectMVVMAndEntityFramework.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourceProjectMVVMAndEntityFramework.ViewModels
{
    class AccoutInfoPageViewModel : BaseViewModel
    {
        #region Текущий пользователь

        private Users _CurrentUser = null;
        public Users CurrentUser
        {
            get => _CurrentUser;
            set => Set(ref _CurrentUser, value);
        }

        #endregion
    }
}
