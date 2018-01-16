using System.IO;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

namespace Entitas.Visual.Controller.CodeGeneration
{
    public class CleanupAndPrepareTargetDirectoryCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var targetFolder = Application.dataPath + SaveAndCompileMacroCommand.GenerateToDirectory;
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            foreach (var file in Directory.GetFiles(targetFolder, "*", SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
        }
    }
}
