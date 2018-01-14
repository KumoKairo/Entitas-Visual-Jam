using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Entitas.Visual.View.Drawer
{
    public class GraphWindow
    {
        public void OnGUI(EditorWindow window)
        {
            DrawBackground(window.position);
            DrawGrid(window.position);
        }

        private void DrawBackground(Rect position)
        {
            GL.PushMatrix();
            StyleProxy.EditorMaterial.SetPass(0);
            GL.Begin(GL.QUADS);
            GL.Color(StyleProxy.DarkBackgroundColor);

            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(position.width, 0f, 0f);
            GL.Vertex3(position.width, position.height, 0f);
            GL.Vertex3(0f, position.height, 0f);

            GL.End();
            GL.PopMatrix();
        }

        private void DrawGrid(Rect position)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            Profiler.BeginSample("Draw grid");
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            this.DrawGridLines(16f, StyleProxy.DarkLineColorMinor, position);
            this.DrawGridLines(80f, StyleProxy.DarkLineColorMajor, position);
            GL.End();
            GL.PopMatrix();
            Profiler.EndSample();
        }

        private void DrawGridLines(float gridSize, Color gridColor, Rect position)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            GL.Color(gridColor);
            float x = 0f;
            while (x < position.width)
            {
                this.DrawLine(
                    new Vector2(x, 16f),
                    new Vector2(x, position.height));

                x += gridSize;
            }

            float y = 16f;
            while (y < position.height)
            {
                this.DrawLine(
                    new Vector2(0f, y),
                    new Vector2(position.width, y));

                y += gridSize;
            }
        }

        private void DrawLine(Vector2 p1, Vector2 p2)
        {
            GL.Vertex(p1);
            GL.Vertex(p2);
        }
    }
}
