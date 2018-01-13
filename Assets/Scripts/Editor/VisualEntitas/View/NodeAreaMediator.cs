using System;
using Entitas.Visual.Model;
using Entitas.Visual.View.Drawer;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View
{
    public class NodeAreaMediator : OnGuiMediator
    {
        public const string Name = "NodeAreaMediator";

        private NodeArea _nodeArea;

        public NodeAreaMediator(EditorWindow parentWindow) : base(Name, parentWindow)
        {
        }

        public override void OnRegister()
        {
            _nodeArea = new NodeArea();
            _nodeArea.CreateNewComponentEvent += OnCreateNewComponent;
        }

        public override void OnRemove()
        {
            _nodeArea.CreateNewComponentEvent -= OnCreateNewComponent;
            _nodeArea = null;
        }

        private void OnCreateNewComponent()
        {
            var graphProxy = Facade.RetrieveProxy(GraphProxy.Name);
            Debug.Log(graphProxy.ProxyName);
        }

        protected override void OnGUI(EditorWindow appView)
        {
            _nodeArea.OnGUI(appView);
        }
    }
}