using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeTitleDrawer
    {
        public bool IsRenaming { get; private set; }

        private Node _lastLinkedNode;
        private Rect _lastTitleRect;

        private string _originalName;

        public void OnGUI(EditorWindow appView, Rect titleRect, Node node)
        {
            _lastLinkedNode = node;

            GUIHelper.DrawQuad(titleRect, StyleProxy.NodeTitleBackdropColor);

            var titlePosition = new Rect(titleRect.position, new Vector2(titleRect.width, 16f));
            titlePosition.y += 6f;
            _lastTitleRect = titlePosition;

            if (IsRenaming)
            {
                var color = GUI.color;
                GUI.color = StyleProxy.BoldTransparentBlackColor;
                node.Name = EditorGUI.TextField(titlePosition, node.Name, StyleProxy.NodeTitleTextStyle);
                GUI.color = color;
            }
            else
            {
                GUI.Box(titlePosition, node.Name, StyleProxy.NodeTitleTextStyle);
            }

            var subtitlePosition = new Rect(titleRect.x, titleRect.y + titleRect.height - 20f, titleRect.width, 16f);

            GUI.Box(subtitlePosition, "COMPONENT", StyleProxy.NodeSubtitleTextStyle);
        }

        public bool HandleOnGUI(Event currentEvent)
        {
            if (IsRenaming)
            {
                bool shoudlUseEvent = false;
                bool shouldReturnTrue = false;

                switch (currentEvent.keyCode)
                {
                    case KeyCode.Escape:
                        _lastLinkedNode.Name = _originalName;
                        shoudlUseEvent = true;
                        break;
                    case KeyCode.Return:
                        shoudlUseEvent = true;
                        shouldReturnTrue = true;
                        break;
                }

                if (!_lastTitleRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown)
                {
                    _lastLinkedNode.Name = _originalName;
                    shoudlUseEvent = true;
                }

                if (shoudlUseEvent)
                {
                    IsRenaming = false;
                    currentEvent.Use();
                }

                if (shouldReturnTrue)
                {
                    return true;
                }
            }

            return false;
        }

        public void StartRenaming()
        {
            IsRenaming = true;
            _originalName = _lastLinkedNode.Name;
        }
    }
}