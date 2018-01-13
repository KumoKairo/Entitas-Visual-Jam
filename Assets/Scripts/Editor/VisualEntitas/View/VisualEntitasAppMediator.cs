using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual.View
{
    public class VisualEntitasAppMediator : Mediator
    {
        public const string Name = "VisualEntitasMediator";

        public VisualEntitasAppMediator(EditorWindow window) : base(Name, window)
        {
            window.titleContent = new GUIContent("Visual Entitas");
        }

        public override void OnRegister()
        {
            Facade.RegisterMediator(new GraphWindowMediator(ViewComponent as EditorWindow));
        }

        public override void OnRemove()
        {
            Facade.RemoveMediator(GraphWindowMediator.Name);
        }
    }
}