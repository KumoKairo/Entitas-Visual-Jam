using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class GraphWindowTopToolbar
    {
        public void OnGUI(EditorWindow parentWindow, float toolbarHeight)
        {
            var toolbarRect = new Rect(0f, 0f, parentWindow.position.width, toolbarHeight);
            GUIHelper.DrawQuad(toolbarRect, StyleProxy.TopToolbarColor);

            var buttonSize = new Vector2(48f, 48f);
            var compileButtonRect = new Rect(new Vector2(toolbarRect.x + toolbarRect.width - buttonSize.x, toolbarRect.y), buttonSize);
            compileButtonRect.y -= (buttonSize.y - toolbarHeight) * 0.5f;
            compileButtonRect.x += 6f;

            var compileButtonHotRect = new Rect(compileButtonRect);
            compileButtonHotRect.height = toolbarHeight;

            var buttonColor = StyleProxy.CompileButtonColorNormal;
            if (compileButtonHotRect.Contains(Event.current.mousePosition))
            {
                buttonColor = StyleProxy.CompileButtonColorHover;
            }

            var refreshButtonRect = new Rect(compileButtonRect);
            refreshButtonRect.x -= 36;

            var color = GUI.color;

            GUI.color = buttonColor;
            GUI.DrawTexture(compileButtonRect, StyleProxy.CompileButtonTexture);

            GUI.color = color;
        }

        public void HandleEvents(Event current)
        {
            
        }
    }
}
