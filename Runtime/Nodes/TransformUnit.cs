using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Base class for nodes changing position, rotation, or scale of an
    /// object
    /// </summary>
    public abstract class TransformUnit : GeometryUnit
    {
        [DoNotSerialize] public ValueInput target;
        [DoNotSerialize] public ValueInput x;
        [DoNotSerialize] public ValueInput y;
        [DoNotSerialize] public ValueInput z;

        protected Transform targetValue;

        protected override IEnumerable<ValueInput> Required => new[] { target };

        protected override void Definition()
        {
            target = ValueInput<Transform>(nameof(target));
            x = ValueInput<float>(nameof(x));
            y = ValueInput<float>(nameof(y));
            z = ValueInput<float>(nameof(z));

            base.Definition();
        }

        protected override void Execute(Flow flow)
        {
            targetValue = flow.GetValue<Transform>(target);
            Vector3 v = Vector;

            // Keep original values on each unconnected axis
            if (x.hasValidConnection) v.x = flow.GetValue<float>(x);
            if (y.hasValidConnection) v.y = flow.GetValue<float>(y);
            if (z.hasValidConnection) v.z = flow.GetValue<float>(z);

            Vector = v;
        }

        /// <summary>
        /// The property this node manipulates
        /// </summary>
        protected abstract Vector3 Vector { get; set; }
    }
}
