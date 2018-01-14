using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class NodeBackgroundDrawer
    {
        private Rect _lastDrawRect;

        private Vector2 _initialDragPosition;
        private Vector2 _draggingOffset;
        private bool _isDragging;

        public void OnGUI(EditorWindow appView, Rect viewBounds, float chevronBackdropHeight)
        {
            _lastDrawRect = viewBounds;

            GUIHelper.DrawQuad(viewBounds, StyleProxy.NodeBackgroundColor);

            float shadowOffset = 8f;
            GUIHelper.DrawQuad(new Rect(
                    viewBounds.x + shadowOffset,
                    viewBounds.y + viewBounds.height + chevronBackdropHeight,
                    viewBounds.width,
                    shadowOffset),
                StyleProxy.SemiTransparentBlackColor);

            GUIHelper.DrawQuad(new Rect(
                    viewBounds.x + viewBounds.width,
                    viewBounds.y + shadowOffset,
                    shadowOffset,
                    viewBounds.height + shadowOffset),
                StyleProxy.SemiTransparentBlackColor);
        }

        /// <returns>Tuple - first parameter is whether we have dragged this "frame". 
        /// Second parameter is whether we have completed dragging</returns>
        public Tuple<bool, bool> HandleDrag(Event currentEvent, out Vector2 newDragPosition)
        {
            var returnValue = new Tuple<bool, bool>(false, false);
            newDragPosition = NodeAreaMediator.SnapDragPositionToGrid(_initialDragPosition + _draggingOffset);

            var currentMousePosition = currentEvent.mousePosition;
            if (_lastDrawRect.Contains(currentMousePosition) 
                && currentEvent.type == EventType.MouseDown 
                && currentEvent.button == 0)
            {
                _initialDragPosition = currentMousePosition;
                _draggingOffset = _lastDrawRect.position - currentMousePosition;
                _isDragging = true;
                currentEvent.Use();
            }

            if (_isDragging && currentEvent.button == 0)
            {
                switch (currentEvent.type)
                {
                    case EventType.MouseDrag:
                        _draggingOffset += currentEvent.delta;
                        currentEvent.Use();
                        returnValue.First = true;
                        return returnValue;
                    case EventType.MouseUp:
                        _isDragging = false;
                        currentEvent.Use();
                        returnValue.Second = true;
                        GUIUtility.hotControl = 0;
                        return returnValue;
                }
            }

            return returnValue;
        }

        public void HandleRightClick(Event currentEvent, GenericMenu rightClickNodeMenu)
        {
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1 &&
                _lastDrawRect.Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();
                rightClickNodeMenu.ShowAsContext();
            }
        }
    }
}