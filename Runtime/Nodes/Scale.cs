using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Change the local scale of an object
    /// </summary>
    [TypeIcon(typeof(Bounds))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Scale : TransformUnit
    {
        /// <summary>
        /// Objects overridden since the last reset
        /// </summary>
        private readonly HashSet<Transform> overriden = new();

        protected override Vector3 Vector
        {
            get => targetValue.localScale;
            set
            {
                overriden.Add(targetValue);
                ScaleOverride.Add(targetValue);
                targetValue.localScale = value;
            }
        }

        public override void Clear()
        {
            foreach (Transform t in overriden)
                ScaleOverride.Remove(t);
            overriden.Clear();
        }
    }
}
