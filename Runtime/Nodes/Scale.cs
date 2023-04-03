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
                AddOverride(targetValue);
                targetValue.localScale = value;
            }
        }

        public override void Clear()
        {
            if (targetValue)
                RemoveOverride(targetValue);
        }

        public static void AddOverride(Transform t)
        {
            if (!t.GetOrAddComponent(out ScaleOverride o))
                o.Original = t.localScale;
        }

        public static void RemoveOverride(Transform t)
        {
            if (t.TryGetComponent(out ScaleOverride o))
            {
                t.localScale = o.Original;
                o.SafeDestroy();
            }
        }
    }
}
