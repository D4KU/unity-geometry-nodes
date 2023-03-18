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
        private Transform voriginal;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector3 originalScale;

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

        public override void Clear()
        {
            if (voriginal)
            {
                if (parentOut)
                    voriginal.parent = parentOut.parent;

                voriginal.localPosition = originalPosition;
                voriginal.localRotation = originalRotation;
                voriginal.localScale = originalScale;
                voriginal = null;
            }

            parentOut.SafeDestroy();
        }

        protected override void Execute(Flow flow)
        {
            if (voriginal == null)
            {
                voriginal = flow.GetValue<Transform>(original);
                originalPosition = voriginal.localPosition;
                originalRotation = voriginal.localRotation;
                originalScale = voriginal.localScale;
            }

            var voffset = flow.GetValue<Vector3>(offset);
            int vcount  = flow.GetValue<int>(count);

            flow.SetValue(start, voriginal.localPosition - voffset);
            flow.SetValue(end  , voriginal.localPosition + voffset * (vcount + 1));
            voriginal.MakeSibling(ref parentOut, nameof(Array));
            voriginal.parent = parentOut;

            // Destroy surplus copies
            List<Transform> toDestroy = parentOut
                .Cast<Transform>()
                .Where(x => x != voriginal)
                .Skip(vcount)
                .ToList();
            foreach (Transform t in toDestroy)
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
