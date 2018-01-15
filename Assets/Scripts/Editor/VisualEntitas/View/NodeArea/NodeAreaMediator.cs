using System.Collections.Generic;
using System.Linq;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.View.Drawer;
using PureMVC.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View
{
    public class NodeAreaMediator : OnGuiMediator
    {
        public const string Name = "NodeAreaMediator";

        public const string CreateNewComponent = "Node_CreateNewComponent";
        public const string NodeFieldAdd = "Node_AddNewField";
        public const string NodePositionUpdate = "Node_PositionUpdate";
        public const string NodeRemove = "Node_Remove";
        public const string NodeCollapse = "Node_Collapse"; 
        public const string NodeFieldRemove = "Node_FieldRemove";
        public const string NodeRename = "Node_Rename";

        private NodeAreaBackgroundDrawer _backgroundDrawer;
        private List<NodeMediator> _nodeMediators;

        private GenericMenu _backdropContextMenu;

        public NodeAreaMediator(EditorWindow parentWindow) : base(Name, parentWindow)
        {
        }

        public override void OnRegister()
        {
            _backgroundDrawer = new NodeAreaBackgroundDrawer();

            var graphProxy = (GraphProxy) Facade.RetrieveProxy(GraphProxy.Name);
            var nodes = graphProxy.GraphData.Nodes;
            _nodeMediators = new List<NodeMediator>(nodes.Count);

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                RegisterNewMediatorFor(i, node);
            }
        }

        protected override void OnGUI(EditorWindow appView)
        {
            NodeMediator mediatorDuringRename = null;

            foreach (var nodeMediator in _nodeMediators)
            {
                if (nodeMediator.IsRenaming)
                {
                    mediatorDuringRename = nodeMediator;
                }
            }

            foreach (var nodeMediator in _nodeMediators)
            {
                var shouldHandleEvents = true;
                if (mediatorDuringRename != null)
                {
                    shouldHandleEvents = nodeMediator == mediatorDuringRename;
                }

                nodeMediator.OnGUI(appView, shouldHandleEvents);
            }

            var currentEvent = Event.current;
            if (currentEvent.type == EventType.Repaint || currentEvent.type == EventType.Layout)
            {
                return;
            }

            if (_backdropContextMenu == null)
            {
                _backdropContextMenu = new GenericMenu();
                _backdropContextMenu.AddItem(new GUIContent("Component"), false, OnCreateComponentMenuSelected);
            }

            _backgroundDrawer.HandleRightClick(appView.position, currentEvent, _backdropContextMenu);
        }

        private void RegisterNewMediatorFor(int i, Node node)
        {
            var mediator = new NodeMediator(NodeMediator.Name + i, node, (EditorWindow) ViewComponent);
            _nodeMediators.Add(mediator);
            Facade.RegisterMediator(mediator);
        }

        private void RemoveMediatorFor(Node node)
        {
            NodeMediator mediator = null;
            foreach (var nodeMediator in _nodeMediators)
            {
                if (nodeMediator.Node == node)
                {
                    mediator = nodeMediator;
                    break;
                }
            }

            if (mediator != null)
            {
                _nodeMediators.Remove(mediator);
                Facade.RemoveMediator(mediator.MediatorName);
            }
        }

        public override void OnRemove()
        {
            
        }

        public override string[] ListNotificationInterests()
        {
            var parentNotifications = base.ListNotificationInterests();
            var nodeNotifications = new[]
            {
                GraphProxy.NodeAdded,
                GraphProxy.NodeRemoved
            };

            return parentNotifications.Concat(nodeNotifications).ToArray(); ;
        }

        public override void HandleNotification(INotification notification)
        {
            if (notification.Name != VisualEntitasFacade.OnGUI)
            {
                Debug.Log("Received " + notification.Name);
            }

            base.HandleNotification(notification);
            Node payload = null;
            switch (notification.Name)
            {
                case GraphProxy.NodeAdded:
                    payload = (Node) notification.Body;
                    RegisterNewMediatorFor(_nodeMediators.Count, payload);
                    break;
                case GraphProxy.NodeRemoved:
                    payload = (Node)notification.Body;
                    RemoveMediatorFor(payload);
                    break;
            }
        }

        private void OnCreateComponentMenuSelected()
        {
            SendNotification(CreateNewComponent, _backgroundDrawer.LastClickPosition);
        }
    }
}