using System.Collections.Generic;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeFieldsDrawer
    {
        private Dictionary<string, GUIContent> _stringToGuiContent = new Dictionary<string, GUIContent>();

        public void OnGUI(Rect rect, float fieldHeight, Node node)
        {
            var minusIconSize = new Vector2(32f, 32f);

            for (int i = 0; i < node.Fields.Count; i++)
            {
                var nodeField = node.Fields[i];

                var fieldBackdropPosition = new Rect(rect.x, rect.y + fieldHeight * i, rect.width, fieldHeight);
                if (i % 2 != 0)
                {
                    GUIHelper.DrawQuad(fieldBackdropPosition, StyleProxy.TransparentBlackColor);
                }

                var splitType = nodeField.Type.Split('.');
                var displayFieldType = splitType[splitType.Length - 1];
                var fieldTypeContent = StringToGuiContent(displayFieldType);
                var fieldTypeSize = StyleProxy.NodeFieldNameStyle.CalcSize(fieldTypeContent);

                var fieldNameContent = StringToGuiContent(nodeField.Name);
                var fieldNameSize = StyleProxy.NodeFieldNameStyle.CalcSize(fieldNameContent);

                var fieldNameRect = new Rect(
                    fieldBackdropPosition.position,
                    new Vector2(fieldNameSize.x, fieldNameSize.y));

                fieldNameRect.y -= 1f;
                fieldNameRect.x += 8f;

                var minusIconCenter = fieldBackdropPosition.y + fieldBackdropPosition.height * 0.5f - minusIconSize.y * 0.5f;
                var minusIconPosition = new Rect(
                    new Vector2(fieldBackdropPosition.x + fieldBackdropPosition.width - minusIconSize.x, minusIconCenter),
                    new Vector2(minusIconSize.x, minusIconSize.y)
                );

                var fieldTypeRect = new Rect(
                    new Vector2(minusIconPosition.x - fieldTypeSize.x, fieldNameRect.y),
                    fieldTypeSize
                );

                var color = GUI.color;
                GUI.color = StyleProxy.MinusIconColor;
                GUI.DrawTexture(minusIconPosition, StyleProxy.MinusIconTexture);
                GUI.color = color;

                GUI.Box(fieldNameRect, fieldNameContent, StyleProxy.NodeFieldNameStyle);
                GUI.Box(fieldTypeRect, fieldTypeContent, StyleProxy.NodeFieldTypeStyle);
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
