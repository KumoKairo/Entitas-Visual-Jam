using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller
{
    public class TeardownCommand : MacroCommand
    {
        protected override void InitializeMacroCommand()
        {
            AddSubCommand(() => new ViewTeardownCommand());
        }
    }
}
