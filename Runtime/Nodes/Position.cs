using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Vector3))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Position : TransformUnit
    {
        protected override Vector3 Vector
        {
            get => targetValue.localPosition;
            set
            {
                AddOverride(targetValue);
                targetValue.localPosition = value;
            }
        }

        public override void Clear()
        {
            if (targetValue)
                RemoveOverride(targetValue);
        }

        public static void AddOverride(Transform t)
        {
            if (!t.GetOrAddComponent(out PositionOverride o))
                o.original = t.localPosition;
        }

        public static void RemoveOverride(Transform t)
        {
            if (t.TryGetComponent(out PositionOverride o))
            {
                t.localPosition = o.original;
                o.SafeDestroy();
            }
        }
    }
}
