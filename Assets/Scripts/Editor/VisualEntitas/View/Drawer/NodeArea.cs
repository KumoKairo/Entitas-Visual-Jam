using System;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeArea : IOnGuiView
    {
        public event Action CreateNewComponentEvent;

        private static int RightClickControlID = "NodeAreaRightClick".GetHashCode();

        private GenericMenu _genericMenu;

        public void OnGUI(EditorWindow window)
        {
            if (_genericMenu == null)
            {
                _genericMenu = new GenericMenu();
                _genericMenu.AddItem(new GUIContent("Component"), false, OnCreateComponentMenuSelected);
            }
            int controlId = GUIUtility.GetControlID(RightClickControlID, FocusType.Passive);
            var current = Event.current;
            if (current.button != 1 && (current.button != 0 || !current.alt))
            {
                return;
            }

            switch (current.GetTypeForControl(controlId))
            {
                case EventType.MouseDown:
                    GUIUtility.hotControl = controlId;
                    _genericMenu.ShowAsContext();
                    current.Use();
                    break;
            }
        }

        private void OnCreateComponentMenuSelected()
        {
            if (CreateNewComponentEvent != null)
            {
                CreateNewComponentEvent();
            }
        }
    }
}