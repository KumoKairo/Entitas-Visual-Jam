using Entitas.Visual.View;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller
{
    public class ViewTeardownCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Facade.RemoveMediator(VisualEntitasAppMediator.Name);
        }
    }
}
