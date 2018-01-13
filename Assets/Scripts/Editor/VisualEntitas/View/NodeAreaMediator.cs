using System;
using Entitas.Visual.Model;
using Entitas.Visual.View.Drawer;
using PureMVC.Patterns.Mediator;
using UnityEngine;

namespace Entitas.Visual.View
{
    public class NodeAreaMediator : Mediator
    {
        public const string Name = "NodeAreaMediator";

        private GraphWindow _parentView
        {
            get
            {
                return ViewComponent as GraphWindow;
            }
        }

        private NodeArea _nodeArea;

        public NodeAreaMediator(GraphWindow parentWindow) : base(Name, parentWindow)
        {
        }

        public override void OnRegister()
        {
            _nodeArea = new NodeArea();
            _nodeArea.RegisterOnGUITo(_parentView, true);
            _nodeArea.CreateNewComponentEvent += OnCreateNewComponent;
        }

        public override void OnRemove()
        {
            _nodeArea.CreateNewComponentEvent -= OnCreateNewComponent;
            _nodeArea.RegisterOnGUITo(_parentView, false);
            _nodeArea = null;
        }

        private void OnCreateNewComponent()
        {
            var graphProxy = Facade.RetrieveProxy(GraphProxy.Name);
            Debug.Log(graphProxy.ProxyName);
        }
    }
}