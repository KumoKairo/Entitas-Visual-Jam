using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View.Drawer
{
    public class GraphWindowTopToolbar
    {
        private Color _buttonColor;
        private Rect _lastCompileButtonRect;

        public void OnGUI(EditorWindow parentWindow, float toolbarHeight)
        {
            var toolbarRect = new Rect(0f, 0f, parentWindow.position.width, toolbarHeight);
            GUIHelper.DrawQuad(toolbarRect, StyleProxy.TopToolbarColor);

            var buttonSize = new Vector2(48f, 48f);
            var compileButtonRect = new Rect(new Vector2(toolbarRect.x + toolbarRect.width - buttonSize.x, toolbarRect.y), buttonSize);
            compileButtonRect.y -= (buttonSize.y - toolbarHeight) * 0.5f;
            compileButtonRect.x += 6f;

            _lastCompileButtonRect = compileButtonRect;

            var refreshButtonRect = new Rect(compileButtonRect);
            refreshButtonRect.x -= 36;

            var color = GUI.color;

            GUI.color = _buttonColor;//StyleProxy.CompileButtonColorNormal;
            GUI.DrawTexture(compileButtonRect, StyleProxy.CompileButtonTexture);
            GUI.DrawTexture(refreshButtonRect, StyleProxy.RefreshIconTexture);

            GUI.color = color;
        }

        public void HandleEvents(Event current)
        {
            _buttonColor = StyleProxy.CompileButtonColorNormal;

            if (_lastCompileButtonRect.Contains(current.mousePosition))
            {
                _buttonColor = StyleProxy.CompileButtonColorHover;
            }
        }
    }
}
