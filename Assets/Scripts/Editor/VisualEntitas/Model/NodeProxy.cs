using Entitas.Visual.Model.VO;
using PureMVC.Patterns.Proxy;

namespace Entitas.Visual.Model
{
    public class NodeProxy : Proxy
    {
        public const string Name = "NodeProxy";

        public NodeProxy() : base(Name, new Node())
        {
        }

        public Node Vo()
        {
            return Data as Node;
        }
    }
}