using Entitas.Visual.Model;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller
{
    public class ModelPrepCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Facade.RegisterProxy(new GraphProxy());
            Facade.RegisterProxy(new FieldTypeProviderProxy());
        }
    }
}