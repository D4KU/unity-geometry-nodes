using Unity.VisualScripting;

namespace GeometryNodes
{
    [TypeIcon(typeof(GraphInput))]
    public class Entry : EventUnit<EmptyEventArgs>
    {
        protected override bool register => true;

        public const string HOOK_NAME = "GeometryNodes.Entry";

        public override EventHook GetHook(GraphReference reference)
            => new(HOOK_NAME, reference.gameObject);
    }
}
