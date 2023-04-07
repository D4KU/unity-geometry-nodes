using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(Merge))]
    internal class MergeDescriptor : UnitDescriptor<Merge>
    {
        public MergeDescriptor(Merge unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Merge.parent) => Descriptions.PARENT,
                _ => string.Empty,
            };
        }
    }
}
