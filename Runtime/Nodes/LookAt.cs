using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Ray))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class LookAt : Rotation
    {
        protected override Vector3 Convert(Vector3 v)
        {
            if (targetValue.parent)
                v = targetValue.parent.InverseTransformPoint(v);
            return Quaternion.LookRotation(v - targetValue.localPosition).eulerAngles;
        }
    }
}
