using System.Collections.Generic;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeFieldsDrawer
    {
        private Dictionary<string, GUIContent> _stringToGuiContent = new Dictionary<string, GUIContent>();
        private Dictionary<Field, Rect> _fieldToMinusButtonRect = new Dictionary<Field, Rect>();

        public void OnGUI(Rect rect, float fieldHeight, Node node)
        {
            var minusIconSize = new Vector2(32f, 32f);

            for (int i = 0; i < node.Fields.Count; i++)
            {
                var field = node.Fields[i];

                var fieldBackdropPosition = new Rect(rect.x, rect.y + fieldHeight * i, rect.width, fieldHeight);
                if (i % 2 != 0)
                {
                    GUIHelper.DrawQuad(fieldBackdropPosition, StyleProxy.TransparentBlackColor);
                }

                var splitType = field.Type.Split('.');
                var displayFieldType = splitType[splitType.Length - 1];
                var fieldTypeContent = StringToGuiContent(displayFieldType);
                var fieldTypeSize = StyleProxy.NodeFieldNameStyle.CalcSize(fieldTypeContent);

                var fieldNameContent = StringToGuiContent(field.Name);
                var fieldNameSize = StyleProxy.NodeFieldNameStyle.CalcSize(fieldNameContent);

                var fieldNameRect = new Rect(
                    fieldBackdropPosition.position,
                    new Vector2(fieldNameSize.x, fieldNameSize.y));

                fieldNameRect.y -= 1f;
                fieldNameRect.x += 8f;

                var minusIconCenter = fieldBackdropPosition.y + fieldBackdropPosition.height * 0.5f - minusIconSize.y * 0.5f;
                var minusIconRect = new Rect(
                    new Vector2(fieldBackdropPosition.x + fieldBackdropPosition.width - minusIconSize.x, minusIconCenter),
                    new Vector2(minusIconSize.x, minusIconSize.y)
                );

                _fieldToMinusButtonRect[field] = new Rect(minusIconRect.x, fieldBackdropPosition.y, minusIconRect.width, fieldHeight);

                var fieldTypeRect = new Rect(
                    new Vector2(minusIconRect.x - fieldTypeSize.x, fieldNameRect.y),
                    fieldTypeSize
                );

                var color = GUI.color;
                GUI.color = StyleProxy.MinusIconColor;
                GUI.DrawTexture(minusIconRect, StyleProxy.MinusIconTexture);
                GUI.color = color;

                GUI.Box(fieldNameRect, fieldNameContent, StyleProxy.NodeFieldNameStyle);
                GUI.Box(fieldTypeRect, fieldTypeContent, StyleProxy.NodeFieldTypeStyle);
            }
        }

        public Field HandleFieldDeletionClick(Event currentEvent)
        {
            Field deletedField = null;

            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                foreach (var rect in _fieldToMinusButtonRect)
                {
                    if (rect.Value.Contains(currentEvent.mousePosition))
                    {
                        currentEvent.Use();
                        deletedField = rect.Key;
                        break;
                    }
                }
            }

            return deletedField;
        }

        public void HandleFieldRemoval(Field field)
        {
            if (_fieldToMinusButtonRect.ContainsKey(field))
            {
                _fieldToMinusButtonRect.Remove(field);
            }
        }

        private GUIContent StringToGuiContent(string str)
        {
            if (_stringToGuiContent.ContainsKey(str))
            {
                return _stringToGuiContent[str];
            }

            var content = new GUIContent(str);
            _stringToGuiContent[str] = content;
            return content;
        }
    }
}
