using System.Collections.Generic;
using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Entitas.Visual.View.Drawer
{
    public class GraphWindow : EditorWindow
    {
        private List<IOnGUIObserver> _children = new List<IOnGUIObserver>();

        public void InitializeContent()
        {
            titleContent = new GUIContent("Graph");
        }

        public void AddOnGUIObserver(IOnGUIObserver observer)
        {
            _children.AddOnce(observer);
        }

        public void RemoveOnGUIObserver(IOnGUIObserver observer)
        {
            _children.Remove(observer);
        }

        private void OnGUI()
        {
            DrawBackground();
            DrawGrid();

            foreach (var onGuiObserver in _children)
            {
                onGuiObserver.OnGUI(this);
            }
        }

        private void DrawBackground()
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

        private void DrawGrid()
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            Profiler.BeginSample("Draw grid");
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            this.DrawGridLines(12f, StyleProxy.DarkLineColorMinor);
            this.DrawGridLines(120f, StyleProxy.DarkLineColorMajor);
            GL.End();
            GL.PopMatrix();
            Profiler.EndSample();
        }

        private void DrawGridLines(float gridSize, Color gridColor)
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
            //GL.Color(gridColor);
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
