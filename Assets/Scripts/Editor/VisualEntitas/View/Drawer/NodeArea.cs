using System;
using System.Collections.Generic;
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
        public event Action<Node, Type> NodeAddedFieldEvent;
        public event Action<Node, Vector2, bool> NodeUpdatedPositionEvent;

        private GenericMenu _backdropContextMenu;
        private GenericMenu _nodeContextMenu;

        private Vector2 _lastMousePosition;
        private Node _lastSelectedNode;
        private Node _draggingNode;

        private List<Node> _nodes;

        private Vector2 _initialDragPosition;
        private Vector2 _draggingOffset;
        private GenericMenu _addFieldsGenericMenu;

        public NodeArea(GraphProxy graphProxy)
        {
            _nodes = graphProxy.GraphData.Nodes;
        }

        public void OnGUI(EditorWindow window)
        {
            CreateContextMenusIfNeeded();

            var current = Event.current;
            Node mouseOverNode = null;
            foreach (var node in _nodes)
            {
                var newMouseOverNode = DrawNode(node, current, window);
                mouseOverNode = newMouseOverNode ?? mouseOverNode;
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

            float titleHeight = 48f;
            float fieldsAdditionLineHeight = 32f;

            float fieldLineHeight = 16f;
            float chevronBackdropHeight = 16f;

            int fieldsCount = node.Fields.Count;

            var nodePosition = node.Position;
            nodePosition.height = node.IsCollapsed ? titleHeight : titleHeight + fieldsAdditionLineHeight + fieldLineHeight * fieldsCount;
            GUIHelper.DrawQuad(nodePosition, StyleProxy.NodeBackgroundColor);

            float shadowOffset = 8f;
            GUIHelper.DrawQuad(new Rect(
                    nodePosition.x + shadowOffset, 
                    nodePosition.y + nodePosition.height + chevronBackdropHeight,
                    nodePosition.width,
                    shadowOffset), 
                StyleProxy.SemiTransparentBlackColor);

            GUIHelper.DrawQuad(new Rect(
                    nodePosition.x + nodePosition.width,
                    nodePosition.y + shadowOffset,
                    shadowOffset,
                    nodePosition.height + shadowOffset),
                StyleProxy.SemiTransparentBlackColor);

            DrawTitle(node, titleHeight);

            if (!node.IsCollapsed)
            {
                DrawFieldsAdditionLine(node, current, nodePosition, titleHeight, fieldsAdditionLineHeight);

                int i = 1;
                float startingFieldsY = nodePosition.y + titleHeight + 24f;

                foreach (var nodeField in node.Fields)
                {
                    if (i % 2 == 0)
                    {
                        var fieldBackdropPosition = 
                            new Rect(nodePosition.position.x, 
                            startingFieldsY + fieldLineHeight * i - fieldLineHeight * 0.5f, 
                            nodePosition.width, fieldLineHeight + 2f);

                        GUIHelper.DrawQuad(fieldBackdropPosition, StyleProxy.TransparentBlackColor);
                    }

                    var fieldTextSize = StyleProxy.NodeFieldsTextStyle.CalcSize(new GUIContent(nodeField));
                    var minusIconSize = new Vector2(30f, 30f);

                    var fieldTextPosition = new Rect(
                        nodePosition.x + 16f,
                        startingFieldsY + fieldLineHeight * i - fieldTextSize.y * 0.5f,
                        fieldTextSize.x,
                        fieldTextSize.y
                    );

                    var minusIconPosition = new Rect(
                        nodePosition.x + nodePosition.width - minusIconSize.x,
                        fieldTextPosition.y - fieldTextSize.y * 0.5f + 2f,
                        minusIconSize.x,
                        minusIconSize.y
                    );

                    var color = GUI.color;
                    GUI.color = StyleProxy.MinusIconColor;
                    GUI.DrawTexture(minusIconPosition, StyleProxy.MinusIconTexture);
                    GUI.color = color;

                    var splitType = nodeField.Split('.');
                    var displayFieldType = splitType[splitType.Length - 1];

                    GUI.Box(fieldTextPosition, displayFieldType, StyleProxy.NodeFieldsTextStyle);

                    i += 1;
                }
            }

            DrawCollapseChevron(node, current, window, nodePosition, chevronBackdropHeight);

            if (node.Position.Contains(current.mousePosition))
            {
                mouseOverNode = node;
            }

            return mouseOverNode;
        }

        private void DrawFieldsAdditionLine(Node node, Event current, Rect nodePosition, float titleHeight,
            float fieldsBlockHeight)
        {
            var fullBackdropPosition = new Rect(nodePosition.position.x, nodePosition.y + titleHeight, nodePosition.width, fieldsBlockHeight);
            GUIHelper.DrawQuad(fullBackdropPosition, StyleProxy.SemiTransparentBlackColor);

            var fieldsTextSize = StyleProxy.NodeFieldsTextStyle.CalcSize(new GUIContent("FIELDS"));
            var plusIconSize = new Vector2(30f, 30f);

            var plusIconPosition = new Rect(
                nodePosition.x + nodePosition.width - plusIconSize.x,
                nodePosition.y + titleHeight + 1f,
                plusIconSize.x,
                plusIconSize.y
            );

            var color = GUI.color;
            GUI.color = StyleProxy.NodeFieldsTextColor;
            GUI.DrawTexture(plusIconPosition, StyleProxy.PlusIconTexture);
            GUI.color = color;

            var fieldsTextPosition = new Rect(
                nodePosition.x + 16f,
                nodePosition.y + titleHeight + fieldsBlockHeight * 0.5f - fieldsTextSize.y * 0.5f,
                fieldsTextSize.x,
                fieldsTextSize.y
            );

            GUI.Box(fieldsTextPosition, "FIELDS", StyleProxy.NodeFieldsTextStyle);

            if (plusIconPosition.Contains(current.mousePosition) && current.type == EventType.MouseDown &&
                current.button == 0)
            {
                if (_addFieldsGenericMenu == null)
                {
                    _addFieldsGenericMenu = new GenericMenu();
                    _addFieldsGenericMenu.AddItem(new GUIContent("float"), false, () => { OnAddField(typeof(float)); });
                    _addFieldsGenericMenu.AddItem(new GUIContent("int"), false, () => { OnAddField(typeof(int)); });
                    _addFieldsGenericMenu.AddItem(new GUIContent("string"), false, () => { OnAddField(typeof(string)); });
                    _addFieldsGenericMenu.AddItem(new GUIContent("Vector3"), false, () => { OnAddField(typeof(Vector3)); });
                    _addFieldsGenericMenu.AddItem(new GUIContent("Vector2"), false, () => { OnAddField(typeof(Vector2)); });
                }
                _lastSelectedNode = node;
                _addFieldsGenericMenu.ShowAsContext();
            }
        }

        private void DrawCollapseChevron(Node node, Event current, EditorWindow window, Rect nodePosition, float chevronBackdropHeight)
        {
            var chevronSize = new Vector2(32f, 32f);
            var chevronPosition = new Rect(
                new Vector2(
                    nodePosition.x + nodePosition.width * 0.5f - chevronSize.x * 0.5f,
                    nodePosition.y + nodePosition.height - chevronSize.y * 0.25f
                ),
                chevronSize);

            var chevronBackdropPosition =
                new Rect(nodePosition.x, nodePosition.y + nodePosition.height, nodePosition.width, chevronBackdropHeight);
            chevronBackdropPosition.x = node.Position.x;
            chevronBackdropPosition.width = node.Position.width;

            GUIHelper.DrawQuad(chevronBackdropPosition, StyleProxy.ChevronUpBackdropColorNormal);

            if (chevronBackdropPosition.Contains(current.mousePosition) && current.type == EventType.MouseDown &&
                current.button == 0)
            {
                if (NodeCollapsedEvent != null)
                {
                    NodeCollapsedEvent(node);
                    window.Repaint();
                    current.Use();
                }
            }

            var color = GUI.color;
            GUI.color = StyleProxy.ChevronUpColor;
            GUI.DrawTexture(chevronPosition, node.IsCollapsed ? StyleProxy.ChevronDownTexture : StyleProxy.ChevronUpTexture);
            GUI.color = color;
        }

        private void DrawTitle(Node node, float titleHeight)
        {
            var titleBackdropBox = new Rect(node.Position.position, new Vector2(node.Position.width, titleHeight));
            GUIHelper.DrawQuad(titleBackdropBox, StyleProxy.NodeTitleBackdropColor);

            var titleBlockSize = StyleProxy.NodeTitleTextStyle.CalcSize(new GUIContent("NODE"));
            var titlePosition = new Rect(
                node.Position.x + node.Position.width * 0.5f - titleBlockSize.x * 0.5f,
                node.Position.y + 6f,
                titleBlockSize.x,
                16f);

            GUI.Box(titlePosition, "NODE", StyleProxy.NodeTitleTextStyle);

            var subtitleBlockSize = StyleProxy.NodeSubtitleTextStyle.CalcSize(new GUIContent("COMPONENT"));
            var subtitlePosition = new Rect(
                node.Position.x + node.Position.width * 0.5f - subtitleBlockSize.x * 0.5f,
                titleBackdropBox.y + titleBackdropBox.height - subtitleBlockSize.y - 6f,
                subtitleBlockSize.x,
                16f);

            GUI.Box(subtitlePosition, "COMPONENT", StyleProxy.NodeSubtitleTextStyle);
        }

        private void HandleDrag(Node mouseOverNode, Event current, EditorWindow window)
        {
            var newPosition = SnapPositionToGrid();

            switch (current.type)
            {
                case EventType.MouseDown:
                    if (mouseOverNode != null)
                    {
                        _draggingNode = mouseOverNode;
                        _initialDragPosition = current.mousePosition;
                        _draggingOffset = mouseOverNode.Position.position - current.mousePosition;
                        current.Use();
                    }
                    break;

                case EventType.MouseUp:
                    if (_draggingNode != null && NodeUpdatedPositionEvent != null)
                    {
                        NodeUpdatedPositionEvent(_draggingNode, newPosition, true);
                    }
                    _draggingNode = null;
                    break;

                case EventType.MouseMove:
                case EventType.MouseDrag:
                    if (_draggingNode != null)
                    {
                        _draggingOffset += current.delta;
                        if (NodeUpdatedPositionEvent != null)
                        {
                            NodeUpdatedPositionEvent(_draggingNode, newPosition, false);
                        }
                        current.Use();
                        window.Repaint();
                    }
                    break;
            }
        }

        private Vector2 SnapPositionToGrid()
        {
            var gridSnap = new Vector2(16f, 16f);

            var newPosition = _initialDragPosition + _draggingOffset;
            var snapIncrements = new Vector2(
                Mathf.Round(newPosition.x / gridSnap.x),
                Mathf.Round(newPosition.y / gridSnap.y));

            newPosition = new Vector2(snapIncrements.x * gridSnap.x, snapIncrements.y * gridSnap.y);
            return newPosition;
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

        private void OnAddField(Type fieldType)
        {
            if (NodeAddedFieldEvent != null)
            {
                NodeAddedFieldEvent(_lastSelectedNode, fieldType);
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