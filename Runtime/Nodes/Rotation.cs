using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Quaternion))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Rotation : TransformUnit
    {
        protected override Vector3 Vector
        {
            get => targetValue.localEulerAngles;
            set => targetValue.localEulerAngles = value;
        }
    }
}
