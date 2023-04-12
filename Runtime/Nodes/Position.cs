using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Change the local position of an object
    /// </summary>
    [TypeIcon(typeof(Vector3))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Position : TransformUnit
    {
        /// <summary>
        /// Objects overridden since the last reset
        /// </summary>
        private readonly HashSet<Transform> overriden = new();

        protected override Vector3 Vector
        {
            get => targetValue.localPosition;
            set
            {
                overriden.Add(targetValue);
                PositionOverride.Add(targetValue);
                targetValue.localPosition = value;
            }
        }

        public override void Clear()
        {
            foreach (Transform t in overriden)
                PositionOverride.Remove(t);
            overriden.Clear();
        }
    }
}
