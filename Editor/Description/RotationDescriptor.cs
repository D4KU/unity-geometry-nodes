using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(Rotation))]
    internal class RotationDescriptor : UnitDescriptor<Rotation>
    {
        public RotationDescriptor(Rotation unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Rotation.target) => "Object to rotate",
                nameof(Rotation.x) => Coordinate('x'),
                nameof(Rotation.y) => Coordinate('y'),
                nameof(Rotation.z) => Coordinate('z'),
                _ => string.Empty,
            };
        }

        private static string Coordinate(char axis) =>
            Descriptions.TransformCoordinate("rotate", "angle by", axis);
    }
}
