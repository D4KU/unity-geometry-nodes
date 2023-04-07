using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(Array))]
    internal class ArrayDescriptor : UnitDescriptor<Array>
    {
        public ArrayDescriptor(Array unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Array.original) => Descriptions.ORIGINAL,
                nameof(Array.parent) => Descriptions.PARENT,
                nameof(Array.offset)
                    => "Distance between each copy in local space of original's parent",
                nameof(Array.count)
                    => "Number of copies to create. 0 still keeps the original.",
                nameof(Array.copies)
                    => "List of copies created with the original as first entry.",
                nameof(Array.start)
                    => "Position at which the -1st copy would be created",
                nameof(Array.end)
                    => "Position at which the count + 1st copy would be created",
                _ => string.Empty,
            };
        }
    }
}
