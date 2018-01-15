using System.Collections.Generic;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeFieldsDrawer
    {
        private enum HoverOver
        {
            Name,
            Type
        }

        public bool IsRenaming { get; private set; }

        private Tuple<Field, HoverOver>? _lastHoverOver;

        private Dictionary<string, GUIContent> _stringToGuiContent = new Dictionary<string, GUIContent>();
        private Dictionary<Field, Rect> _fieldToMinusButtonRect = new Dictionary<Field, Rect>();

        public void OnGUI(Rect rect, float fieldHeight, Node node)
        {
            _lastHoverOver = null;

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
                var fieldNameContent = StringToGuiContent(field.Name);

                var fieldNameRect = new Rect(fieldBackdropPosition);
                fieldNameRect.x += 8f;

                var minusIconCenter = fieldBackdropPosition.y + fieldBackdropPosition.height * 0.5f - minusIconSize.y * 0.5f;
                var minusIconRect = new Rect(
                    new Vector2(fieldBackdropPosition.x + fieldBackdropPosition.width - minusIconSize.x, minusIconCenter),
                    new Vector2(minusIconSize.x, minusIconSize.y)
                );

                _fieldToMinusButtonRect[field] = new Rect(minusIconRect.x, fieldBackdropPosition.y, minusIconRect.width, fieldHeight);

                var fieldTypeRect = new Rect(fieldBackdropPosition);
                fieldTypeRect.x -= minusIconRect.width;

                var color = GUI.color;
                GUI.color = StyleProxy.MinusIconColor;
                GUI.DrawTexture(minusIconRect, StyleProxy.MinusIconTexture);
                GUI.color = color;

                DrawHotFieldName(field, fieldNameContent, fieldNameRect);
                DrawHotFieldType(field, rect, fieldTypeContent, fieldTypeRect, minusIconSize);
            }
        }

        private void DrawHotFieldName(Field field, GUIContent fieldNameContent, Rect fieldNameRect)
        {
            var fieldNameSize = StyleProxy.NodeFieldNameStyleNormal.CalcSize(fieldNameContent);
            var hotFieldRect = new Rect(fieldNameRect);
            hotFieldRect.width = fieldNameSize.x;

            var fieldNameStyle = StyleProxy.NodeFieldNameStyleNormal;
            if (hotFieldRect.Contains(Event.current.mousePosition))
            {
                _lastHoverOver = new Tuple<Field, HoverOver>(field, HoverOver.Name);
                fieldNameStyle = StyleProxy.NodeFieldNameStyleHover;
            }

            if (IsRenaming && _lastHoverOver != null && _lastHoverOver.Value.First == field)
            {
                fieldNameStyle = StyleProxy.NodeFieldNameStyleHover;
                field.Name = EditorGUI.TextField(fieldNameRect, field.Name, fieldNameStyle);
            }
            else
            {
                GUI.Box(fieldNameRect, fieldNameContent, fieldNameStyle);
            }
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
                _lastHoverOver = new Tuple<Field, HoverOver>(field, HoverOver.Type);
                fieldTypeStyle = StyleProxy.NodeFieldTypeStyleHover;
            }

            GUI.Box(fieldTypeRect, fieldTypeContent, fieldTypeStyle);
        }

        public Field HandleFieldDeletionClick(Event currentEvent)
        {
            Field deletedField = null;

            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                if (_lastHoverOver != null && _lastHoverOver.Value.Second == HoverOver.Name)
                {
                    IsRenaming = true;
                    currentEvent.Use();
                    return null;
                }
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

            if ((currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.Escape))
            {
                IsRenaming = false;
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
