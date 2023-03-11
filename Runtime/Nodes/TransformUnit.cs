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
        protected Vector3? originalVector;

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
            if (targetValue && originalVector.HasValue)
                SetVector(originalVector.Value);
        }

        protected override void Execute(Flow flow)
        {
            targetValue = flow.GetValue<Transform>(this.target);
            Vector3 source = Vector;
            Vector3 target = flow.GetValue<Vector3>(vector);
            originalVector ??= source;

            if (!flow.GetValue<bool>(x)) target.x = source.x;
            if (!flow.GetValue<bool>(y)) target.y = source.y;
            if (!flow.GetValue<bool>(z)) target.z = source.z;

            SetVector(Convert(target));
        }

        protected abstract ValueInput VectorInput { get; }
        protected abstract Vector3 Vector { get; }
        protected abstract void SetVector(Vector3 v);
        protected virtual Vector3 Convert(Vector3 v) => v;
    }
}
