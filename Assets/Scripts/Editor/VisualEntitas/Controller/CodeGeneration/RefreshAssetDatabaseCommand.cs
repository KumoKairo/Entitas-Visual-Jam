using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEditor;

namespace Entitas.Visual.Controller.CodeGeneration
{
    public class RefreshAssetDatabaseCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            AssetDatabase.Refresh();
        }
    }
}
