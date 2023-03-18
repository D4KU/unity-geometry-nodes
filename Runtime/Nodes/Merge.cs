using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace GeometryNodes
{
    [TypeIcon(typeof(ISelectUnit))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    internal class Merge : GeometryUnit
    {
        private ValueInput children;
        private ValueOutput parent;

        private Transform parentOut;

        protected override void Definition()
        {
            base.Definition();

            children = ValueInput<IEnumerable>(nameof(children));
            parent = ValueOutput(nameof(parent), _ => parentOut);

            Requirement(children, input);
            Assignment(input, parent);
        }

        public override void AfterAdd()
        {
            base.AfterAdd();
            graph.valueConnections.ItemRemoved += OnChildrenDisconnected;
        }

        public override void BeforeRemove()
        {
            graph.valueConnections.ItemRemoved -= OnChildrenDisconnected;
            base.BeforeRemove();
        }

        private void OnChildrenDisconnected(IUnitConnection connection)
            => OnPortDisconnected(connection, children);

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
            foreach (Transform child in flow.GetValue<IEnumerable>(children))
            {
                if (child == null)
                    continue;

                if (first)
                {
                    child.MakeSibling(ref parentOut, nameof(Merge));
                    first = false;
                }

                child.parent = parentOut;
            }
        }
    }
}
