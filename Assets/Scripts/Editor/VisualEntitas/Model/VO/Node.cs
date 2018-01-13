using UnityEngine;

namespace Entitas.Visual.Model.VO
{
    using System;

    [Serializable]
    public class Node
    {
        public Vector2 Position;

        public string Name;
        public string[] Fields;
    }
}