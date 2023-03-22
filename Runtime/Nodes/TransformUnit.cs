using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    internal abstract class TransformUnit : GeometryUnit
    {
        protected ValueInput target;
        protected ValueInput x;
        protected ValueInput y;
        protected ValueInput z;

        protected Transform targetValue;

        protected override void Definition()
        {
            base.Definition();

            target = ValueInput<Transform>(nameof(target));
            x = ValueInput<float>(nameof(x));
            y = ValueInput<float>(nameof(y));
            z = ValueInput<float>(nameof(z));

            Requirement(target, input);
        }

        public override void AfterAdd()
        {
            base.AfterAdd();
            graph.valueConnections.ItemRemoved += OnTargetDisconnected;
        }

        public override void BeforeRemove()
        {
            graph.valueConnections.ItemRemoved -= OnTargetDisconnected;
            base.BeforeRemove();
        }

        private void OnTargetDisconnected(IUnitConnection connection)
            => OnPortDisconnected(connection, target);

        protected override void Execute(Flow flow)
        {
            targetValue = flow.GetValue<Transform>(target);
            Vector3 v = Vector;
            if (x.hasValidConnection) v.x = flow.GetValue<float>(x);
            if (y.hasValidConnection) v.y = flow.GetValue<float>(y);
            if (z.hasValidConnection) v.z = flow.GetValue<float>(z);
            Vector = Convert(v);
        }

        protected abstract Vector3 Vector { get; set; }
        protected virtual Vector3 Convert(Vector3 v) => v;
    }
}
