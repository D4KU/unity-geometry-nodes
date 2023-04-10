using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Rotates an object so that it points its forward axis to a given point
    /// </summary>
    [TypeIcon(typeof(Ray))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class LookAt : Rotation
    {
        [DoNotSerialize] public ValueInput roll;

        protected override void Definition()
        {
            base.Definition();
            roll = ValueInput<float>(nameof(roll));
        }

        protected override void Execute(Flow flow)
        {
            targetValue = flow.GetValue<Transform>(target);
            Quaternion origRot = RotationOverride.Add(targetValue).Original;

            // Keep original roll when no value is connected
            Vector3 up = roll.hasValidConnection
                ? Quaternion.Euler(0, flow.GetValue<float>(roll), 0) * Vector3.right
                : origRot * Vector3.up;

            // Every unconnected point coordinate stays zero
            Vector3 forward = Vector3.zero;

            if (x.hasValidConnection) forward.x = flow.GetValue<float>(x);
            if (y.hasValidConnection) forward.y = flow.GetValue<float>(y);
            if (z.hasValidConnection) forward.z = flow.GetValue<float>(z);

            // When all coordinates are zero, point the forward axis forward
            // (i.e. do nothing)
            if (forward == Vector3.zero)
                forward = Vector3.forward;

            targetValue.localRotation = Quaternion.LookRotation(forward, up);
        }
    }
}
