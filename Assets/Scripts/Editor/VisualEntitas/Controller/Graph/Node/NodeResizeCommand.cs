using System;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

namespace Entitas.Visual.Controller.Graph
{
    public class NodeResizeCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy = (GraphProxy)Facade.RetrieveProxy(GraphProxy.Name);
            var payload = (Tuple<Node, Rect>) notification.Body;
            graphProxy.ResizeNode(payload.First, payload.Second);
        }
    }
}
