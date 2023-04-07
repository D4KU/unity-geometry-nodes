using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(Scale))]
    internal class ScaleDescriptor : UnitDescriptor<Scale>
    {
        public ScaleDescriptor(Scale unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Position.target) => "Object to scale",
                nameof(Position.x) => Coordinate('x'),
                nameof(Position.y) => Coordinate('y'),
                nameof(Position.z) => Coordinate('z'),
                _ => string.Empty,
            };
        }

        private static string Coordinate(char axis) =>
            Descriptions.TransformCoordinate("scale", "value by", axis);
    }
}
