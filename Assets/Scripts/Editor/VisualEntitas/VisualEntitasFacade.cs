using Entitas.Visual.Controller;
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using PureMVC.Utils;
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
        }

        public void Start(EditorWindow mainWindow)
        {
            Stop();
            SendNotification(Startup, mainWindow);
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
