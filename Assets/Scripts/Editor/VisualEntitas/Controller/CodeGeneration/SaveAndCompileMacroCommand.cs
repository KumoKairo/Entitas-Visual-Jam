using Entitas.Visual.Controller.CodeGeneration;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller
{
    public class SaveAndCompileMacroCommand : MacroCommand
    {
        public const string GenerateToDirectory = "/GeneratedCode/";

        protected override void InitializeMacroCommand()
        {
            AddSubCommand(() => new CleanupAndPrepareTargetDirectoryCommand());
            AddSubCommand(() => new GenerateComponentsCommand());
            AddSubCommand(() => new RefreshAssetDatabaseCommand());
        }
    }
}
