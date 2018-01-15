using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
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

        public NodeSeparateFieldDrawer(Field field)
        {
            _field = field;
        }

        public void Draw(Rect contentRect, int i)
        {
            var minusIconSize = new Vector2(32f, 32f);

            _lastHoveredOver = null;

            if (i % 2 != 0)
            {
                GUIHelper.DrawQuad(contentRect, StyleProxy.TransparentBlackColor);
            }

            var splitType = _field.Type.Split('.');
            var displayFieldType = splitType[splitType.Length - 1];

            var fieldNameContent = GetOrCreateGuiContent(ref _fiendTypeContent, _field.Name);
            var fieldTypeContent = GetOrCreateGuiContent(ref _fiendNameContent, displayFieldType);

            var fieldNameRect = new Rect(contentRect);
            fieldNameRect.x += 8f;

            var minusIconCenter = contentRect.y + contentRect.height * 0.5f - minusIconSize.y * 0.5f;
            var minusIconRect = new Rect(
                new Vector2(contentRect.x + contentRect.width - minusIconSize.x, minusIconCenter),
                new Vector2(minusIconSize.x, minusIconSize.y)
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

            _lastHoveredOver = DrawHotText(fieldNameContent, fieldNameRect,
                StyleProxy.NodeFieldNameStyleNormal,
                StyleProxy.NodeFieldNameStyleHover) ? HoverType.Name : _lastHoveredOver;

            _lastHoveredOver = DrawHotText(fieldTypeContent, fieldTypeRect,
                StyleProxy.NodeFieldTypeStyleNormal,
                StyleProxy.NodeFieldTypeStyleHover, true) ? HoverType.Type : _lastHoveredOver;
        }

        /// Returns whether we have clicked on a minus (delete) sign
        public bool HandleEvent(Event current)
        {
            if (_lastHoveredOver != null && current.type == EventType.MouseDown)
            {
                Debug.Log("Renaming " + _lastHoveredOver + " " + _field.Name);
                //IsRenaming = true;
                current.Use();
            }

            if (current.type == EventType.MouseDown && current.button == 0)
            {
                if (_minusIconHotRect.Contains(current.mousePosition))
                {
                    current.Use();
                    return true;
                }
            }

            return false;
        }

        private GUIContent GetOrCreateGuiContent(ref GUIContent content, string name)
        {
            if (content == null)
            {
                content = new GUIContent(name);
            }

            return content;
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

        private void DrawHotFieldType(Field field, Rect rect, GUIContent fieldTypeContent, Rect fieldTypeRect, Vector2 minusIconSize)
        {
            var fieldTypeSize = StyleProxy.NodeFieldNameStyleNormal.CalcSize(fieldTypeContent);
            var hotTypeRect = new Rect(fieldTypeRect);
            hotTypeRect.width = fieldTypeSize.x;
            hotTypeRect.x = rect.x + rect.width - hotTypeRect.width - minusIconSize.x;

            var fieldTypeStyle = StyleProxy.NodeFieldTypeStyleNormal;
            if (hotTypeRect.Contains(Event.current.mousePosition))
            {
                fieldTypeStyle = StyleProxy.NodeFieldTypeStyleHover;
            }

            GUI.Box(fieldTypeRect, fieldTypeContent, fieldTypeStyle);
        }
    }
}