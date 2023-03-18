using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Vector3))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Position : TransformUnit
    {
        protected override Vector3 Vector
        {
            get => targetValue.localPosition;
            set => targetValue.localPosition = value;
        }
    }
}
