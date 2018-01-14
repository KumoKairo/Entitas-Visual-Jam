﻿using System;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using Entitas.Visual.View.Drawer;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View
{
    public class NodeMediator : OnGuiMediator
    {
        public const string Name = "NodeMediator";

        public Node Node { get; private set; }

        private NodeBackgroundDrawer _nodeBackgroundDrawer;
        private NodeTitleDrawer _nodeTitleDrawer;
        private NodeFieldsAdditonWidgetDrawer _nodeFieldsAdditonWidgetDrawer;
        private NodeCollapseChevronDrawer _nodeCollapseChevronDrawer;
        private NodeFieldsDrawer _nodeFieldsDrawer;

        private GenericMenu _addFieldsGenericMenu;

        public NodeMediator(string name, Node node, EditorWindow viewComponent) : base(name, viewComponent)
        {
            Node = node;
            _nodeBackgroundDrawer = new NodeBackgroundDrawer();
            _nodeTitleDrawer = new NodeTitleDrawer();
            _nodeFieldsAdditonWidgetDrawer = new NodeFieldsAdditonWidgetDrawer();
            _nodeCollapseChevronDrawer = new NodeCollapseChevronDrawer();
            _nodeFieldsDrawer = new NodeFieldsDrawer();
        }

        protected override void OnGUI(EditorWindow appView)
        {
            PaintElements(appView);
            HandleEvents(appView);
        }

        private void PaintElements(EditorWindow appView)
        {
            float titleHeight = 48f;
            float fieldsAdditionLineHeight = 32f;

            float fieldLineHeight = 16f;
            float chevronBackdropHeight = 16f;

            var isNodeCollapsed = Node.IsCollapsed;
            var nodeRect = Node.Position;
            nodeRect.height = isNodeCollapsed ? titleHeight : nodeRect.height;

            var titleRect = new Rect(nodeRect.x, nodeRect.y, nodeRect.width, titleHeight);
            var addFieldsRect = new Rect(nodeRect.x, titleRect.y + titleRect.height, nodeRect.width, fieldsAdditionLineHeight);
            addFieldsRect.height = isNodeCollapsed ? 0f : addFieldsRect.height;

            var fieldsRect = new Rect(nodeRect.x, addFieldsRect.y + addFieldsRect.height, nodeRect.width,
                fieldLineHeight * Node.Fields.Count);
            fieldsRect.height = isNodeCollapsed ? 0f : fieldsRect.height;

            var chevronRect = new Rect(nodeRect.x, fieldsRect.y + fieldsRect.height, nodeRect.width, chevronBackdropHeight);

            if (isNodeCollapsed)
            {
                chevronRect.y = nodeRect.y + titleRect.height;
            }

            var backgroundRect = new Rect(nodeRect.x, nodeRect.y, nodeRect.width,
                titleRect.height + addFieldsRect.height + fieldsRect.height);

            _nodeBackgroundDrawer.OnGUI(appView, backgroundRect, chevronRect.height);
            _nodeTitleDrawer.OnGUI(appView, titleRect, Node);

            if (!isNodeCollapsed)
            {
                _nodeFieldsAdditonWidgetDrawer.OnGUI(appView, addFieldsRect);
                _nodeFieldsDrawer.OnGUI(fieldsRect, fieldLineHeight, Node);
            }

            _nodeCollapseChevronDrawer.OnGUI(appView, chevronRect, isNodeCollapsed);
        }

        private void HandleEvents(EditorWindow appView)
        {
            var currentEvent = Event.current;
            if (currentEvent.type == EventType.Repaint || currentEvent.type == EventType.Layout)
            {
                return;
            }

            var chevronPressed = _nodeCollapseChevronDrawer.HandleEvents(currentEvent);
            if (chevronPressed)
            {
                SendNotification(NodeAreaMediator.NodeCollapse, Node);
            }

            if (_addFieldsGenericMenu == null)
            {
                _addFieldsGenericMenu = new GenericMenu();
                _addFieldsGenericMenu.AddItem(new GUIContent("float"), false, () => { OnAddFieldToNode(typeof(float)); });
                _addFieldsGenericMenu.AddItem(new GUIContent("int"), false, () => { OnAddFieldToNode(typeof(int)); });
                _addFieldsGenericMenu.AddItem(new GUIContent("string"), false, () => { OnAddFieldToNode(typeof(string)); });
                _addFieldsGenericMenu.AddItem(new GUIContent("Vector3"), false, () => { OnAddFieldToNode(typeof(Vector3)); });
                _addFieldsGenericMenu.AddItem(new GUIContent("Vector2"), false, () => { OnAddFieldToNode(typeof(Vector2)); });
            }

            _nodeFieldsAdditonWidgetDrawer.HandleEvents(currentEvent, _addFieldsGenericMenu);

            Vector2 dragNodePosition;
            Tuple<bool, bool> isNodeDraggedOrCompletedDragging = _nodeBackgroundDrawer.HandleDrag(currentEvent, out dragNodePosition);

            if (isNodeDraggedOrCompletedDragging.First)
            {
                Node.Position.position = dragNodePosition;
            }
            if (isNodeDraggedOrCompletedDragging.Second)
            {
                SendNotification(NodeAreaMediator.NodePositionUpdate, new Tuple<Node, Vector2>(Node, dragNodePosition));
            }

            if (chevronPressed || isNodeDraggedOrCompletedDragging.First)
            {
                appView.Repaint();
            }
        }

        private void OnAddFieldToNode(Type fieldType)
        {
            SendNotification(NodeAreaMediator.AddNewNodeField, new Tuple<Node, Type>(Node, fieldType));
        }
    }
}