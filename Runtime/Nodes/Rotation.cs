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
                if (!targetValue.GetOrAddComponent(out RotationOverride o))
                    o.original = targetValue.localRotation;

                targetValue.localEulerAngles = value;
            }
        }

        public override void Clear()
        {
            if (targetValue && targetValue.TryGetComponent(out RotationOverride o))
            {
                targetValue.localRotation = o.original;
                o.SafeDestroy();
            }
        }
    }
}
