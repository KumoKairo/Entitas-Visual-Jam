using System;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeArea : IOnGuiView
    {
        public event Action<Vector2> CreateNewComponentEvent;
        public event Action<Node> NodeRemovedEvent;
        public event Action<Node, Vector2> NodeUpdatedPositionEvent;

        private static int RightClickControlID = "NodeAreaRightClick".GetHashCode();

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
                GUI.Box(node.Position, "", StyleProxy.NodeBackgroundStyle);
                if (node.Position.Contains(current.mousePosition))
                {
                    mouseOverNode = node;
                }
            }

            var nodeAreaRect = new Rect(0f, 16f, window.position.width, window.position.height);

            if (current.type == EventType.Layout || current.type == EventType.Repaint)
            {
                return;
            }

            int controlId = GUIUtility.GetControlID(RightClickControlID, FocusType.Passive);
            switch (current.button)
            {
                case 0:
                    HandleDrag(mouseOverNode, current, controlId, window);
                    break;
                case 1:
                    HandleRightClick(mouseOverNode, current, nodeAreaRect, controlId);
                    break;
            }
        }

        private void HandleDrag(Node mouseOverNode, Event current, int controlId, EditorWindow window)
        {
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (mouseOverNode != null)
                    {
                        _draggingNode = mouseOverNode;
                        _draggingOffset = mouseOverNode.Position.position - current.mousePosition;
                        //GUIUtility.hotControl = controlId;
                    }
                    break;

                case EventType.MouseUp:
                    //if (GUIUtility.hotControl != controlId)
                    //{
                    //    break;
                    //}
                    //GUIUtility.hotControl = 0;
                    _draggingNode = null;
                    break;

                case EventType.MouseMove:
                case EventType.MouseDrag:
                    //if (GUIUtility.hotControl != controlId)
                    //{
                    //    break;
                    //}
                    if (_draggingNode != null)
                    {
                        if (NodeUpdatedPositionEvent != null)
                        {
                            NodeUpdatedPositionEvent(_draggingNode, current.mousePosition + _draggingOffset);
                            window.Repaint();
                        }
                    }
                    break;
            }

        }

        private void HandleRightClick(Node mouseOverNode, Event current, Rect nodeAreaRect, int controlId)
        {
            if (mouseOverNode != null)
            {
                _nodeContextMenu.ShowAsContext();
                _lastSelectedNode = mouseOverNode;
                current.Use();
            }
            else if (nodeAreaRect.Contains(current.mousePosition)
                     && current.GetTypeForControl(controlId) == EventType.MouseDown)
            {
                GUIUtility.hotControl = controlId;
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