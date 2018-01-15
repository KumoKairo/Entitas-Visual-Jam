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

        public static Color NodeTitleBackdropColor = new Color32(91, 222, 147, 255);
        public static Color NodeBackgroundColor = new Color32(38, 55, 72, 170);

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

        public static Color BoldTransparentBlackColor = new Color32(0, 0, 0, 150);
        public static Color SemiTransparentBlackColor = new Color32(0, 0, 0, 55);
        public static Color TransparentBlackColor = new Color32(0, 0, 0, 35);
        public static Color ChevronUpBackdropColorNormal = new Color32(52, 73, 94, 200);
        public static Color ChevronUpBackdropColorHover = new Color32(52, 73, 94, 150);
        public static Color ChevronUpColor = new Color32(22, 160, 133, 200);
        public static Color ChevronDownColor = new Color32(22, 160, 133, 200);

        public static Color NodeTitleRenamingBackdropColor = new Color32(241, 196, 15, 155);
        public static Color NodeTitleColor = new Color32(19, 40, 20, 255);
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
                    focused =
                    {
                        background = null,
                    },
                    alignment = TextAnchor.MiddleCenter,
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
                    alignment = TextAnchor.MiddleCenter,
                    font = (Font)EditorGUIUtility.Load("Fonts/Montserrat-Medium.ttf"),
                    fontSize = 10
                };

                return (GUIStyle)_nodeSubtitleTextStyle;
            }
        }

        public static Color NodeFieldNameTextColorNormal = new Color32(163, 202, 229, 255);
        private static object _nodeFieldNameStyleNormal;
        public static GUIStyle NodeFieldNameStyleNormal
        {
            get
            {
                _nodeFieldNameStyleNormal = _nodeFieldNameStyleNormal ?? new GUIStyle
                {
                    normal =
                    {
                        background = null,
                        textColor = NodeFieldNameTextColorNormal
                    },
                    alignment = TextAnchor.MiddleLeft,
                    font = (Font)EditorGUIUtility.Load("Fonts/Montserrat-SemiBold.ttf"),
                    fontSize = 14
                };

                return (GUIStyle)_nodeFieldNameStyleNormal;
            }
        }

        public static Color NodeFieldNameTextColorHover = new Color32(163, 202, 229, 180);
        private static object _nodeFieldNameStyleHover;
        public static GUIStyle NodeFieldNameStyleHover
        {
            get
            {
                _nodeFieldNameStyleHover = _nodeFieldNameStyleHover ?? new GUIStyle
                {
                    normal =
                    {
                        background = null,
                        textColor = NodeFieldNameTextColorHover
                    },
                    alignment = TextAnchor.MiddleLeft,
                    font = (Font)EditorGUIUtility.Load("Fonts/Montserrat-SemiBold.ttf"),
                    fontSize = 14
                };

                return (GUIStyle)_nodeFieldNameStyleHover;
            }
        }

        public static Color NodeFieldTypeNameColorNormal = new Color32(190, 133, 213, 255);
        private static object _nodeFieldTypeStyleNormal;
        public static GUIStyle NodeFieldTypeStyleNormal
        {
            get
            {
                _nodeFieldTypeStyleNormal = _nodeFieldTypeStyleNormal ?? new GUIStyle
                {
                    normal =
                    {
                        background = null,
                        textColor = NodeFieldTypeNameColorNormal
                    },
                    alignment = TextAnchor.MiddleRight,
                    font = (Font)EditorGUIUtility.Load("Fonts/Montserrat-Medium.ttf"),
                    fontSize = 14
                };

                return (GUIStyle)_nodeFieldTypeStyleNormal;
            }
        }

        public static Color NodeFieldTypeNameColorHover = new Color32(190, 133, 213, 180);
        private static object _nodeFieldTypeStyleHover;
        public static GUIStyle NodeFieldTypeStyleHover
        {
            get
            {
                _nodeFieldTypeStyleHover = _nodeFieldTypeStyleHover ?? new GUIStyle
                {
                    normal =
                    {
                        background = null,
                        textColor = NodeFieldTypeNameColorHover
                    },
                    alignment = TextAnchor.MiddleRight,
                    font = (Font)EditorGUIUtility.Load("Fonts/Montserrat-Medium.ttf"),
                    fontSize = 14
                };

                return (GUIStyle)_nodeFieldTypeStyleHover;
            }
        }

        public static Color MinusIconColor = new Color32(192, 57, 43, 255);

        public static object _minusIconTexture;
        public static Texture2D MinusIconTexture
        {
            get { return (Texture2D)(_minusIconTexture = _minusIconTexture ?? EditorGUIUtility.Load("Textures/Icons/appbar.minus.png")); }
        }

        public static object _plusIconTexture;
        public static Texture2D PlusIconTexture
        {
            get { return (Texture2D)(_plusIconTexture = _plusIconTexture ?? EditorGUIUtility.Load("Textures/Icons/appbar.add.png")); }
        }

        public static Color CompileButtonColorNormal = new Color32(233, 148, 72, 255);
        public static Color CompileButtonColorHover = new Color32(233, 148, 72, 155);
        public static Color CompileButtonColorPressed = new Color32(211, 84, 0, 15);
        public static object _compileButtonTexture;
        public static Texture2D CompileButtonTexture
        {
            get { return (Texture2D)(_compileButtonTexture = _compileButtonTexture ?? EditorGUIUtility.Load("Textures/Icons/appbar.save.png")); }
        }

        public static object _componentIconTexture;
        public static Texture2D ComponentIconTexture
        {
            get { return (Texture2D)(_componentIconTexture = _componentIconTexture ?? EditorGUIUtility.Load("Textures/Icons/appbar.resource.png")); }
        }

        public static object _refreshIconTexture;
        public static Texture2D RefreshIconTexture
        {
            get { return (Texture2D)(_refreshIconTexture = _refreshIconTexture ?? EditorGUIUtility.Load("Textures/Icons/appbar.refresh.png")); }
        }

        
        public static int EditorMaterialTextureParameterName = Shader.PropertyToID("_MainTex");
        private static object _editorMaterial;
        public static Material EditorMaterial
        {
            get { return (Material)(_editorMaterial = _editorMaterial ?? EditorGUIUtility.Load("UnlitTextureMaterial.mat")); }
        }
    }
}
