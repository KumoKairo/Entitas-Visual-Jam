using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class DragWindowTest : EditorWindow
{
    [MenuItem("Window/TestDragWindow")]
    public static void TestDragWindow()
    {
        GetWindow<DragWindowTest>();
    }

    private Rect _boxRect = new Rect(15f, 15f, 50f, 50f);
    private bool _isDragging;
    private Vector2 _dragOffset;

    private void OnGUI()
    {
        GUI.Box(_boxRect, "");

        var current = Event.current;
        switch (current.type)
        {
            case EventType.MouseDown:
                if (_boxRect.Contains(current.mousePosition))
                {
                    _isDragging = true;
                    _dragOffset = _boxRect.position - current.mousePosition;
                }
                break;

            case EventType.MouseUp:
                _isDragging = false;

                break;
            case EventType.MouseDrag:
                if (_isDragging)
                {
                    _boxRect.position = current.mousePosition + _dragOffset;
                }
                break;
        }
    
    }
}
