using Entitas.Visual.View;
using UnityEngine;

namespace Entitas.Visual.Utils
{
    public static class GUIHelper
    {
        public static void DrawSlicedRectTopPart(Rect position, Texture2D texture, RectOffset borders, Color? color = null)
        {
            GL.PushMatrix();
            StyleProxy.EditorMaterial.SetTexture(StyleProxy.EditorMaterialTextureParameterName, texture);
            StyleProxy.EditorMaterial.SetPass(0);
            GL.Begin(GL.QUADS);

            if (color != null)
            {
                GL.Color(color.Value);
            }

            float leftUvBorder = (float) borders.left / texture.width;
            float rightUvBorder = (float) (texture.width - borders.right) / texture.width;
            float rightBorder = (float) (borders.right) / texture.width;

            float topUvBorder = (float) (texture.height - borders.bottom) / texture.height;
            float topBorder = (float) (borders.top) / texture.height;

            float bottomUvBorder = (float) borders.top / texture.height;
            float bottomBorder = (float) (texture.height - borders.top) / texture.height;

            //Left top part
            GL.TexCoord3(0f, 1f, 0f);
            GL.Vertex3(position.x, position.y, 0f);

            GL.TexCoord3(leftUvBorder, 1f, 0f);
            GL.Vertex3(position.x + position.width * leftUvBorder, 
                position.y, 0f);

            GL.TexCoord3(leftUvBorder, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width * leftUvBorder, 
                position.y + position.height * topBorder, 0f);

            GL.TexCoord3(0f, topUvBorder, 0f);
            GL.Vertex3(position.x, position.y + position.height * topBorder, 0f);

            //Middle top part
            GL.TexCoord3(leftUvBorder, 1f, 0f);
            GL.Vertex3(position.x + position.width * leftUvBorder, position.y, 0f);

            GL.TexCoord3(rightUvBorder, 1f, 0f);
            GL.Vertex3(position.x + position.width * rightUvBorder, position.y, 0f);

            GL.TexCoord3(rightUvBorder, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width * rightUvBorder,
                position.y + position.height * topBorder, 0f);

            GL.TexCoord3(leftUvBorder, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width * leftUvBorder,
                position.y + position.height * topBorder, 0f);

            //Right top part
            GL.TexCoord3(rightUvBorder, 1f, 0f);
            GL.Vertex3(position.x + position.width * rightUvBorder, position.y, 0f);

            GL.TexCoord3(1f, 1f, 0f);
            GL.Vertex3(position.x + position.width, position.y, 0f);

            GL.TexCoord3(1f, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width, 
                position.y + position.height * topBorder, 0f);

            GL.TexCoord3(rightUvBorder, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width * rightUvBorder, 
                position.y + position.height * topBorder, 0f);

            //Right middle part
            GL.TexCoord3(rightUvBorder, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width * rightUvBorder, 
                position.y + position.height * topBorder, 0f);

            GL.TexCoord3(1f, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width,
                position.y + position.height * topBorder, 0f);

            GL.TexCoord3(1f, bottomUvBorder, 0f);
            GL.Vertex3(position.x + position.width,
                position.y + position.height * bottomBorder, 0f);

            GL.TexCoord3(rightUvBorder, bottomUvBorder, 0f);
            GL.Vertex3(position.x + position.width * rightUvBorder,
                position.y + position.height * bottomBorder, 0f);

            //Middle part
            GL.TexCoord3(leftUvBorder, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width * leftUvBorder,
                position.y + position.height * topBorder, 0f);

            GL.TexCoord3(rightUvBorder, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width * rightUvBorder,
                position.y + position.height * topBorder, 0f);

            GL.TexCoord3(rightUvBorder, bottomUvBorder, 0f);
            GL.Vertex3(position.x + position.width * rightUvBorder,
                position.y + position.height * bottomBorder, 0f);

            GL.TexCoord3(leftUvBorder, bottomUvBorder, 0f);
            GL.Vertex3(position.x + position.width * leftUvBorder,
                position.y + position.height * bottomBorder, 0f);

            //Left middle part
            GL.TexCoord3(0f, topUvBorder, 0f);
            GL.Vertex3(position.x, position.y + position.height * topBorder, 0f);

            GL.TexCoord3(leftUvBorder, topUvBorder, 0f);
            GL.Vertex3(position.x + position.width * leftUvBorder,
                position.y + position.height * topBorder, 0f);

            GL.TexCoord3(leftUvBorder, bottomUvBorder, 0f);
            GL.Vertex3(position.x + position.width * leftUvBorder,
                position.y + position.height * bottomBorder, 0f);

            GL.TexCoord3(0f, bottomUvBorder, 0f);
            GL.Vertex3(position.x, position.y + position.height * bottomBorder, 0f);

            GL.End();
            GL.PopMatrix();
            StyleProxy.EditorMaterial.SetTexture(StyleProxy.EditorMaterialTextureParameterName, null);
        }

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
    }
}