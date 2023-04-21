using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(Duplicate))]
    internal class DuplicateDescriptor : UnitDescriptor<Duplicate>
    {
        public DuplicateDescriptor(Duplicate unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Duplicate.original) => Descriptions.ORIGINAL,
                nameof(Duplicate.count) => "Number of copies to create",
                nameof(Duplicate.copies) => "List of copies created",
                _ => string.Empty,
            };
        }
    }
}
