using System;
using System.Collections.Generic;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.View.Drawer;
using UnityEditor;
using UnityEngine;
using Entitas.Visual.Utils;

namespace Entitas.Visual.View
{
    public class NodeAreaMediator : OnGuiMediator
    {
        public const string Name = "NodeAreaMediator";

        public const string CreateNewComponent = "Node_CreateNewComponent";
        public const string AddNewNodeField = "Node_AddNewField";
        public const string NodePositionUpdate = "Node_PositionUpdate";
        public const string NodeRemove = "Node_Remove";
        public const string NodeCollapse = "Node_Collapse";

        private NodeArea _nodeArea;

        private List<NodeMediator> _nodeMediators;

        public NodeAreaMediator(EditorWindow parentWindow) : base(Name, parentWindow)
        {
        }

        public override void OnRegister()
        {
            var graphProxy = (GraphProxy) Facade.RetrieveProxy(GraphProxy.Name);
            var nodes = graphProxy.GraphData.Nodes;
            _nodeMediators = new List<NodeMediator>(nodes.Count);

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var mediator = new NodeMediator(NodeMediator.Name + i, node, (EditorWindow) ViewComponent);
                _nodeMediators.Add(mediator);
                Facade.RegisterMediator(mediator);
            }

            //_nodeArea = new NodeArea(graphProxy);

            //_nodeArea.CreateNewComponentEvent += OnCreateNewComponent;
            //_nodeArea.NodeRemovedEvent += OnNodeRemove;
            //_nodeArea.NodeUpdatedPositionEvent += OnNodePositionUpdated;
            //_nodeArea.NodeCollapsedEvent += OnNodeCollapsed;
            //_nodeArea.NodeAddedFieldEvent += OnNodeAddedField;
        }

        public override void OnRemove()
        {
            //_nodeArea.CreateNewComponentEvent -= OnCreateNewComponent;
            //_nodeArea.NodeRemovedEvent -= OnNodeRemove;
            //_nodeArea.NodeUpdatedPositionEvent -= OnNodePositionUpdated;
            //_nodeArea.NodeCollapsedEvent -= OnNodeCollapsed;
            //_nodeArea.NodeAddedFieldEvent -= OnNodeAddedField;

            //_nodeArea = null;
        }

        protected override void OnGUI(EditorWindow appView)
        {
            //_nodeArea.OnGUI(appView);
        }

        private void OnCreateNewComponent(Vector2 mousePosition)
        {
            SendNotification(CreateNewComponent, mousePosition);
        }

        private void OnNodeAddedField(Node node, Type type)
        {
            SendNotification(AddNewNodeField, new Tuple<Node, Type>(node, type));
        }

        private void OnNodePositionUpdated(Node node, Vector2 newPosition, bool isCompleted)
        {
            node.Position.position = newPosition;
            if (isCompleted)
            {
                SendNotification(NodePositionUpdate, new Tuple<Node, Vector2>(node, newPosition));
            }
        }

        private void OnNodeRemove(Node node)
        {
            SendNotification(NodeRemove, node);
        }

        private void OnNodeCollapsed(Node node)
        {
            SendNotification(NodeCollapse, node);
        }
    }
}