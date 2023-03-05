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
        private ValueOutput instances;
        private ValueOutput start;
        private ValueOutput end;

        private readonly List<List<Transform>> instancesOut = new();

        protected override void Definition()
        {
            base.Definition();

            original = ValueInput<Transform>(nameof(original));
            offset   = ValueInput(nameof(offset), Vector3.right);
            count    = ValueInput(nameof(count), 1);
            index    = ValueInput(nameof(index), 0);

            instances = ValueOutput(nameof(instances), GetOutputValue);
            start = ValueOutput<Vector3>(nameof(start));
            end = ValueOutput<Vector3>(nameof(end));

            Requirement(original, input);
            Assignment(input, instances);
        }

        private List<Transform> GetOutputValue(Flow flow)
            => instancesOut[flow.GetValue<int>(index)];

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
            foreach (List<Transform> list in instancesOut)
                foreach (Transform transform in list.Skip(1))
                transform.SaveDestroy();
            instancesOut.Clear();
        }

        protected override void Execute(Flow flow)
        {
            var voriginal = flow.GetValue<Transform>(original);
            var voffset   = flow.GetValue<Vector3>(offset);
            int vcount    = Mathf.Max(0, flow.GetValue<int>(count));
            int vindex    = Mathf.Max(0, flow.GetValue<int>(index));

            flow.SetValue(start, voriginal.localPosition - voffset);
            flow.SetValue(end  , voriginal.localPosition + voffset * (vcount + 1));

            List<Transform> list;
            if (vindex < instancesOut.Count)
            {
                list = instancesOut[vindex];
            }
            else
            {
                list = new List<Transform> { voriginal };
                instancesOut.Add(list);
            }

            // Destroy surplus copies
            for (int i = list.Count - 1; i > vcount; i--)
            {
                list[i].SaveDestroy();
                list.RemoveAt(i);
            }

            // Create newly wanted copies
            Quaternion rotation = voriginal.rotation;
            for (int i = list.Count; i <= vcount; i++)
            {
                Vector3 position = i * voffset + voriginal.localPosition;
                if (voriginal.parent)
                    position = voriginal.parent.TransformPoint(position);

                Transform copy = voriginal.Duplicate(
                        position: position,
                        rotation: rotation,
                        parent: voriginal.parent);
                copy.name += $"({i})";
                list.Add(copy);
            }
        }
    }
}
