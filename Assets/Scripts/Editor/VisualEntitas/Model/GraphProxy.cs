using System;
using System.IO;
using Atrox;
using Entitas.Visual.Model.VO;
using Entitas.Visual.Utils;
using PureMVC.Patterns.Proxy;
using UnityEngine;

namespace Entitas.Visual.Model
{
    public class GraphProxy : Proxy
    {
        public const string NodeAdded = "GraphNodeAdded";
        public const string NodePositionUpdated = "GraphNodePositionUpdated";
        public const string NodeRemoved = "GraphNodeRemoved";
        public const string NodeCollapsed = "GraphNodeCollapsed";
        public const string NodeFieldAdded = "GraphNodeFieldAdded";
        public const string NodeFieldRemoved = "GraphNodeFieldRemoved";
        public const string NodeFieldRenamed = "GraphNodeFieldRenamed";
        public const string NodeFieldTypeChanged = "GraphNodeFieldTypeChanged";
        public const string NodeRenamed = "GraphNodeRenamed";

        public const string GraphPath = "/Graph.json";

        public const string Name = "GraphProxy";
        private string _totalGraphPath;

        public GraphProxy() : base(Name)
        {
            Graph graph;

            _totalGraphPath = Application.dataPath + GraphPath;

            if (File.Exists(_totalGraphPath))
            {
                graph = JsonUtility.FromJson<Graph>(File.ReadAllText(_totalGraphPath));
            }
            else
            {
                graph = new Graph();
                SaveGraph(graph);
            }

            Data = graph;
        }

        public void AddNewNode(Vector2 mousePosition)
        {
            var newNode = new Node
            {
                Position = new Rect(mousePosition, new Vector2(240f, 80f)),
                Name = Haikunator.Random()
            };
            GraphData.Nodes.Add(newNode);
            SaveGraph(GraphData);

            SendNotification(NodeAdded, newNode);
        }

        public void RemoveNode(Node node)
        {
            var isRemoved = GraphData.Nodes.Remove(node);

            if (isRemoved)
            {
                SaveGraph(GraphData);
                SendNotification(NodeRemoved, node);
            }
        }

        public void RemoveNodeField(Node node, Field field)
        {
            var fieldRemoved = node.Fields.Remove(field);
            if (fieldRemoved)
            {
                SaveGraph(GraphData);
                SendNotification(NodeFieldRemoved, new Tuple<Node, Field>(node, field));
            }
        }

        public void UpdateNodePosition(Node node, Vector2 newPosition)
        {
            node.Position.position = newPosition;
            SaveGraph(GraphData);
            SendNotification(NodePositionUpdated, node);
        }

        public void CollapseNode(Node node)
        {
            node.IsCollapsed = !node.IsCollapsed;
            SaveGraph(GraphData);
            SendNotification(NodeCollapsed, node);
        }

        public void AddFieldToNode(Node node, Type type)
        {
            var newField = new Field(Haikunator.Random(), type.FullName);
            node.Fields.Add(newField);
            SaveGraph(GraphData);
            SendNotification(NodeFieldAdded, new Tuple<Node, Field>(node, newField));
        }

        public void RenameNode(Node node, string name)
        {
            node.Name = name;
            SaveGraph(GraphData);
            SendNotification(NodeRenamed, node);
        }

        public void RenameNodeField(Node node, Field field)
        {
            SaveGraph(GraphData);
            SendNotification(NodeFieldRenamed, new Tuple<Node, Field>(node, field));
        }

        public Graph GraphData
        {
            get { return (Graph) Data; }
        }

        private void SaveGraph(Graph graph)
        {
            var serializedString = JsonUtility.ToJson(graph);
            File.WriteAllText(_totalGraphPath, serializedString);
        }

        public void ChangeFieldType(Tuple<Node, Field, Type> payload)
        {
            var field = payload.Second;
            var type = payload.Third;

            field.Type = type.FullName;
            SaveGraph(GraphData);
            SendNotification(NodeFieldTypeChanged);
        }
    }
}