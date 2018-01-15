using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller.Graph
{
    public class NodeFieldRenameCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy = (GraphProxy)Facade.RetrieveProxy(GraphProxy.Name);
            var payload = (Tuple<Node, Field>)notification.Body;
            graphProxy.RenameNodeField(payload.First, payload.Second);
        }
    }
}
