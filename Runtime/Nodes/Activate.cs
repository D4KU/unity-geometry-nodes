using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

namespace GeometryNodes
{
    [TypeIcon(typeof(UnityEngine.UI.Toggle))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Activate : GeometryUnit
    {
        [DoNotSerialize] public ValueInput target;
        [DoNotSerialize] public ValueInput active;

        protected Transform targetValue;

        protected override IEnumerable<ValueInput> Required => new[] { target };

        protected override void Definition()
        {
            target = ValueInput<Transform>(nameof(target));
            active = ValueInput<bool>(nameof(active), false);
            base.Definition();
        }

        public override void Clear() => ActiveOverride.Remove(targetValue);

        protected override void Execute(Flow flow)
        {
             targetValue = flow.GetValue<Transform>(target);
             ActiveOverride.Add(targetValue);
             targetValue.gameObject.SetActive(flow.GetValue<bool>(active));
        }
    }
}
