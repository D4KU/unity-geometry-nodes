using Unity.VisualScripting;

namespace GeometryNodes
{
    [Descriptor(typeof(Activate))]
    internal class ActivateDescriptor : UnitDescriptor<Activate>
    {
        public ActivateDescriptor(Activate unit) : base(unit) {}

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);
            description.summary = port.key switch
            {
                nameof(Activate.target) => "Object to set (in-)active",
                nameof(Activate.active) => "New active state of Target",
                _ => string.Empty,
            };
        }
    }
}
