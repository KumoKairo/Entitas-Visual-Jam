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
        private GUIContent _titleContent;

        public void OnGUI(EditorWindow appView, Rect titleRect, Node node)
        {
            _lastLinkedNode = node;

            GUIHelper.DrawQuad(titleRect, StyleProxy.NodeTitleBackdropColor);

            var titlePosition = new Rect(titleRect.position, new Vector2(titleRect.width, 16f));
            titlePosition.y += 6f;
            _lastTitleRect = titlePosition;

            var hotTitleRect = GetHotRect(_lastTitleRect,
                GUIHelper.GetOrCreateOrUpdateGUIContentFor(node.Name, ref _titleContent));

            var titleStyle = hotTitleRect.Contains(Event.current.mousePosition)
                ? StyleProxy.NodeTitleTextStyleHover
                : StyleProxy.NodeTitleTextStyleNormal;

            if (IsRenaming)
            {
                var color = GUI.skin.settings.selectionColor;
                GUI.skin.settings.selectionColor = StyleProxy.NodeTitleRenamingBackdropColor;

                GUI.SetNextControlName(_originalName);

                node.Name = EditorGUI.TextField(titlePosition, node.Name, StyleProxy.NodeTitleTextStyleNormal);
                GUI.skin.settings.selectionColor = color;

                if (GUI.GetNameOfFocusedControl() != _originalName)
                {
                    EditorGUI.FocusTextInControl(_originalName);
                }
            }
            else
            {
                GUI.Box(titlePosition, node.Name, titleStyle);
            }

            var subtitlePosition = new Rect(titleRect.x, titleRect.y + titleRect.height - 20f, titleRect.width, 16f);

            GUI.Box(subtitlePosition, "COMPONENT", StyleProxy.NodeSubtitleTextStyle);
        }

        private Rect GetHotRect(Rect referenceRect, GUIContent content)
        {
            var hotRect = new Rect(referenceRect);
            hotRect.width = StyleProxy.NodeTitleTextStyleNormal.CalcSize(content).x;
            hotRect.x += (referenceRect.width - hotRect.width) * 0.5f;
            return hotRect;
        }

        public bool HandleOnGUI(Event currentEvent)
        {
            if (IsRenaming)
            {
                bool shoudlUseEvent = false;
                bool hasTitleChanged = false;

                switch (currentEvent.keyCode)
                {
                    case KeyCode.Escape:
                        _lastLinkedNode.Name = _originalName;
                        shoudlUseEvent = true;
                        break;
                    case KeyCode.Return:
                        shoudlUseEvent = true;
                        hasTitleChanged = true;
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
                    GUI.FocusControl("");
                }

                if (hasTitleChanged)
                {
                    return true;
                }
            }
            else
            {
                var hotTitleRect = GetHotRect(_lastTitleRect,
                    GUIHelper.GetOrCreateOrUpdateGUIContentFor(_lastLinkedNode.Name, ref _titleContent));

                if (hotTitleRect.Contains(currentEvent.mousePosition) 
                    && currentEvent.button == 0
                    && currentEvent.clickCount == 2)
                {
                    StartRenaming();
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