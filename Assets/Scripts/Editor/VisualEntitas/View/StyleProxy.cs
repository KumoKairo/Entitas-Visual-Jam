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

        public static Color NodeTitleBackdropColor = new Color32(137, 204, 249, 200);
        public static Color NodeBackgroundColor = new Color32(38, 55, 72, 200);

        private static object _chevronUpTexture;
        public static Texture2D ChevronUpTexture
        {
            get { return (Texture2D)(_chevronUpTexture = _chevronUpTexture ?? EditorGUIUtility.Load("Textures/Icons/appbar.chevron.up.png")); }
        }

        private static object _chevronDownTexture;
        public static Texture2D ChevronDownTexture
        {
            get { return (Texture2D)(_chevronDownTexture = _chevronDownTexture ?? EditorGUIUtility.Load("Textures/Icons/appbar.chevron.down.png")); }
        }

        public static Color ChevronUpBackdropColorNormal = new Color32(52, 73, 94, 200);
        public static Color ChevronUpBackdropColorHover = new Color32(39, 174, 96, 255);
        public static Color ChevronUpColor = new Color32(22, 160, 133, 200);
        public static Color ChevronDownColor = new Color32(22, 160, 133, 200);

        private static Color NodeTitleColor = new Color32(44, 50, 50, 255);
        private static object _nodeTitleTextStyle;
        public static GUIStyle NodeTitleTextStyle
        {
            get
            {
                _nodeTitleTextStyle = _nodeTitleTextStyle ?? new GUIStyle
                {
                    normal =
                    {
                        background = null,
                        textColor = NodeTitleColor
                    },
                    font = (Font)EditorGUIUtility.Load("Fonts/Montserrat-Bold.ttf"),
                    fontSize = 16
                };

                return (GUIStyle)_nodeTitleTextStyle;
            }
        }

        private static object _nodeSubtitleTextStyle;
        public static GUIStyle NodeSubtitleTextStyle
        {
            get
            {
                _nodeSubtitleTextStyle = _nodeSubtitleTextStyle ?? new GUIStyle
                {
                    normal =
                    {
                        background = null,
                        textColor = NodeTitleColor
                    },
                    font = (Font)EditorGUIUtility.Load("Fonts/Montserrat-Bold.ttf"),
                    fontSize = 10
                };

                return (GUIStyle)_nodeSubtitleTextStyle;
            }
        }

        public static int EditorMaterialTextureParameterName = Shader.PropertyToID("_MainTex");
        private static object _editorMaterial;
        public static Material EditorMaterial
        {
            get { return (Material)(_editorMaterial = _editorMaterial ?? EditorGUIUtility.Load("UnlitTextureMaterial.mat")); }
        }
    }
}
