using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeFieldsAdditonWidgetDrawer
    {
        private Rect _lastViewRect;

        public void OnGUI(EditorWindow appView, Rect rect)
        {
            _lastViewRect = rect;

            GUIHelper.DrawQuad(rect, StyleProxy.SemiTransparentBlackColor);

            var fieldsTextSize = StyleProxy.NodeFieldNameStyle.CalcSize(new GUIContent("FIELDS"));
            var plusIconSize = new Vector2(32f, 32f);

            var plusIconPosition = new Rect(
                rect.x + rect.width - plusIconSize.x,
                rect.y,
                plusIconSize.x,
                plusIconSize.y
            );

            var color = GUI.color;
            GUI.color = StyleProxy.NodeFieldNameTextColor;
            GUI.DrawTexture(plusIconPosition, StyleProxy.PlusIconTexture);
            GUI.color = color;

            var fieldsTextPosition = new Rect(
                rect.x + 8f,
                rect.y + fieldsTextSize.y * 0.5f - 2f,
                fieldsTextSize.x,
                fieldsTextSize.y
            );

            GUI.Box(fieldsTextPosition, "FIELDS", StyleProxy.NodeFieldNameStyle);
        }

        public void HandleEvents(Event currentEvent, GenericMenu addFieldsGenericMenu)
        {
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && _lastViewRect.Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();
                addFieldsGenericMenu.ShowAsContext();
            }
        }
    }
}
