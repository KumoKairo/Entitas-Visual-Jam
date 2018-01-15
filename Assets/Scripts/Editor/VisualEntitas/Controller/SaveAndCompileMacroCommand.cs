using Entitas.Visual.Controller.CodeGeneration;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller
{
    public class SaveAndCompileMacroCommand : MacroCommand
    {
        protected override void InitializeMacroCommand()
        {
            AddSubCommand(() => new GenerateComponentsCommand());
        }
    }
}
