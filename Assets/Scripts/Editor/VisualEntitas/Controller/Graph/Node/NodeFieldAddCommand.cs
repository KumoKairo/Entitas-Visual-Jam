using System;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Entitas.Visual.Controller.Graph
{
    public class NodeFieldAddCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy =  (GraphProxy) Facade.RetrieveProxy(GraphProxy.Name);
            var payload = (Tuple<Node, Type>) notification.Body;
            graphProxy.AddFieldToNode(payload.First, payload.Second);
        }
    }
}