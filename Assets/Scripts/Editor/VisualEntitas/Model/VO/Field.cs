using System;

namespace Entitas.Visual.Model.VO
{
    [Serializable]
    public class Field
    {
        public string Name;
        public string Type;

        public Field(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
