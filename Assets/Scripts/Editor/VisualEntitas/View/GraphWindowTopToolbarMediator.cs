using Entitas.Visual.View.Drawer;
using UnityEditor;
using UnityEngine;

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
            float toolbarHeight = 32f;
            _toolbar.OnGUI(appView, toolbarHeight);
            _toolbar.HandleEvents(Event.current);
        }
    }
}
