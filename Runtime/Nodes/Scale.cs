using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Bounds))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Scale : TransformUnit
    {
        protected override Vector3 Vector
        {
            get => targetValue.localScale;
            set
            {
                ScaleOverride.Add(targetValue);
                targetValue.localScale = value;
            }
        }

        public override void Clear() => ScaleOverride.Remove(targetValue);
    }
}
