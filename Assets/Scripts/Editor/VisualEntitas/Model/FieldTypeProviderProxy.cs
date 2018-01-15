using Entitas.Visual.Model.VO;
using PureMVC.Patterns.Proxy;

namespace Entitas.Visual.Model
{
    public class FieldTypeProviderProxy : Proxy
    {
        public const string Name = "FieldTypeProviderProxy";

        public FieldTypeProviderProxy() : base(Name, new FieldTypeProvider())
        {
            FieldTypeProviderData.InitializeWithSimpleTypes();
        }

        public FieldTypeProvider FieldTypeProviderData
        {
            get { return (FieldTypeProvider) Data; }
        }
    }
}
