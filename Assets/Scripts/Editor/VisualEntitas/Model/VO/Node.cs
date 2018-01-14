using System.Collections.Generic;
using UnityEngine;

namespace Entitas.Visual.Model.VO
{
    using System;

    [Serializable]
    public class Node
    {
        public bool IsCollapsed;
        public Rect Position;

        public string Name;
        public List<string> Fields = new List<string>();
    }
}