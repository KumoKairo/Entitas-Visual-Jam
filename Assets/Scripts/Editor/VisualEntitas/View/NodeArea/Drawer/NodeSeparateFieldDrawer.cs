using System;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeSeparateFieldDrawer
    {
        private enum HoverType
        {
            Name,
            Type
        }

        public bool IsRenaming { get; private set; }

        private Field _field;

        private GUIContent _fiendNameContent;
        private GUIContent _fiendTypeContent;

        private HoverType? _lastHoveredOver;
        private Rect _minusIconHotRect;
        private string _initialFieldName;

        private GenericMenu _changeFieldTypeGenericMenu;
        private Vector2 _minusIconSize = new Vector2(32f, 32f);
        private const float _leftMargin = 8f;
        private const float _fieldNameToFieldTypeMargin = 16f;

        public NodeSeparateFieldDrawer(Field field)
        {
            _field = field;
        }

        public void Draw(Rect contentRect, int i)
        {
            _lastHoveredOver = null;

            if (i % 2 != 0)
            {
                GUIHelper.DrawQuad(contentRect, StyleProxy.TransparentBlackColor);
            }

            var splitType = _field.Type.Split('.');
            var displayFieldType = splitType[splitType.Length - 1];

            var fieldNameContent = GUIHelper.GetOrCreateOrUpdateGUIContentFor(_field.Name, ref _fiendTypeContent);
            var fieldTypeContent = GUIHelper.GetOrCreateOrUpdateGUIContentFor(displayFieldType, ref _fiendNameContent);

            var fieldNameRect = new Rect(contentRect);
            fieldNameRect.x += _leftMargin;

            var minusIconCenter = contentRect.y + contentRect.height * 0.5f - _minusIconSize.y * 0.5f;
            var minusIconRect = new Rect(
                new Vector2(contentRect.x + contentRect.width - _minusIconSize.x, minusIconCenter),
                new Vector2(_minusIconSize.x, _minusIconSize.y)
            );

            _minusIconHotRect = minusIconRect;
            _minusIconHotRect.height = contentRect.height - 1f;
            _minusIconHotRect.y += minusIconRect.height - _minusIconHotRect.height;

            var color = GUI.color;
            GUI.color = _minusIconHotRect.Contains(Event.current.mousePosition)
                ? StyleProxy.MinusIconColorHover
                : StyleProxy.MinusIconColorNormal;

            GUI.DrawTexture(minusIconRect, StyleProxy.MinusIconTexture);
            GUI.color = color;

            var fieldTypeRect = new Rect(contentRect);
            fieldTypeRect.x -= minusIconRect.width;

            if (IsRenaming)
            {
                var controlRectName = contentRect.ToString();
                GUI.SetNextControlName(controlRectName);

                var selectionColor = GUI.skin.settings.selectionColor;
                var cursorColor = GUI.skin.settings.cursorColor;

                GUI.skin.settings.selectionColor = StyleProxy.NodeFieldNameTextRenamingBackgroundColor;
                GUI.skin.settings.cursorColor = StyleProxy.NodeFieldNameTextColorNormal;

                _field.Name = EditorGUI.TextField(fieldNameRect, _field.Name, StyleProxy.NodeFieldNameStyleHover);

                GUI.skin.settings.selectionColor = selectionColor;
                GUI.skin.settings.cursorColor = cursorColor;

                if (GUI.GetNameOfFocusedControl() != controlRectName)
                {
                    EditorGUI.FocusTextInControl(controlRectName);
                }
            }
            else
            {
                _lastHoveredOver = DrawHotText(fieldNameContent, fieldNameRect,
                    StyleProxy.NodeFieldNameStyleNormal,
                    StyleProxy.NodeFieldNameStyleHover) ? HoverType.Name : _lastHoveredOver;
            }

            _lastHoveredOver = DrawHotText(fieldTypeContent, fieldTypeRect,
                StyleProxy.NodeFieldTypeStyleNormal,
                StyleProxy.NodeFieldTypeStyleHover, true) ? HoverType.Type : _lastHoveredOver;
        }

        internal void OnRegister(FieldTypeProviderProxy typeProviderProxy, Action<Field, Type> onFieldTypeChanged)
        {
            _changeFieldTypeGenericMenu = new GenericMenu();
            foreach (var type in typeProviderProxy.FieldTypeProviderData.Types)
            {
                var localType = type;
                _changeFieldTypeGenericMenu.AddItem(new GUIContent(type.Name), false, () => onFieldTypeChanged(_field, localType));
            }
        }

        public float GetCurrentDesiredWidth()
        {
            return CalculateCurrentDesiredWidth();
        }

        /// Returns whether we have clicked on a minus (delete) sign (FIRST)
        public bool HandleEvent(Event current, out bool hasSuccessfullyRenamedField)
        {
            hasSuccessfullyRenamedField = false;

            if (_lastHoveredOver != null 
                && _lastHoveredOver.Value == HoverType.Name
                && current.type == EventType.MouseDown 
                && current.clickCount == 2
                && current.button == 0)
            {
                IsRenaming = true;
                _initialFieldName = _field.Name;
                current.Use();
            }

            if (_lastHoveredOver != null
                && _lastHoveredOver.Value == HoverType.Type
                && current.type == EventType.MouseDown
                && current.button == 0)
            {
                current.Use();
                _changeFieldTypeGenericMenu.ShowAsContext();
            }

            if (IsRenaming)
            {
                bool shouldUseEvent = false;
                bool shouldContinueRenaming = true;

                var isNotHoveringOverOrHoveringOverType =
                    _lastHoveredOver == null || _lastHoveredOver.Value == HoverType.Type;

                if (isNotHoveringOverOrHoveringOverType && current.type == EventType.MouseDown 
                    || current.keyCode == KeyCode.Escape)
                {
                    shouldContinueRenaming = false;
                    shouldUseEvent = true;
                    _field.Name = _initialFieldName;
                }
                else if (current.keyCode == KeyCode.Return)
                {
                    hasSuccessfullyRenamedField = true;
                    shouldContinueRenaming = false;
                    shouldUseEvent = true;
                }

                IsRenaming = shouldContinueRenaming;
                if (!IsRenaming)
                {
                    GUI.FocusControl("");
                }

                if (shouldUseEvent)
                {
                    current.Use();
                }
            }

            if (_minusIconHotRect.Contains(current.mousePosition)
                && current.type == EventType.MouseDown
                && current.button == 0)
            {
                current.Use();
                return true;
            }

            return false;
        }

        private float CalculateCurrentDesiredWidth()
        {
            var fieldNameSize = StyleProxy.NodeFieldNameStyleNormal.CalcSize(_fiendNameContent);
            var fieldTypeSuze = StyleProxy.NodeFieldTypeStyleNormal.CalcSize(_fiendTypeContent);

            return fieldNameSize.x + _fieldNameToFieldTypeMargin + fieldTypeSuze.x + _leftMargin + _minusIconSize.x ;
        }

        private bool DrawHotText(
            GUIContent content, 
            Rect contentRect, 
            GUIStyle styleNormal, 
            GUIStyle styleHover,
            bool alignRight = false)
        {
            bool isHover = false;

            var size = StyleProxy.NodeFieldNameStyleNormal.CalcSize(content);
            var hotRect = new Rect(contentRect);
            hotRect.width = size.x;
            if (alignRight)
            {
                hotRect.x = contentRect.x + contentRect.width - hotRect.width;
            }

            var style = styleNormal;
            if (hotRect.Contains(Event.current.mousePosition))
            {
                isHover = true;
                style = styleHover;
            }

            GUI.Box(contentRect, content, style);

            return isHover;
        }
    }
}