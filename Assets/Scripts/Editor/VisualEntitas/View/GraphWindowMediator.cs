using Entitas.Visual.View.Drawer;
using UnityEditor;

namespace Entitas.Visual.View
{
    public class GraphWindowMediator : OnGuiMediator
    {
        public const string Name = "GraphWindow";

        private GraphWindow _window;

        public GraphWindowMediator(EditorWindow window) : base(Name, window)
        {
        }

        public override void OnRegister()
        {
            _window = new GraphWindow();

            Facade.RegisterMediator(new GraphWindowTopToolbarMediator(AppView));
            Facade.RegisterMediator(new NodeAreaMediator(AppView));
        }

        protected override void OnGUI(EditorWindow appView)
        {
            _window.OnGUI(appView);
        }
    }
}