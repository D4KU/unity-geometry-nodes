using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Vector3))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Position : TransformUnit
    {
        protected override ValueInput VectorInput => ValueInput("Position", Vector3.zero);
        protected override Vector3 Vector => targetValue.localPosition;
        protected override void SetVector(Vector3 v) => targetValue.localPosition = v;
    }
}
