using Entitas.Visual.Model;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

namespace Entitas.Visual.Controller.Graph.Node
{
    public class CreateNewGraphNodeCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy = (GraphProxy) Facade.RetrieveProxy(GraphProxy.Name);
            graphProxy.AddNewNode((Vector2) notification.Body);
        }
    }
}