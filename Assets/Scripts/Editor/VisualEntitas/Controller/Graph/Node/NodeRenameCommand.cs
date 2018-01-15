using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller.Graph
{
    public class NodeRenameCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy = (GraphProxy)Facade.RetrieveProxy(GraphProxy.Name);
            var payload = (Tuple<Node, string>) notification.Body;
            graphProxy.RenameNode(payload.First, payload.Second);
        }
    }
}
