using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class GraphWindowTopToolbar
    {
        public void OnGUI(EditorWindow parentWindow)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            GL.PushMatrix();
            GL.Begin(GL.QUADS);
            GL.Color(StyleProxy.TopToolbarColor);

            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(parentWindow.position.width, 0f, 0f);
            GL.Vertex3(parentWindow.position.width, 16f, 0f);
            GL.Vertex3(0f, 16f, 0f);

            GL.End();
            GL.PopMatrix();
        }
    }
}
