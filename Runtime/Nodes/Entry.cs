using Unity.VisualScripting;

namespace GeometryNodes
{
    /// <summary>
    /// Entry point from which to start traversing through Geometry Nodes
    /// once and input variable changes
    /// </summary>
    [TypeIcon(typeof(GraphInput))]
    public class Entry : EventUnit<EmptyEventArgs>
    {
        protected override bool register => true;

        /// <summary>
        /// ID to trigger this event unit from the outside
        /// </summary>
        public const string HOOK_NAME = "GeometryNodes.Entry";

        public override EventHook GetHook(GraphReference reference)
            => new(HOOK_NAME, reference.gameObject);
    }
}
