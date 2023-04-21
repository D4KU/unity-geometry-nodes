using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace GeometryNodes
{
    /// <summary>
    /// Base class for every Geometry Node
    /// </summary>
    [UnitCategory(CATEGORY)]
    public abstract class GeometryUnit : Unit
    {
        public const string SUBTITLE = "Geometry Node";
        public const string CATEGORY = "Geometry Nodes";

        [PortLabelHidden, DoNotSerialize] public ControlInput input;
        [PortLabelHidden, DoNotSerialize] public ControlOutput output;

        /// <summary>
        /// Input ports causing the node to reset when disconnected
        /// </summary>
        protected virtual IEnumerable<ValueInput> Required => new ValueInput[0];

        protected override void Definition()
        {
            input = ControlInput(nameof(input), WrapExecute);
            output = ControlOutput(nameof(output));
            Succession(input, output);

            foreach (var i in Required)
                Requirement(i, input);
        }

        public override void AfterAdd()
        {
            base.AfterAdd();
            graph.controlConnections.ItemRemoved += OnControlDisconnected;
            graph.valueConnections.ItemRemoved += OnValueDisconnected;
        }

        public override void BeforeRemove()
        {
            base.BeforeRemove();
            graph.controlConnections.ItemRemoved -= OnControlDisconnected;
            graph.valueConnections.ItemRemoved -= OnValueDisconnected;
            ClearDownstream();
        }

        private void OnControlDisconnected(IUnitConnection connection)
        {
            if (connection.destination == input)
                ClearDownstream();
        }

        private void OnValueDisconnected(IUnitConnection connection)
        {
            if (Required.Contains(connection.destination))
                ClearDownstream();
        }

        /// Recursively clear nodes linked to this node's output and this
        /// node itself
        private void ClearDownstream()
        {
            // Don't use input.ClearDownstream() because input might already
            // have no connection anymore
            output.ClearDownstream();
            Clear();
        }

        /// <summary>
        /// Wrapper so abstract Execute() doesn't have to return output port
        /// </summary>
        private ControlOutput WrapExecute(Flow flow)
        {
            Execute(flow);
            return output;
        }

        /// <summary>
        /// Main behaviour of the node
        /// </summary>
        abstract protected void Execute(Flow flow);

        /// <summary>
        /// Undo everything done in <see cref="Execute"/>
        /// </summary>
        abstract public void Clear();
    }
}
