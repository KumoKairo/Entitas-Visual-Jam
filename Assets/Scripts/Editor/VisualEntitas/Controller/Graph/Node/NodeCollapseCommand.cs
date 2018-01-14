using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller.Graph
{
    public class NodeCollapseCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy = (GraphProxy)Facade.RetrieveProxy(GraphProxy.Name);
            var payload = (Node)notification.Body;
            graphProxy.CollapseNode(payload);
        }
    }
}