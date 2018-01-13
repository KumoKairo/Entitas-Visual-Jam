using Entitas.Visual.View.Drawer;
using UnityEditor;

namespace Entitas.Visual.View
{
    public class GraphWindowTopToolbarMediator : OnGuiMediator
    {
        public const string Name = "GraphWindowTopToolbarMediator";

        private GraphWindowTopToolbar _toolbar;

        public GraphWindowTopToolbarMediator(EditorWindow viewComponent) : base(Name, viewComponent)
        {
        }

        public override void OnRegister()
        {
            _toolbar = new GraphWindowTopToolbar();
        }

        public override void OnRemove()
        {
            _toolbar = null;
        }

        protected override void OnGUI(EditorWindow appView)
        {
            _toolbar.OnGUI(appView);
        }
    }
}
