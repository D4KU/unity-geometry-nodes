using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Ray))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class LookAt : Rotation
    {
        protected ValueInput roll;

        protected override void Definition()
        {
            base.Definition();
            roll = ValueInput<float>(nameof(roll));
        }

        protected override void Execute(Flow flow)
        {
            targetValue = flow.GetValue<Transform>(target);
            Quaternion origRot = AddOverride(targetValue).Original;

            Vector3 up = roll.hasValidConnection
                ? Quaternion.Euler(0, flow.GetValue<float>(roll), 0) * Vector3.right
                : origRot * Vector3.up;

            Vector3 forward = Vector3.zero;

            if (x.hasValidConnection) forward.x = flow.GetValue<float>(x);
            if (y.hasValidConnection) forward.y = flow.GetValue<float>(y);
            if (z.hasValidConnection) forward.z = flow.GetValue<float>(z);

            if (forward == Vector3.zero)
                forward = Vector3.forward;

            targetValue.localRotation = Quaternion.LookRotation(forward, up);
        }
    }
}
