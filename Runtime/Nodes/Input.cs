using Unity.VisualScripting;

namespace GeometryNodes
{
    [TypeIcon(typeof(GraphInput))]
    [UnitCategory(GeometryUnit.CATEGORY)]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public abstract class Input<T> : Unit
    {
        [Serialize, Inspectable, UnitHeaderInspectable]
        public string key;
        public T value;
        public ValueOutput output;

        protected override void Definition()
        {
            output = ValueOutput<T>(nameof(output), _ => value);
        }
    }

    public class FloatInput : Input<float> {}
    public class IntInput : Input<int> {}
    public class BoolInput : Input<bool> {}
}
