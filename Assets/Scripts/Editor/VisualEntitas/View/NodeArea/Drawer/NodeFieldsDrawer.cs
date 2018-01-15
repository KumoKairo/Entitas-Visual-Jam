using System.Collections.Generic;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeFieldsDrawer
    {
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

        public Field HandleEvents(Event currentEvent, Node node)
        {
            Field deletedField = null;

            for (int i = 0; i < node.Fields.Count; i++)
            {
                var field = node.Fields[i];
                var isDeleted = _fieldToDrawer[field].HandleEvent(currentEvent);
                deletedField = isDeleted ? field : deletedField;
            }

            return deletedField;
        }

        public void HandleFieldRemoval(Field field)
        {
            if (_fieldToDrawer.ContainsKey(field))
            {
                _fieldToDrawer.Remove(field);
            }
        }

        public void HandleFieldAddition(Field addedField)
        {
            Debug.Log("Added new drawer");
            _fieldToDrawer[addedField] = new NodeSeparateFieldDrawer(addedField);
        }
    }
}
