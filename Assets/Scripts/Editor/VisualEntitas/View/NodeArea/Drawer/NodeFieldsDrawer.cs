using System;
using System.Collections.Generic;
using Entitas.Visual.Model;
using Entitas.Visual.Model.VO;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeFieldsDrawer
    {
        public bool IsRenaming { get; private set; }

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
            IsRenaming = false;
            
            for (int i = 0; i < node.Fields.Count; i++)
            {
                var field = node.Fields[i];
                var fieldBackdropPosition = new Rect(rect.x, rect.y + fieldHeight * i, rect.width, fieldHeight);
                var fieldDrawer = _fieldToDrawer[field];
                fieldDrawer.Draw(fieldBackdropPosition, i);
                IsRenaming = fieldDrawer.IsRenaming || IsRenaming;
            }
        }

        public Field HandleEvents(Event currentEvent, Node node, out Field renamedField)
        {
            Field deletedField = null;
            renamedField = null;

            NodeSeparateFieldDrawer renamingDrawer = null;

            foreach (var drawer in _fieldToDrawer)
            {
                if (drawer.Value.IsRenaming)
                {
                    renamingDrawer = drawer.Value;
                    break;
                }
            }

            for (int i = 0; i < node.Fields.Count; i++)
            {
                var field = node.Fields[i];

                var fieldDrawer = _fieldToDrawer[field];
                if (renamingDrawer == null || renamingDrawer == fieldDrawer)
                {
                    bool hasSuccessfullyRenamedField;
                    var isDeleted = _fieldToDrawer[field].HandleEvent(currentEvent, out hasSuccessfullyRenamedField);
                    deletedField = isDeleted ? field : deletedField;
                    renamedField = hasSuccessfullyRenamedField ? field : renamedField;
                }
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
            _fieldToDrawer[addedField] = new NodeSeparateFieldDrawer(addedField);
        }

        public void OnRegister(FieldTypeProviderProxy typeProviderProxy, Action<Field, Type> onFieldTypeChanged)
        {
            foreach (var drawer in _fieldToDrawer)
            {
                drawer.Value.OnRegister(typeProviderProxy, onFieldTypeChanged);
            }
        }
    }
}
