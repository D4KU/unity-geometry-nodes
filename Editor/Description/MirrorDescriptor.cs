using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(Mirror))]
    internal class MirrorDescriptor : UnitDescriptor<Mirror>
    {
        public MirrorDescriptor(Mirror unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Mirror.original) => Descriptions.ORIGINAL,
                nameof(Mirror.parent) => Descriptions.PARENT,
                nameof(Mirror.copy) => "Mirrored copy created",
                nameof(Mirror.x) => Coordinate('x'),
                nameof(Mirror.y) => Coordinate('y'),
                nameof(Mirror.z) => Coordinate('z'),
                _ => string.Empty,
            };
        }

        private static string Coordinate(char axis) =>
            $"{axis}-coordinate of the point around which to mirror, " +
            "in local space of Original's parent. If unconnected " +
            $"the object is not mirrored around the {axis}-axis.";
    }
}
