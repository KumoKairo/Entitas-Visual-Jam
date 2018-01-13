using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View
{
    public static class StyleProxy
    {
        public static Color DarkBackgroundColor = new Color32(24, 34, 44, 255);
        public static Color DarkLineColorMinor = new Color(0.0f, 0.0f, 0.0f, 0.18f);
        public static Color DarkLineColorMajor = new Color(0.0f, 0.0f, 0.0f, 0.28f);
        public static Color TopToolbarColor = new Color32(44, 62, 80, 255);

        public static Color OrangeDebugColor = new Color32(231, 76, 60, 100);

        private static object _nodeBackgroundStyle;
        public static GUIStyle NodeBackgroundStyle
        {
            get
            {
                _nodeBackgroundStyle = _nodeBackgroundStyle ?? new GUIStyle
                {
                    normal =
                    {
                        background = (Texture2D) EditorGUIUtility.Load("Textures/NodeBackground.png"),
                        textColor = new Color(0.82f, 0.82f, 0.82f),
                    },
                    stretchHeight = true,
                    stretchWidth = true,
                    border = new RectOffset(44, 50, 20, 34),
                };

                return (GUIStyle) _nodeBackgroundStyle;
            }
        }

        private static object _editorMaterial;
        public static Material EditorMaterial
        {
            get
            {
                _editorMaterial = _editorMaterial ?? EditorGUIUtility.Load("UnlitColorMaterial.mat");
                return (Material) _editorMaterial;
            }
        }
    }
}
