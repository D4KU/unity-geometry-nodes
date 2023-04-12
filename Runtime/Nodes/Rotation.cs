using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Change the local rotation of an object
    /// </summary>
    [TypeIcon(typeof(Quaternion))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Rotation : TransformUnit
    {
        /// <summary>
        /// Objects overridden since the last reset
        /// </summary>
        private readonly HashSet<Transform> overriden = new();

        protected override Vector3 Vector
        {
            get => targetValue.localEulerAngles;
            set
            {
                overriden.Add(targetValue);
                RotationOverride.Add(targetValue);
                targetValue.localEulerAngles = value;
            }
        }

        public override void Clear()
        {
            foreach (Transform t in overriden)
                RotationOverride.Remove(t);
            overriden.Clear();
        }
    }
}
