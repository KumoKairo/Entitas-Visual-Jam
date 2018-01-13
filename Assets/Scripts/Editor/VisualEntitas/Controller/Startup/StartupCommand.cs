using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller
{
    public class StartupCommand : MacroCommand
    {
        protected override void InitializeMacroCommand()
        {
            AddSubCommand(() => new ModelPrepCommand());
            AddSubCommand(() => new ViewPrepCommand());
        }
    }
}
