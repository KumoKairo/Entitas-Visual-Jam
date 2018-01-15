using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

namespace Entitas.Visual.Controller.CodeGeneration
{
    public class GenerateComponentsCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Debug.Log("CodeGen");
        }
    }
}
