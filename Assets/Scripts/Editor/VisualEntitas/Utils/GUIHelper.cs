using Entitas.Visual.View;
using UnityEngine;

namespace Entitas.Visual.Utils
{
    public static class GUIHelper
    {
        public static void DrawQuad(Rect position, Color color)
        {
            GL.PushMatrix();
            GL.Begin(GL.QUADS);
            StyleProxy.EditorMaterial.SetTexture(StyleProxy.EditorMaterialTextureParameterName, null);
            StyleProxy.EditorMaterial.SetPass(0);
            GL.Color(color);
            GL.Vertex3(position.x, position.y, 0f);
            GL.Vertex3(position.x + position.width, position.y, 0f);
            GL.Vertex3(position.x + position.width, position.y + position.height, 0f);
            GL.Vertex3(position.x, position.y + position.height, 0f);
            GL.End();
            GL.PopMatrix();
        }

        public static GUIContent GetOrCreateOrUpdateGUIContentFor(string text, ref GUIContent content)
        {
            if (content == null || content.text != text)
            {
                content = new GUIContent(text);
            }

            return content;
        }
    }
}