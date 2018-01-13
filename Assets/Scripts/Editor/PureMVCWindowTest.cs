using Entitas.Visual;
using PureMVC.Patterns.Facade;
using UnityEditor;

public class PureMVCWindowTest : EditorWindow
{
    [MenuItem("Window/Visual Entitas")]
    public static void RunPureMVCTest()
    {
        const string mainFacadeName = "Visual Entitas Core";
        var facade = (VisualEntitasFacade) Facade.GetInstance(mainFacadeName, () => new VisualEntitasFacade(mainFacadeName));
        facade.Startup();
    }
}
