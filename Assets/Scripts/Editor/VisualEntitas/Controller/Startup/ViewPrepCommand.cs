using Entitas.Visual.View;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller
{
    public class ViewPrepCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Facade.RegisterMediator(new VisualEntitasAppMediator());
        }
    }
}
