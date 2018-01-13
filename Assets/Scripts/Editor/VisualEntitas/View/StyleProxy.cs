using Entitas.Visual.Utils;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View
{
    public static class StyleProxy
    {
        public static Color DarkBackgroundColor = new Color32(44, 62, 80, 255);
        public static Color DarkLineColorMinor = new Color(0.0f, 0.0f, 0.0f, 0.18f);
        public static Color DarkLineColorMajor = new Color(0.0f, 0.0f, 0.0f, 0.28f);
        public static Color TopToolbarColor = new Color32(192, 57, 43, 255);

        public static Color OrangeDebugColor = new Color32(231, 76, 60, 100);

        private static Material _editorMaterial;
        public static Material EditorMaterial
        {
            get
            {
                if (_editorMaterial == null)
                {
                    _editorMaterial = (Material)
                        EditorGUIUtility.Load("UnlitColorMaterial.mat");
                }

                return _editorMaterial;
            }
        }
    }
}
