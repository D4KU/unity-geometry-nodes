using Unity.VisualScripting;

namespace GeometryNodes
{
    [TypeIcon(typeof(GraphInput))]
    [UnitCategory("Events")]
    public abstract class Input<T> : EventUnit<(string, T)>
    {
        [Serialize, Inspectable, UnitHeaderInspectable]
        public string Key { get; set; }

        private ValueOutput result;
        private T outputValue;

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
            => new(HOOK_NAME, reference.gameObject);

        public const string HOOK_NAME = "GeometryNodeInput";
        public static string GetClearCommand(string key) => "RESET" + key;
        public static string GetInitCommand(string key) => "INIT" + key;

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
            if (args.Item1 == GetClearCommand(Key))
            {
                trigger.ClearDownstream();
                return false;
            }
            if (args.Item1 == GetInitCommand(Key))
            {
                outputValue = args.Item2;
                return false;
            }
            return Key == args.Item1;
        }
    }

    public class FloatInput : Input<float> {}
    public class IntInput : Input<int> {}
    public class BoolInput : Input<bool> {}
}
