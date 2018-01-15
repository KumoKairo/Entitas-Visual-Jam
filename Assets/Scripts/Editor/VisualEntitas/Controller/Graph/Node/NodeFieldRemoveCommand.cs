using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller.Graph
{
    public class NodeFieldRemoveCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy = (GraphProxy)Facade.RetrieveProxy(GraphProxy.Name);
            var payload = (Tuple<Node, Field>)notification.Body;
            graphProxy.RemoveNodeField(payload.First, payload.Second);
        }
    }
}
