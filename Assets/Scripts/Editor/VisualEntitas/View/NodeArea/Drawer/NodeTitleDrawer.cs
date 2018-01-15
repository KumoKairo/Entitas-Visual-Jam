using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeTitleDrawer
    {
        public bool IsRenaming { get; private set; }

        private Node _node;
        private Rect _lastTitleRect;

        private string _originalName;
        private GUIContent _titleContent;

        public void OnGUI(EditorWindow appView, Rect titleRect, Node node)
        {
            _node = node;

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
                var selectionColor = GUI.skin.settings.selectionColor;
                var cursorColor = GUI.skin.settings.cursorColor;

                GUI.skin.settings.selectionColor = StyleProxy.NodeTitleRenamingBackdropColor;
                GUI.skin.settings.cursorColor = StyleProxy.NodeTitleColorNormal;

                GUI.SetNextControlName(_originalName);

                node.Name = EditorGUI.TextField(titlePosition, node.Name, StyleProxy.NodeTitleTextStyleNormal);

                GUI.skin.settings.selectionColor = selectionColor;
                GUI.skin.settings.cursorColor = cursorColor;

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

        public bool HandleOnGUI(Event currentEvent, out float desiredWidth)
        {
            var hotTitleRect = GetHotRect(_lastTitleRect,
                GUIHelper.GetOrCreateOrUpdateGUIContentFor(_node.Name, ref _titleContent));

            desiredWidth = hotTitleRect.width + NodeMediator.TitleMargins * 2f;

            if (IsRenaming)
            {
                bool shoudlUseEvent = false;
                bool hasTitleChanged = false;

                switch (currentEvent.keyCode)
                {
                    case KeyCode.Escape:
                        _node.Name = _originalName;
                        shoudlUseEvent = true;
                        break;
                    case KeyCode.Return:
                        shoudlUseEvent = true;
                        hasTitleChanged = true;
                        break;
                }

                if (!_lastTitleRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown)
                {
                    _node.Name = _originalName;
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
            _originalName = _node.Name;
        }
    }
}