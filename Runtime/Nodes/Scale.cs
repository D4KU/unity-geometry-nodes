using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Bounds))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Scale : TransformUnit
    {
        protected override ValueInput VectorInput => ValueInput("Scale", Vector3.one);
        protected override Vector3 Vector => targetValue.localScale;
        protected override void SetVector(Vector3 v) => targetValue.localScale = v;
    }
}
