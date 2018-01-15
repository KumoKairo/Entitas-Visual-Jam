using System.Collections.Generic;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeFieldsDrawer
    {
        private Dictionary<string, GUIContent> _stringToGuiContent = new Dictionary<string, GUIContent>();
        private Dictionary<Field, Rect> _fieldToMinusButtonRect = new Dictionary<Field, Rect>();

        private Dictionary<Field, NodeSeparateFieldDrawer> _fieldToDrawer = new Dictionary<Field, NodeSeparateFieldDrawer>();

        public NodeFieldsDrawer(Node node)
        {
            foreach (var nodeField in node.Fields)
            {
                _fieldToDrawer[nodeField] = new NodeSeparateFieldDrawer(nodeField);
            }
        }

        public void OnGUI(Rect rect, float fieldHeight, Node node)
        {

            for (int i = 0; i < node.Fields.Count; i++)
            {
                var field = node.Fields[i];
                var fieldBackdropPosition = new Rect(rect.x, rect.y + fieldHeight * i, rect.width, fieldHeight);
                _fieldToDrawer[field].Draw(fieldBackdropPosition, i);

                
            }
        }

        

        public Field HandleEvents(Event currentEvent)
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
