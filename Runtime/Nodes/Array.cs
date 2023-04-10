using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Creates a variable amount of copies from a template object and
    /// positions them with a given offset
    /// </summary>
    [TypeIcon(typeof(List<>))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Array : GeometryUnit
    {
        [DoNotSerialize] public ValueInput original;
        [DoNotSerialize] public ValueInput offset;
        [DoNotSerialize] public ValueInput count;
        [DoNotSerialize] public ValueOutput parent;
        [DoNotSerialize] public ValueOutput copies;
        [DoNotSerialize] public ValueOutput start;
        [DoNotSerialize] public ValueOutput end;

        /// <summary>
        /// Parent created for Original and its copies
        /// </summary>
        private Transform parentOut;

        /// <summary>
        /// List of copies created with original as first entry
        /// </summary>
        private readonly List<Transform> copiesOut = new();

        protected override IEnumerable<ValueInput> Required => new[] { original };

        protected override void Definition()
        {
            original = ValueInput<Transform>(nameof(original));
            offset   = ValueInput(nameof(offset), Vector3.right);
            count    = ValueInput(nameof(count), 1);

            parent = ValueOutput(nameof(parent), _ => parentOut);
            copies = ValueOutput<List<Transform>>(nameof(copies), _ => copiesOut);
            start = ValueOutput<Vector3>(nameof(start));
            end = ValueOutput<Vector3>(nameof(end));

            base.Definition();
            Assignment(input, parent);
        }

        public override void Clear()
        {
            copiesOut.Clear();
            if (parentOut == null)
                return;

            /// Don't iterate over <see cref="copiesOut"/> because a
            /// downstream node could have moved the copy to another parent
            /// in the meantime
            foreach (Transform child in parentOut.ChildrenToRescue())
            {
                child.parent = parentOut.parent;
                PositionOverride.Remove(child);
            }

            parentOut.SafeDestroy();
        }

        protected override void Execute(Flow flow)
        {
            var voffset = flow.GetValue<Vector3>(offset);
            int vcount  = flow.GetValue<int>(count);
            Transform voriginal = flow.GetValue<Transform>(original);

            flow.SetValue(start, voriginal.localPosition - voffset);
            flow.SetValue(end  , voriginal.localPosition + voffset * (vcount + 1));

            if (copiesOut.Count == 0)
                copiesOut.Add(voriginal);

            if (parentOut == null)
                parentOut = voriginal.Group(nameof(Array));

            // Find surplus copies to destroy
            List<Transform> toDestroy = new();
            int childCnt = parentOut.childCount;

            for (int i = childCnt - 1; i >= 0 && childCnt - toDestroy.Count > vcount; i--)
            {
                Transform child = parentOut.GetChild(i);

                // Even the original can have a Copy Component if created by
                // an upstream node. Then this node has no ownership of it.
                if (child != voriginal && child.GetComponent<Copy>())
                {
                    toDestroy.Add(child);
                    copiesOut.Remove(child);
                }
            }

            // Destroy copies in separate loop to not destroy children while
            // iterating over parent
            foreach (Transform t in toDestroy)
                t.SafeDestroy();

            // Create newly wanted copies
            for (int i = parentOut.childCount; i < vcount; i++)
            {
                Transform copy = voriginal.Duplicate(parentOut);
                copy.name += $"({i})";
                copiesOut.Add(copy);
            }

            // Position children
            PositionOverride.Add(voriginal);
            int? origId = voriginal.GroupId();
            int j = 0;
            foreach (Transform child in parentOut)
            {
                child.localPosition = voffset * j++;

                // Only rotate and scale when original already was a group
                // or child and original both aren't one. Otherwise the group
                // was created by a downstream node handling rotation
                // and scale with its copies.
                if (child.GroupId() == origId)
                {
                    child.localRotation = voriginal.localRotation;
                    child.localScale = voriginal.localScale;
                }
            }
        }
    }
}
