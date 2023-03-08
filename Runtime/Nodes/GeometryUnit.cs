using Unity.VisualScripting;

namespace GeometryNodes
{
    [UnitCategory(GeometryUnit.CATEGORY)]
    internal abstract class GeometryUnit : Unit
    {
        public const string SUBTITLE = "Geometry Node";
        public const string CATEGORY = "Geometry Nodes";

        [PortLabelHidden, DoNotSerialize] public ControlInput input;
        [PortLabelHidden, DoNotSerialize] public ControlOutput output;

        protected override void Definition()
        {
            input = ControlInput(nameof(input), WrapExecute);
            output = ControlOutput(nameof(output));
            Succession(input, output);
        }

        public override void AfterAdd()
        {
            base.AfterAdd();
            graph.controlConnections.ItemRemoved += OnInputDisconnected;
        }

        public override void BeforeRemove()
        {
            base.BeforeRemove();
            graph.controlConnections.ItemRemoved -= OnInputDisconnected;
            Clear();
        }

        private void OnInputDisconnected(IUnitConnection connection)
            => OnPortDisconnected(connection, input);

        protected void OnPortDisconnected(IUnitConnection connection, IUnitInputPort destination)
        {
            if (connection.destination != destination)
                return;
            output.ClearDownstream();
            Clear();
        }

        private ControlOutput WrapExecute(Flow flow)
        {
            Execute(flow);
            return output;
        }

        abstract public void Clear();
        abstract protected void Execute(Flow flow);
    }
}
