using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Ray))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class LookAt : Rotation
    {
        protected override ValueInput VectorInput => ValueInput("Position", Vector3.zero);
        protected override Vector3 Convert(Vector3 v)
        {
            if (targetValue.parent)
                v = targetValue.parent.InverseTransformPoint(v);
            return Quaternion.LookRotation(v - targetValue.localPosition).eulerAngles;
        }
    }
}
