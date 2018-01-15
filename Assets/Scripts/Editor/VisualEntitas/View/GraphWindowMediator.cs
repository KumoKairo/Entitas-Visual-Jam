using Entitas.Visual.View.Drawer;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View
{
    public class GraphWindowMediator : OnGuiMediator
    {
        public const float CellWidth = 16f;
        public static Vector2 SnapCell = new Vector2(CellWidth, CellWidth);

        public const string Name = "GraphWindow";

        private GraphWindow _window;

        public static Vector2 SnapDragPositionToGrid(Vector2 position)
        {
            var gridSnap = SnapCell;

            var snapIncrements = new Vector2(
                Mathf.Round(position.x / gridSnap.x),
                Mathf.Round(position.y / gridSnap.y));

            return new Vector2(snapIncrements.x * gridSnap.x, snapIncrements.y * gridSnap.y);
        }

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