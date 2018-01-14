using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeCollapseChevronDrawer
    {
        private Rect _lastDrawRect;

        public void OnGUI(EditorWindow appView, Rect rect, bool isNodeCollapsed)
        {
            _lastDrawRect = rect;

            GUIHelper.DrawQuad(rect, StyleProxy.ChevronUpBackdropColorNormal);

            var chevronIconSize = new Vector2(32f, 32f);
            var chevronPosition = new Vector2(
                rect.x + rect.width * 0.5f - chevronIconSize.x * 0.5f,
                rect.y + rect.height * 0.5f - chevronIconSize.y * 0.5f);

            var chevronIconRect = new Rect(chevronPosition, chevronIconSize);

            var color = GUI.color;
            GUI.color = StyleProxy.ChevronUpColor;
            GUI.DrawTexture(chevronIconRect, isNodeCollapsed ? StyleProxy.ChevronDownTexture : StyleProxy.ChevronUpTexture);
            GUI.color = color;
        }

        public bool HandleEvents(Event current)
        {
            if (current.type == EventType.MouseDown && _lastDrawRect.Contains(current.mousePosition))
            {
                current.Use();
                return true;
            }

            return false;
        }
    }
}
