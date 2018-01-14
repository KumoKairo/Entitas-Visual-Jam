using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeFieldsAdditonWidgetDrawer
    {
        public void OnGUI(EditorWindow appView, Rect rect)
        {
            GUIHelper.DrawQuad(rect, StyleProxy.SemiTransparentBlackColor);

            var fieldsTextSize = StyleProxy.NodeFieldsTextStyle.CalcSize(new GUIContent("FIELDS"));
            var plusIconSize = new Vector2(32f, 32f);

            var plusIconPosition = new Rect(
                rect.x + rect.width - plusIconSize.x,
                rect.y,
                plusIconSize.x,
                plusIconSize.y
            );

            var color = GUI.color;
            GUI.color = StyleProxy.NodeFieldsTextColor;
            GUI.DrawTexture(plusIconPosition, StyleProxy.PlusIconTexture);
            GUI.color = color;

            var fieldsTextPosition = new Rect(
                rect.x + 8f,
                rect.y + fieldsTextSize.y * 0.5f - 2f,
                fieldsTextSize.x,
                fieldsTextSize.y
            );

            GUI.Box(fieldsTextPosition, "FIELDS", StyleProxy.NodeFieldsTextStyle);
        }
    }
}
