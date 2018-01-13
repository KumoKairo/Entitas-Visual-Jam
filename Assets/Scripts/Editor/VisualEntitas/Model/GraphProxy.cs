using Entitas.Visual.Model.VO;
using PureMVC.Patterns.Proxy;

namespace Entitas.Visual.Model
{
    public class GraphProxy : Proxy
    {
        public const string Name = "GraphProxy";

        public GraphProxy() : base(Name, new Graph())
        {
        }
    }
}