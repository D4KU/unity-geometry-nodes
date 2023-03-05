using Unity.VisualScripting;

namespace GeometryNodes
{
    [TypeIcon(typeof(GraphInput))]
    [UnitCategory("Events")]
    public abstract class GeometryInput<T> : EventUnit<(string, T)>
    {
        [Serialize, Inspectable, UnitHeaderInspectable]
        public string Key { get; set; }

        private ValueOutput result;
        private T outputValue;

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
            => new(HOOK_NAME, reference.gameObject);

        public const string HOOK_NAME = "GeometryNodeInput";
        public static string GetResetCommand(string key) => "RESET" + key;

        protected override void Definition()
        {
            base.Definition();
            result = ValueOutput<T>(nameof(result), _ => outputValue);
        }

        protected override void AssignArguments(Flow flow, (string, T) args)
        {
            base.AssignArguments(flow, args);
            outputValue = args.Item2;
        }

        protected override bool ShouldTrigger(Flow flow, (string, T) args)
        {
            if (args.Item1 == GetResetCommand(Key))
            {
                trigger.RecursiveClear();
                return false;
            }
            return Key == args.Item1;
        }
    }

    public class FloatInput : GeometryInput<float> {}
    public class IntInput : GeometryInput<int> {}
    public class BoolInput : GeometryInput<bool> {}
}
