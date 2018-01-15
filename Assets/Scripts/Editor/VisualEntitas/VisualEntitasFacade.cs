using Entitas.Visual.Controller;
using Entitas.Visual.Controller.Graph;
using Entitas.Visual.View;
using PureMVC.Patterns.Facade;
using UnityEditor;
using UnityEngine;

namespace Entitas.Visual
{
    public class VisualEntitasFacade : Facade
    {
        public const string OnGUI = "OnGUI";

        public const string Name = "VisualEntitasCore";
        public const string Startup = Name + "Startup";
        public const string Teardown = Name + "Teardown";

        protected override void InitializeController()
        {
            base.InitializeController();

            RegisterCommand(Startup, () => new StartupCommand());
            RegisterCommand(Teardown, () => new TeardownCommand());

            RegisterCommand(NodeAreaMediator.CreateNewComponent, () => new ComponentNodeCreateNewCommand());

            RegisterCommand(NodeAreaMediator.NodeFieldAdd, () => new NodeFieldAddCommand());
            RegisterCommand(NodeAreaMediator.NodeFieldRemove, () => new NodeFieldRemoveCommand());
            RegisterCommand(NodeAreaMediator.NodeFieldRename, () => new NodeFieldRenameCommand());
            RegisterCommand(NodeAreaMediator.NodeFieldChangeType, () => new NodeChangeFieldTypeCommand());
            RegisterCommand(NodeAreaMediator.NodeRename, () => new NodeRenameCommand());

            RegisterCommand(NodeAreaMediator.NodePositionUpdate, () => new NodeUpdatePositionCommand());
            RegisterCommand(NodeAreaMediator.NodeRemove, () => new NodeRemoveCommand());
            RegisterCommand(NodeAreaMediator.NodeCollapse, () => new NodeCollapseCommand()); 
        }

        public void Start(EditorWindow mainWindow)
        {
            Stop();
            SendNotification(Startup, mainWindow);

            GUI.FocusControl("");
        }

        public void Stop()
        {
            SendNotification(Teardown);
        }

        public override void SendNotification(string notificationName, object body = null, string type = null)
        {
            if (notificationName != VisualEntitasFacade.OnGUI)
            {
                Debug.Log("Sent " + notificationName);
            }

            base.SendNotification(notificationName, body, type);
        }

        public VisualEntitasFacade() : base(Name)
        {
        }
    }
}
