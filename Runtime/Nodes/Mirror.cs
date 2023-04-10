using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Create a copy of an object with inverted scale on one or multiple axes
    /// </summary>
    [TypeIcon(typeof(Variables))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Mirror : GeometryUnit
    {
        [DoNotSerialize] public ValueInput original;
        [DoNotSerialize] public ValueInput x;
        [DoNotSerialize] public ValueInput y;
        [DoNotSerialize] public ValueInput z;
        [DoNotSerialize] public ValueOutput parent;
        [DoNotSerialize] public ValueOutput copy;

        /// <summary>
        /// The parent object created for the original and its mirrored copy
        /// </summary>
        private Transform parentOut;

        /// <summary>
        /// The mirrored copy created
        /// </summary>
        private Transform copyOut;

        protected override IEnumerable<ValueInput> Required => new[] { original };

        protected override void Definition()
        {
            original = ValueInput<Transform>(nameof(original));
            x = ValueInput<float>(nameof(x));
            y = ValueInput<float>(nameof(y));
            z = ValueInput<float>(nameof(z));

            parent = ValueOutput(nameof(parent), _ => parentOut);
            copy = ValueOutput(nameof(copy), _ => copyOut);

            base.Definition();
            Assignment(input, parent);
        }

        public override void Clear()
        {
            if (parentOut == null)
                return;

            foreach (Transform child in parentOut.ChildrenToRescue())
                child.parent = parentOut.parent;

            parentOut.SafeDestroy();
        }

        protected override void Execute(Flow flow)
        {
            // The copy is moved on each mirrored axis so that the signed distance
            // between the original and the pivot is equal to the inverse
            // signed distance between the copy and pivot on the respective axis
            Vector3 pivot = Vector3.zero;

            // On every mirrored axis the scale of the copy is inverted
            Vector3 scale = Vector3.one;

            // Scale and move the original on every axis with a connection
            if (x.hasValidConnection)
            {
                pivot.x = flow.GetValue<float>(x);
                scale.x = -1;
            }
            if (y.hasValidConnection)
            {
                pivot.y = flow.GetValue<float>(y);
                scale.y = -1;
            }
            if (z.hasValidConnection)
            {
                pivot.z = flow.GetValue<float>(z);
                scale.z = -1;
            }

            Transform voriginal = flow.GetValue<Transform>(original);
            if (parentOut == null)
                parentOut = voriginal.Group(nameof(Mirror));

            copyOut.SafeDestroy();
            copyOut = voriginal.Duplicate(parentOut);

            // Pivot point coordinates are in local space of parent
            copyOut.localPosition = pivot + Vector3.Scale(scale,
                parentOut.InverseTransformPoint(voriginal.position) - pivot);
            copyOut.localRotation = voriginal.localRotation;
            copyOut.localScale = Vector3.Scale(scale, voriginal.localScale);
        }
    }
}
