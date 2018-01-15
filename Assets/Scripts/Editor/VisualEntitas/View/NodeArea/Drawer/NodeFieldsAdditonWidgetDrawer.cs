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

            GUIHelper.DrawQuad(rect, StyleProxy.SemiTransparentBlackColor);

            var fieldsTextSize = StyleProxy.NodeFieldNameStyleNormal.CalcSize(new GUIContent("FIELDS"));
            var plusIconSize = new Vector2(32f, 32f);

            var plusIconRect = new Rect(
                rect.x + rect.width - plusIconSize.x,
                rect.y,
                plusIconSize.x,
                plusIconSize.y
            );

            _lastViewRect = plusIconRect;

            var color = GUI.color;
            GUI.color = plusIconRect.Contains(Event.current.mousePosition)
                ? StyleProxy.NodeFieldNameTextColorHover
                : StyleProxy.NodeFieldNameTextColorNormal;

            GUI.DrawTexture(plusIconRect, StyleProxy.PlusIconTexture);
            GUI.color = color;

            var fieldsTextPosition = new Rect(
                rect.x + 8f,
                rect.y + fieldsTextSize.y * 0.5f - 2f,
                fieldsTextSize.x,
                fieldsTextSize.y
            );

            GUI.Box(fieldsTextPosition, "FIELDS", StyleProxy.NodeFieldNameStyleNormal);
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
