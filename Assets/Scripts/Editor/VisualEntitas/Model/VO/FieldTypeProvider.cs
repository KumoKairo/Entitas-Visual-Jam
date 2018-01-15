using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entitas.Visual.Model.VO
{
    public class FieldTypeProvider
    {
        public List<Type> Types;

        public FieldTypeProvider()
        {
            Types = new List<Type>();
        }

        public void InitializeWithSimpleTypes()
        {
            Types.Clear();
            Types.Add(typeof(float));
            Types.Add(typeof(int));
            Types.Add(typeof(string));
            Types.Add(typeof(Vector3));
            Types.Add(typeof(Vector2));
        }
    }
}
