using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourceProjectMVVMAndEntityFramework.Infrastructure.Commands.Base
{
    class LambdaCommand : BaseCommand
    {
        private Action<object> CommandExecute;
        private Func<object, bool> CanCommandExecuted;

        public LambdaCommand(Action<object> CommandExec, Func<object, bool> CanCommandExec = null)
        {
            CommandExecute = CommandExec ?? throw new ArgumentNullException(nameof(CommandExec));
            CanCommandExecuted = CanCommandExec;
        }

        public override bool CanExecute(object parameter) => CanCommandExecuted?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => CommandExecute(parameter);
    }
}
