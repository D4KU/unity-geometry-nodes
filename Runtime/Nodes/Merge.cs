using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

namespace GeometryNodes
{
    [TypeIcon(typeof(ISelectUnit))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Merge : GeometryUnit
    {
        [Serialize, Inspectable]
        public int valueInputCount = 2;

        private ValueInput[] children;
        private ValueOutput parent;

        private Transform parentOut;

        protected override void Definition()
        {
            base.Definition();

            children = new ValueInput[valueInputCount];
            for (int i = 0; i < children.Length; i++)
                children[i] = ValueInput<Transform>(i.ToString());

            parent = ValueOutput(nameof(parent), _ => parentOut);
            Assignment(input, parent);
        }

        public override void AfterAdd()
        {
            base.AfterAdd();
            graph.valueConnections.ItemRemoved += OnChildDisconnected;
        }

        public override void BeforeRemove()
        {
            graph.valueConnections.ItemRemoved -= OnChildDisconnected;
            base.BeforeRemove();
        }

        private void OnChildDisconnected(IUnitConnection connection)
            => OnAnyPortDisconnected(connection, children);

        public override void Clear()
        {
            if (parentOut == null)
                return;

            foreach (Transform child in parentOut.Cast<Transform>().ToList())
                child.parent = parentOut.parent;
            parentOut.SafeDestroy();
        }

        protected override void Execute(Flow flow)
        {
            bool first = true;
            foreach (ValueInput input in children)
            {
                if (!input.hasValidConnection)
                    continue;

                Transform child = flow.GetValue<Transform>(input);
                if (child == null)
                    continue;

                if (first)
                {
                    child.Group(ref parentOut, nameof(Merge));
                    first = false;
                    continue;
                }

                child.parent = parentOut;
            }
        }
    }
}
