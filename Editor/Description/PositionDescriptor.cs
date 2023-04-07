using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(Position))]
    internal class PositionDescriptor : UnitDescriptor<Position>
    {
        public PositionDescriptor(Position unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Position.target) => "Object to move",
                nameof(Position.x) => Coordinate('x'),
                nameof(Position.y) => Coordinate('y'),
                nameof(Position.z) => Coordinate('z'),
                _ => string.Empty,
            };
        }

        private static string Coordinate(char axis) =>
            Descriptions.TransformCoordinate("move", "coordinate of the point to", axis);
    }
}
