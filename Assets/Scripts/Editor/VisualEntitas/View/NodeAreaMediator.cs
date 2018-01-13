using System;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.View.Drawer;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View
{
    public class NodeAreaMediator : OnGuiMediator
    {
        public const string Name = "NodeAreaMediator";

        private NodeArea _nodeArea;
        private GraphProxy _graphProxy;

        public NodeAreaMediator(EditorWindow parentWindow) : base(Name, parentWindow)
        {
        }

        public override void OnRegister()
        {
            _graphProxy = (GraphProxy) Facade.RetrieveProxy(GraphProxy.Name);
            _nodeArea = new NodeArea(_graphProxy);
            _nodeArea.CreateNewComponentEvent += OnCreateNewComponent;
            _nodeArea.NodeRemovedEvent += OnNodeRemove;
            _nodeArea.NodeUpdatedPositionEvent += OnNodePositionUpdated;
        }

        public override void OnRemove()
        {
            _nodeArea.CreateNewComponentEvent -= OnCreateNewComponent;
            _nodeArea.NodeRemovedEvent -= OnNodeRemove;
            _nodeArea.NodeUpdatedPositionEvent -= OnNodePositionUpdated;
            _nodeArea = null;
        }

        protected override void OnGUI(EditorWindow appView)
        {
            _nodeArea.OnGUI(appView);
        }

        private void OnCreateNewComponent(Vector2 mousePosition)
        {
            _graphProxy.AddNewNode(mousePosition);
        }

        private void OnNodePositionUpdated(Node node, Vector2 newPosition)
        {
            var position = node.Position;
            position.position = newPosition;
            node.Position = position;
            //_graphProxy.UpdateNodePosition(node);
        }

        private void OnNodeRemove(Node node)
        {
            _graphProxy.RemoveNode(node);
        }
    }
}