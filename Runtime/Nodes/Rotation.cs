using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Quaternion))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Rotation : TransformUnit
    {
        protected override Vector3 Vector
        {
            get => targetValue.localEulerAngles;
            set
            {
                AddOverride(targetValue);
                targetValue.localEulerAngles = value;
            }
        }

        public override void Clear()
        {
            if (targetValue)
                RemoveOverride(targetValue);
        }

        public static RotationOverride AddOverride(Transform t)
        {
            if (!t.GetOrAddComponent(out RotationOverride o))
                o.Original = t.localRotation;
            return o;
        }

        public static void RemoveOverride(Transform t)
        {
            if (t.TryGetComponent(out RotationOverride o))
            {
                t.localRotation = o.Original;
                o.SafeDestroy();
            }
        }
    }
}
