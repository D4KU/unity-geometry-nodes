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
            set => targetValue.localScale = value;
        }
    }
}
