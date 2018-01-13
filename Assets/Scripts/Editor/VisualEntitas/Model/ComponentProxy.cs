using Entitas.Visual.Model.VO;
using PureMVC.Patterns.Proxy;

namespace Entitas.Visual.Model
{
    public class ComponentProxy : Proxy
    {
        public const string Name = "ComponentProxy";

        public ComponentProxy() : base(Name, new ComponentVO())
        {
        }

        public ComponentVO Vo()
        {
            return Data as ComponentVO;
        }
    }
}