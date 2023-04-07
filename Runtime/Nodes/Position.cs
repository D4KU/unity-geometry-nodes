using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Vector3))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Position : TransformUnit
    {
        protected override Vector3 Vector
        {
            get => targetValue.localPosition;
            set
            {
                PositionOverride.Add(targetValue);
                targetValue.localPosition = value;
            }
        }

        public override void Clear() => PositionOverride.Remove(targetValue);
    }
}
