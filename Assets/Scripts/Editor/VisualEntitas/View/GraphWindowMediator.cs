using Entitas.Visual.View.Drawer;
using PureMVC.Patterns.Mediator;
using UnityEditor;

namespace Entitas.Visual.View
{
    public class GraphWindowMediator : Mediator
    {
        public const string Name = "GraphWindow";

        private GraphWindow _window;

        public GraphWindowMediator() : base(Name, null)
        {
        }

        public override void OnRegister()
        {
            var window = EditorWindow.GetWindow<GraphWindow>();
            window.InitializeContent();

            Facade.RegisterMediator(new GraphWindowTopToolbarMediator(window));
            Facade.RegisterMediator(new NodeAreaMediator(window));
        }
    }
}