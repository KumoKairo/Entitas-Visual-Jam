using System.Collections.Generic;
using UnityEngine;

namespace Entitas.Visual.Model.VO
{
    using System;

    [Serializable]
    public class Node
    {
        public const float DefaultWidth = 200f;
        public const float DefaultHeight = 60;

        public bool IsCollapsed;
        public Rect Position;

        public string Name = "NewComponent";
        public List<Field> Fields = new List<Field>();
    }
}