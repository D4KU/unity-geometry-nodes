using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Quaternion))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Rotation : TransformUnit
    {
        protected override ValueInput VectorInput => ValueInput("Rotation", Vector3.zero);
        protected override Vector3 Vector => targetValue.localEulerAngles;
        protected override void SetVector(Vector3 v) => targetValue.localEulerAngles = v;
    }
}
