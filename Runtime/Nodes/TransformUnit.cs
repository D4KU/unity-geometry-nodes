using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    internal abstract class TransformUnit : GeometryUnit
    {
        protected ValueInput target;
        protected ValueInput vector;
        protected ValueInput x;
        protected ValueInput y;
        protected ValueInput z;

        protected Transform targetValue;
        protected Vector3? originalValue;

        protected override void Definition()
        {
            base.Definition();

            target = ValueInput<Transform>(nameof(target));
            vector = VectorInput;
            x = ValueInput(nameof(x), true);
            y = ValueInput(nameof(y), true);
            z = ValueInput(nameof(z), true);

            Requirement(target, input);
            Requirement(vector, input);
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

        public override void Clear()
        {
            if (targetValue && originalValue.HasValue)
                SetVector(originalValue.Value);
        }

        protected override void Execute(Flow flow)
        {
            targetValue = flow.GetValue<Transform>(target);
            originalValue ??= Vector;
            Vector3 vvector = flow.GetValue<Vector3>(vector);

            if (!flow.GetValue<bool>(x)) vvector.x = originalValue.Value.x;
            if (!flow.GetValue<bool>(y)) vvector.y = originalValue.Value.y;
            if (!flow.GetValue<bool>(z)) vvector.z = originalValue.Value.z;

            SetVector(Convert(vvector));
        }

        protected abstract ValueInput VectorInput { get; }
        protected abstract Vector3 Vector { get; }
        protected abstract void SetVector(Vector3 v);
        protected virtual Vector3 Convert(Vector3 v) => v;
    }
}
