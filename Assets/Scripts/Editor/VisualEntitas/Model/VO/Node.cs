using UnityEngine;

namespace Entitas.Visual.Model.VO
{
    using System;

    [Serializable]
    public class Node
    {
        public Rect Position;

        public string Name;
        public string[] Fields;
    }
}