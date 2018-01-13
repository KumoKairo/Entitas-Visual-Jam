using Entitas.Visual.Controller;
using PureMVC.Patterns.Facade;
using UnityEngine;

namespace Entitas.Visual
{
    public class VisualEntitasFacade : Facade
    {
        public const string NAME = "VisualEntitasCore";
        public const string STARTUP = NAME + "Startup";
        public const string TEARDOWN = NAME + "Teardown";

        protected override void InitializeController()
        {
            base.InitializeController();
            RegisterCommand(STARTUP, () => new StartupCommand());
            RegisterCommand(TEARDOWN, () => new TeardownCommand());
        }

        public void Startup()
        {
            SendNotification(TEARDOWN);
            SendNotification(STARTUP);
        }

        public override void SendNotification(string notificationName, object body = null, string type = null)
        {
            Debug.Log("Sent " + notificationName);
            base.SendNotification(notificationName, body, type);
        }

        public VisualEntitasFacade(string key) : base(key)
        {
        }
    }
}
