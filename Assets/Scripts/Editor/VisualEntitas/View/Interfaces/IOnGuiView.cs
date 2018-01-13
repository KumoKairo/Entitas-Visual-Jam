using UnityEditor;

namespace Entitas.Visual.View
{
    public interface IOnGuiView
    {
        void OnGUI(EditorWindow appView);
    }
}