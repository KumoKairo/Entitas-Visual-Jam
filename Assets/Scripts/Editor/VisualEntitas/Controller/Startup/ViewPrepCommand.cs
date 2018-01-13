using Entitas.Visual.View;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEditor;

namespace Entitas.Visual.Controller
{
    public class ViewPrepCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Facade.RegisterMediator(new VisualEntitasAppMediator(notification.Body as EditorWindow));
        }
    }
}
