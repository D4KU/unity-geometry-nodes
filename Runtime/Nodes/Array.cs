using System.Collections.Generic;
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
        private ValueOutput parent;
        private ValueOutput copies;
        private ValueOutput start;
        private ValueOutput end;

        private Transform parentOut;
        private readonly List<Transform> copiesOut = new();

        protected override void Definition()
        {
            base.Definition();

            original = ValueInput<Transform>(nameof(original));
            offset   = ValueInput(nameof(offset), Vector3.right);
            count    = ValueInput(nameof(count), 1);

            parent = ValueOutput(nameof(parent), _ => parentOut);
            copies = ValueOutput<List<Transform>>(nameof(copies), _ => copiesOut);
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
            if (parentOut == null)
                return;

            foreach (Transform child in parentOut)
            {
                if (!child.GetComponent<Copy>() && !child.GetComponent<Group>())
                {
                    child.parent = parentOut.parent;
                    Position.RemoveOverride(child);
                }
            }

            parentOut.SafeDestroy();
            copiesOut.Clear();
        }

        protected override void Execute(Flow flow)
        {
            var voffset = flow.GetValue<Vector3>(offset);
            int vcount  = flow.GetValue<int>(count);
            Transform voriginal = flow.GetValue<Transform>(original);

            if (copiesOut.Count == 0)
                copiesOut.Add(voriginal);

            flow.SetValue(start, voriginal.localPosition - voffset);
            flow.SetValue(end  , voriginal.localPosition + voffset * (vcount + 1));
            voriginal.Group(ref parentOut, nameof(Array));

            // Destroy surplus copies
            List<Transform> toDestroy = new();
            int childCnt = parentOut.childCount;
            for (int i = childCnt - 1; i >= 0 && childCnt - toDestroy.Count > vcount; i--)
            {
                Transform child = parentOut.GetChild(i);
                if (child != voriginal && child.GetComponent<Copy>())
                {
                    toDestroy.Add(child);
                    copiesOut.Remove(child);
                }
            }
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
            Position.AddOverride(voriginal);
            int? origId = voriginal.TryGetComponent(out Group g) ? g.Id : null;
            int j = 0;
            foreach (Transform child in parentOut)
            {
                child.localPosition = voffset * j++;
                if (child.TryGetComponent(out Group gr) && gr.Id != origId)
                    continue;

                child.localRotation = voriginal.localRotation;
                child.localScale = voriginal.localScale;
            }
        }
    }
}
