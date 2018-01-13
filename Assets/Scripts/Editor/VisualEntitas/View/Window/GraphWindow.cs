using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.Profiling;

namespace Entitas.Visual.View.Window
{
    public class GraphWindow : EditorWindow
    {
        private Texture _graphWindowBackgroundTexture;

        public void InitializeContent()
        {
            titleContent = new GUIContent("Graph");
        }

        private void OnGUI()
        {
            if (_graphWindowBackgroundTexture == null)
            {
                _graphWindowBackgroundTexture = (Texture) EditorGUIUtility.Load("Textures/GraphWindowBackground.png");
            }

            GUI.DrawTexture(new Rect(0f, 0f, position.width, position.height), _graphWindowBackgroundTexture);
            DrawGrid();
        }

        private void DrawGrid()
        {
            if (Event.current.type != EventType.Repaint)
                return;
            Profiler.BeginSample("Draw grid");
            //HandleUtility.ApplyWireMaterial();
            GL.PushMatrix();
            GL.Begin(1);
            this.DrawGridLines(12f, new Color(0.0f, 0.0f, 0.0f, 0.18f));
            this.DrawGridLines(120f, new Color(0.0f, 0.0f, 0.0f, 0.28f));
            GL.End();
            GL.PopMatrix();
            Profiler.EndSample();
        }

        private void DrawGridLines(float gridSize, Color gridColor)
        {
            GL.Color(gridColor);
            float x = 0f;
            while (x < position.width)
            {
                this.DrawLine(
                    new Vector2(x, 0f), 
                    new Vector2(x, position.height));

                x += gridSize;
            }
            //GL.Color(gridColor);
            float y = 0f;
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
            GL.Vertex((Vector3)p1);
            GL.Vertex((Vector3)p2);
        }
    }
}
