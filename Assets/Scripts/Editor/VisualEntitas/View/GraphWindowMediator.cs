using Entitas.Visual.View.Window;
using PureMVC.Patterns.Mediator;
using UnityEditor;

namespace Entitas.Visual.View
{
    public class GraphWindowMediator : Mediator
    {
        public const string Name = "GraphWindow";

        public GraphWindowMediator() : base(Name, null)
        {
        }

        public override void OnRegister()
        {
            var window = EditorWindow.GetWindow<GraphWindow>();
            window.InitializeContent();
        }
    }
}