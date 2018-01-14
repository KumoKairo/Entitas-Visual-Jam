using Entitas.Visual.Model;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

namespace Entitas.Visual.Controller.Graph
{
    public class ComponentNodeCreateNewCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy = (GraphProxy) Facade.RetrieveProxy(GraphProxy.Name);
            graphProxy.AddNewNode((Vector2) notification.Body);
        }
    }
}