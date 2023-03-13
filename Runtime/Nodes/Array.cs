using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(List<>))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Array : GeometryUnit
    {
        private ValueInput original;
        private ValueInput offset;
        private ValueInput count;
        private ValueInput index;
        private ValueOutput parent;
        private ValueOutput start;
        private ValueOutput end;

        private Transform parentOut;

        protected override void Definition()
        {
            base.Definition();

            original = ValueInput<Transform>(nameof(original));
            offset   = ValueInput(nameof(offset), Vector3.right);
            count    = ValueInput(nameof(count), 1);
            index    = ValueInput(nameof(index), 0);

            parent = ValueOutput(nameof(parent), _ => parentOut);
            start = ValueOutput<Vector3>(nameof(start));
            end = ValueOutput<Vector3>(nameof(end));

            Requirement(original, input);
            Assignment(input, parent);
        }

        public override void AfterAdd()
        {
            base.AfterAdd();
            graph.valueConnections.ItemRemoved += OnOriginalDisconnected;
        }

        public override void BeforeRemove()
        {
            graph.valueConnections.ItemRemoved -= OnOriginalDisconnected;
            base.BeforeRemove();
        }

        private void OnOriginalDisconnected(IUnitConnection connection)
            => OnPortDisconnected(connection, original);

        public override void Clear() => parentOut.SafeDestroy();

        protected override void Execute(Flow flow)
        {
            var voriginal = flow.GetValue<Transform>(original);
            var voffset   = flow.GetValue<Vector3>(offset);
            int vcount    = flow.GetValue<int>(count);

            flow.SetValue(start, voriginal.localPosition - voffset);
            flow.SetValue(end  , voriginal.localPosition + voffset * (vcount + 1));
            voriginal.EnsureParent(ref parentOut, nameof(Array));

            // Destroy surplus copies
            foreach (Transform t in parentOut.Cast<Transform>().Skip(vcount).ToList())
                t.SafeDestroy();

            // Create newly wanted copies
            for (int i = parentOut.childCount; i <= vcount; i++)
                voriginal.Duplicate(parentOut).name += $"({i})";

            int j = 0;
            foreach (Transform child in parentOut)
            {
                child.localPosition = voffset * j++;
                child.localRotation = voriginal.localRotation;
                child.localScale = voriginal.localScale;
            }
        }
    }
}
