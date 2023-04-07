using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(LookAt))]
    public class LookAtDescriptor : UnitDescriptor<LookAt>
    {
        public LookAtDescriptor(LookAt unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Position.target) => "Object to rotate",
                nameof(Position.x) => Coordinate('x'),
                nameof(Position.y) => Coordinate('y'),
                nameof(Position.z) => Coordinate('z'),
                nameof(LookAt.roll) => "Angle to apply around the Target's local forward axis",
                _ => string.Empty,
            };
        }

        private static string Coordinate(char axis) =>
            $"{char.ToUpper(axis)}-coordinate of the point which to " +
            $"face with Target's forward axis, in local space of its " +
            $"parent. If unconnected it is not rotated on the {axis}-axis.";
    }
}
