using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeSeparateFieldDrawer
    {
        private Field _field;

        private GUIContent _fiendNameContent;
        private GUIContent _fiendTypeContent;

        public NodeSeparateFieldDrawer(Field field)
        {
            _field = field;
        }

        public void Draw(Rect contentRect, int i)
        {
            var minusIconSize = new Vector2(32f, 32f);

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

            var color = GUI.color;
            GUI.color = minusIconRect.Contains(Event.current.mousePosition)
                ? StyleProxy.MinusIconColorHover
                : StyleProxy.MinusIconColorNormal;

            GUI.DrawTexture(minusIconRect, StyleProxy.MinusIconTexture);
            GUI.color = color;

            var fieldTypeRect = new Rect(contentRect);
            fieldTypeRect.x -= minusIconRect.width;

            DrawHotText(fieldNameContent, fieldNameRect, 
                StyleProxy.NodeFieldNameStyleNormal,
                StyleProxy.NodeFieldNameStyleHover);

            DrawHotText(fieldTypeContent, fieldTypeRect,
                StyleProxy.NodeFieldTypeStyleNormal,
                StyleProxy.NodeFieldTypeStyleHover, true);
        }

        public void HandleEvent(Event current)
        {

        }

        private GUIContent GetOrCreateGuiContent(ref GUIContent content, string name)
        {
            if (content == null)
            {
                content = new GUIContent(name);
            }

            return content;
        }

        private void DrawHotText(GUIContent content, Rect contentRect, GUIStyle styleNormal, GUIStyle styleHover, bool alignRight = false)
        {
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
                style = styleHover;
            }

            GUI.Box(contentRect, content, style);
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