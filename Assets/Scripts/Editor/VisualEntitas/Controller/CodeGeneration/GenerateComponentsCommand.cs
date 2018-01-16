using System.IO;
using System.Text;
using Entitas.Visual.Model;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using UnityEngine;

namespace Entitas.Visual.Controller.CodeGeneration
{
    public class GenerateComponentsCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            var graphProxy = (GraphProxy) Facade.RetrieveProxy(GraphProxy.Name);
            var data = graphProxy.GraphData;

            foreach (var dataNode in data.Nodes)
            {
                var targetFolder = Application.dataPath + SaveAndCompileMacroCommand.GenerateToDirectory;
                var fileName = targetFolder + dataNode.Name + ".cs";

                var componentStringBuilder = new StringBuilder(ComponentTemplate.ClassTemplate);
                componentStringBuilder.Replace("#COMPONENT_NAME#", dataNode.Name);
                
                var fieldsStringBuilder = new StringBuilder();
                foreach (var dataNodeField in dataNode.Fields)
                {
                    fieldsStringBuilder.Append(
                        ComponentTemplate.FieldTemplate
                            .Replace("#FIELD_TYPE#", dataNodeField.Type)
                            .Replace("#FIELD_NAME#", dataNodeField.Name)
                        );
                }

                componentStringBuilder.Replace("#FIELDS#", fieldsStringBuilder.ToString());
                
                using (var file = File.Open(fileName, FileMode.OpenOrCreate))
                {
                    using (var streamWriter = new StreamWriter(file))
                    {
                        streamWriter.Write(componentStringBuilder.ToString());
                    }
                }
            }
        }
    }
}
