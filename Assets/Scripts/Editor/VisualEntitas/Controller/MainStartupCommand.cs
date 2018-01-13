using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

namespace Entitas.Visual.Controller
{
    public class MainStartupCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Debug.Log("Pure MVC Startup!");
        }
    }
}
