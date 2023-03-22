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
                if (!targetValue.GetOrAddComponent(out PositionOverride o))
                    o.original = Vector;

                targetValue.localPosition = value;
            }
        }

        public override void Clear()
        {
            if (targetValue && targetValue.TryGetComponent(out PositionOverride o))
            {
                Vector = o.original;
                o.SafeDestroy();
            }
        }
    }
}
