using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Bounds))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Scale : TransformUnit
    {
        protected override Vector3 Vector
        {
            get => targetValue.localScale;
            set
            {
                if (!targetValue.GetOrAddComponent(out ScaleOverride o))
                    o.original = Vector;

                targetValue.localScale = value;
            }
        }

        public override void Clear()
        {
            if (targetValue && targetValue.TryGetComponent(out ScaleOverride o))
            {
                Vector = o.original;
                o.SafeDestroy();
            }
        }
    }
}
