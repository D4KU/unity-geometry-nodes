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
                nameof(Scale.target) => "Object to scale",
                nameof(Scale.x) => Coordinate('x'),
                nameof(Scale.y) => Coordinate('y'),
                nameof(Scale.z) => Coordinate('z'),
                _ => string.Empty,
            };
        }

        private static string Coordinate(char axis) =>
            Descriptions.TransformCoordinate("scale", "value by", axis);
    }
}
