using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Variables))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Mirror : GeometryUnit
    {
        private ValueInput original;
        private ValueInput x;
        private ValueInput y;
        private ValueInput z;
        private ValueOutput parent;
        private ValueOutput copy;

        private Transform parentOut;
        private Transform copyOut;

        protected override void Definition()
        {
            base.Definition();

            original = ValueInput<Transform>(nameof(original));
            x = ValueInput<float>(nameof(x));
            y = ValueInput<float>(nameof(y));
            z = ValueInput<float>(nameof(z));

            parent = ValueOutput(nameof(parent), _ => parentOut);
            copy = ValueOutput(nameof(copy), _ => copyOut);

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
                if (child != copyOut)
                    child.parent = parentOut.parent;

            parentOut.SafeDestroy();
        }

        protected override void Execute(Flow flow)
        {
            Vector3 p = Vector3.zero;
            Vector3 s = Vector3.one;

            if (x.hasValidConnection)
            {
                p.x = flow.GetValue<float>(x);
                s.x = -1;
            }
            if (y.hasValidConnection)
            {
                p.y = flow.GetValue<float>(y);
                s.y = -1;
            }
            if (z.hasValidConnection)
            {
                p.z = flow.GetValue<float>(z);
                s.z = -1;
            }

            Transform voriginal = flow.GetValue<Transform>(original);
            voriginal.Group(ref parentOut, nameof(Mirror));

            copyOut.SafeDestroy();
            copyOut = voriginal.Duplicate(parentOut);
            copyOut.localPosition = Vector3.Scale(parentOut.InverseTransformPoint(voriginal.position) - p, s) + p;
            copyOut.localRotation = voriginal.localRotation;
            copyOut.localScale = Vector3.Scale(voriginal.localScale, s);
        }
    }
}
