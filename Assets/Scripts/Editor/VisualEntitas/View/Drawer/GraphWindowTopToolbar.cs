using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class GraphWindowTopToolbar
    {
        private Rect _compileButtonRect;

        public void OnGUI(EditorWindow parentWindow, float toolbarHeight)
        {
            var toolbarRect = new Rect(0f, 0f, parentWindow.position.width, toolbarHeight);
            GUIHelper.DrawQuad(toolbarRect, StyleProxy.TopToolbarColor);

            var buttonSize = new Vector2(48f, 48f);
            _compileButtonRect = new Rect(new Vector2(toolbarRect.x + toolbarRect.width - buttonSize.x, toolbarRect.y), buttonSize);
            _compileButtonRect.y -= (buttonSize.y - toolbarHeight) * 0.5f;
            _compileButtonRect.x += 6f;

            var compileButtonHotRect = new Rect(_compileButtonRect);
            compileButtonHotRect.height = toolbarHeight;

            var buttonColor = StyleProxy.CompileButtonColorNormal;
            if (compileButtonHotRect.Contains(Event.current.mousePosition))
            {
                buttonColor = StyleProxy.CompileButtonColorHover;
            }

            var refreshButtonRect = new Rect(_compileButtonRect);
            refreshButtonRect.x -= 36;

            var backgroundColor = GUI.backgroundColor;
            var contentColor = GUI.contentColor;
            GUI.backgroundColor = Color.clear;
            GUI.contentColor = buttonColor;

            var saveButtonContent = new GUIContent(StyleProxy.CompileButtonTexture, "Save");
            GUI.Box(_compileButtonRect, saveButtonContent);

            GUI.color = backgroundColor;
            GUI.contentColor = contentColor;
        }

        public void HandleEvents(Event current, out bool shouldCompile)
        {
            shouldCompile = false;

            if (current.type == EventType.MouseDown
                && _compileButtonRect.Contains(current.mousePosition))
            {
                shouldCompile = true;
                current.Use();
            }
        }
    }
}
