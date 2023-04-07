using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Quaternion))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Rotation : TransformUnit
    {
        protected override Vector3 Vector
        {
            get => targetValue.localEulerAngles;
            set
            {
                RotationOverride.Add(targetValue);
                targetValue.localEulerAngles = value;
            }
        }

        public override void Clear() => RotationOverride.Remove(targetValue);
    }
}
