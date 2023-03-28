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
                AddOverride();
                targetValue.localEulerAngles = value;
            }
        }

        protected void AddOverride()
        {
            if (!targetValue.GetOrAddComponent(out RotationOverride o))
                o.original = targetValue.localRotation;
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
