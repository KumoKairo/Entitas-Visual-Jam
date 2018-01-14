using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeAreaBackgroundDrawer
    {
        public Vector2 LastClickPosition;

        public void HandleRightClick(Rect appViewPosition, Event currentEvent, GenericMenu contextMenu)
        {
            if (currentEvent.type == EventType.MouseDown 
                && currentEvent.button == 1)
            {
                LastClickPosition = NodeAreaMediator.SnapDragPositionToGrid(currentEvent.mousePosition);
                currentEvent.Use();
                contextMenu.ShowAsContext();
            }
        }
    }
}
