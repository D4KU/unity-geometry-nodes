using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [TypeIcon(typeof(Variables))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Mirror : GeometryUnit
    {
        private ValueInput original;
        private ValueInput pivot;
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
            pivot = ValueInput(nameof(pivot), Vector3.zero);
            x = ValueInput(nameof(x), true);
            y = ValueInput(nameof(y), true);
            z = ValueInput(nameof(z), true);
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
            Transform voriginal = flow.GetValue<Transform>(original);
            Vector3 vpivot = flow.GetValue<Vector3>(pivot);
            Vector3 m = new(flow.GetValue<bool>(x) ? -1 : 1,
                            flow.GetValue<bool>(y) ? -1 : 1,
                            flow.GetValue<bool>(z) ? -1 : 1);

            voriginal.EnsureParent(ref parentOut, nameof(Mirror));
            voriginal.parent = parentOut;

            copyOut.SafeDestroy();
            copyOut = voriginal.Duplicate(parentOut);
            copyOut.localPosition = Vector3.Scale(voriginal.localPosition - vpivot, m) + vpivot;
            copyOut.localRotation = voriginal.localRotation;
            copyOut.localScale = Vector3.Scale(voriginal.localScale, m);
        }
    }
}
