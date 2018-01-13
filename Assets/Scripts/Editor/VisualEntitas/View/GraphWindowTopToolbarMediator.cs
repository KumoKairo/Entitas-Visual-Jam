using Entitas.Visual.View.Drawer;
using PureMVC.Patterns.Mediator;
using UnityEditor;

namespace Entitas.Visual.View
{
    public class GraphWindowTopToolbarMediator : Mediator
    {
        public const string Name = "GraphWindowTopToolbarMediator";

        private GraphWindow _parentView
        {
            get
            {
                return ViewComponent as GraphWindow;
            }
        }

        private GraphWindowTopToolbar _toolbar;

        public GraphWindowTopToolbarMediator(GraphWindow viewComponent) : base(Name, viewComponent)
        {
        }

        public override void OnRegister()
        {
            _toolbar = new GraphWindowTopToolbar();
            _toolbar.RegisterOnGUITo(_parentView, true);
        }

        public override void OnRemove()
        {
            _toolbar.RegisterOnGUITo(_parentView, false);
            _toolbar = null;
        }
    }
}
