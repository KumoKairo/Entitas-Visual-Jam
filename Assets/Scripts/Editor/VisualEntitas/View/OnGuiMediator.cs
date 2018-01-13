using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using UnityEditor;

namespace Entitas.Visual.View
{
    public abstract class OnGuiMediator : Mediator
    {
        protected OnGuiMediator(string mediatorName, EditorWindow viewComponent) : base(mediatorName, viewComponent){}

        public override string[] ListNotificationInterests()
        {
            return new[]
            {
                VisualEntitasFacade.OnGUI
            };
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case VisualEntitasFacade.OnGUI:
                    OnGUI(AppView);
                    break;
            }
        }

        protected abstract void OnGUI(EditorWindow appView);

        protected EditorWindow AppView
        {
            get { return (EditorWindow)ViewComponent; }
        }
    }
}