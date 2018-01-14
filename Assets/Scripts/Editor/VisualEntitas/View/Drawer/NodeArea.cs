using System;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeArea : IOnGuiView
    {
        public event Action<Vector2> CreateNewComponentEvent;
        public event Action<Node> NodeRemovedEvent;
        public event Action<Node> NodeCollapsedEvent;
        public event Action<Node, Vector2, bool> NodeUpdatedPositionEvent;

        private GenericMenu _backdropContextMenu;
        private GenericMenu _nodeContextMenu;

        private Vector2 _lastMousePosition;
        private Node _lastSelectedNode;
        private Node _draggingNode;
        private Vector2 _draggingOffset;
        private GraphProxy _graphProxy;

        public NodeArea(GraphProxy graphProxy)
        {
            _graphProxy = graphProxy;
        }

        public void OnGUI(EditorWindow window)
        {
            CreateContextMenusIfNeeded();

            var current = Event.current;
            Node mouseOverNode = null;
            foreach (var node in _graphProxy.GraphData.Nodes)
            {
                var newMouseOverNode = DrawNode(node, current, window);
                mouseOverNode = mouseOverNode ?? newMouseOverNode;
            }

            var nodeAreaRect = new Rect(0f, 16f, window.position.width, window.position.height);

            if (current.type == EventType.Layout || current.type == EventType.Repaint)
            {
                return;
            }

            switch (current.button)
            {
                case 0:
                    HandleDrag(mouseOverNode, current, window);
                    break;
                case 1:
                    HandleRightClick(mouseOverNode, current, nodeAreaRect);
                    break;
            }
        }

        private Node DrawNode(Node node, Event current, EditorWindow window)
        {
            Node mouseOverNode = null;

            float titleHeight = 32f;

            var nodePosition = node.Position;
            nodePosition.height = node.IsCollapsed ? titleHeight : nodePosition.height;

            GUIHelper.DrawQuad(nodePosition, StyleProxy.NodeBackgroundColor);
            var titleBackdropBox = new Rect(node.Position.position, new Vector2(node.Position.width, titleHeight));
            GUIHelper.DrawQuad(titleBackdropBox, StyleProxy.NodeTitleBackdropColor);

            var textBlockSize = StyleProxy.NodeTitleTextStyle.CalcSize(new GUIContent("NODE"));
            var textPosition = new Rect(
                node.Position.x + node.Position.width * 0.5f - textBlockSize.x * 0.5f,
                node.Position.y + titleBackdropBox.height * 0.5f - textBlockSize.y * 0.5f,
                textBlockSize.x,
                16f);

            GUI.Box(textPosition, "NODE", StyleProxy.NodeTitleTextStyle);
            if (node.Position.Contains(current.mousePosition))
            {
                mouseOverNode = node;
            }

            var chevronSize = new Vector2(32f, 32f);
            var chevronPosition = new Rect(
                new Vector2(
                    nodePosition.x + nodePosition.width * 0.5f - chevronSize.x * 0.5f,
                    nodePosition.y + nodePosition.height - chevronSize.y * 0.25f
                    ),
                chevronSize);

            var chevronBackdropPosition = chevronPosition.Offset(new Rect(-8f, 8f, 16f, -16f));
            chevronBackdropPosition.x = node.Position.x;
            chevronBackdropPosition.width = node.Position.width;
            GUIHelper.DrawQuad(chevronBackdropPosition, StyleProxy.ChevronUpBackdropColor);
            var color = GUI.color;
            GUI.color = StyleProxy.ChevronUpColor;
            GUI.DrawTexture(chevronPosition, node.IsCollapsed ? StyleProxy.ChevronDownTexture : StyleProxy.ChevronUpTexture);
            GUI.color = color;

            if (current.type == EventType.MouseDown && chevronBackdropPosition.Contains(current.mousePosition) && current.button == 0)
            {
                if (NodeCollapsedEvent != null)
                {
                    NodeCollapsedEvent(node);
                    window.Repaint();
                    current.Use();
                }
            }

            return mouseOverNode;
        }

        private void HandleDrag(Node mouseOverNode, Event current, EditorWindow window)
        {
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (mouseOverNode != null)
                    {
                        _draggingNode = mouseOverNode;
                        _draggingOffset = mouseOverNode.Position.position - current.mousePosition;
                        current.Use();
                    }
                    break;

                case EventType.MouseUp:
                    if (_draggingNode != null && NodeUpdatedPositionEvent != null)
                    {
                        NodeUpdatedPositionEvent(_draggingNode, current.mousePosition + _draggingOffset, true);
                    }
                    _draggingNode = null;
                    break;

                case EventType.MouseMove:
                case EventType.MouseDrag:
                    if (_draggingNode != null)
                    {
                        if(NodeUpdatedPositionEvent != null)
                        {
                            NodeUpdatedPositionEvent(_draggingNode, current.mousePosition + _draggingOffset, false);
                        }
                        current.Use();
                        window.Repaint();
                    }
                    break;
            }

        }

        private void HandleRightClick(Node mouseOverNode, Event current, Rect nodeAreaRect)
        {
            if (mouseOverNode != null)
            {
                _nodeContextMenu.ShowAsContext();
                _lastSelectedNode = mouseOverNode;
                current.Use();
            }
            else if (nodeAreaRect.Contains(current.mousePosition) 
                && current.type == EventType.MouseDown)
            {
                _backdropContextMenu.ShowAsContext();
                _lastMousePosition = current.mousePosition;
                current.Use();
            }
        }

        private void CreateContextMenusIfNeeded()
        {
            if (_backdropContextMenu == null)
            {
                _backdropContextMenu = new GenericMenu();
                _backdropContextMenu.AddItem(new GUIContent("Component"), false, OnCreateComponentMenuSelected);
            }
            if (_nodeContextMenu == null)
            {
                _nodeContextMenu = new GenericMenu();
                _nodeContextMenu.AddItem(new GUIContent("Remove"), false, OnRemoveNodeMenuSelected);
            }
        }

        private void OnCreateComponentMenuSelected()
        {
            if (CreateNewComponentEvent != null)
            {
                CreateNewComponentEvent(_lastMousePosition);
            }
        }

        private void OnRemoveNodeMenuSelected()
        {
            if (NodeRemovedEvent != null)
            {
                NodeRemovedEvent(_lastSelectedNode);
            }
        }
    }
}