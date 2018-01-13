using PureMVC.Patterns.Mediator;
using UnityEngine;

namespace Entitas.Visual.View
{
    public class VisualEntitasAppMediator : Mediator
    {
        public const string Name = "VisualEntitasMediator";
        public VisualEntitasAppMediator() : base(Name, null)
        {
        }

        public override void OnRegister()
        {
            Debug.Log("OnRegister " + Name);
            Facade.RegisterMediator(new GraphWindowMediator());
        }

        public override void OnRemove()
        {
            Debug.Log("OnRemove " + Name);
            Facade.RemoveMediator(GraphWindowMediator.Name);
        }
    }
}