using PureMVC.Patterns.Facade;
using UnityEditor;

namespace Entitas.Visual
{
    public class VisualEntitasGraphWindowOpener : EditorWindow
    {
        private VisualEntitasFacade _facade;

        [MenuItem("Window/Visual Entitas")]
        public static void RunVisualEntitasGraph()
        {
            GetWindow<VisualEntitasGraphWindowOpener>();
        }

        public void Update()
        {
        }

        public void OnGUI()
        {
            if (_facade == null)
            {
                _facade = (VisualEntitasFacade) Facade.GetInstance(VisualEntitasFacade.Name, () => new VisualEntitasFacade());
                _facade.Start(this);
            }

            _facade.SendNotification(VisualEntitasFacade.OnGUI);
        }

        public void Awake()
        {
        }

        public void OnDestroy()
        {
        }

    }
}