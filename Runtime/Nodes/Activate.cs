using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

namespace GeometryNodes
{
    /// <summary>
    /// Set active state of an object
    /// </summary>
    [TypeIcon(typeof(UnityEngine.UI.Toggle))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Activate : GeometryUnit
    {
        [DoNotSerialize] public ValueInput target;
        [DoNotSerialize] public ValueInput active;

        /// <summary>
        /// Objects overridden since the last reset
        /// </summary>
        private readonly HashSet<Transform> overriden = new();

        protected override IEnumerable<ValueInput> Required => new[] { target };

        protected override void Definition()
        {
            target = ValueInput<Transform>(nameof(target));
            active = ValueInput<bool>(nameof(active), false);
            base.Definition();
        }

        public override void Clear()
        {
            foreach (Transform t in overriden)
                ActiveOverride.Remove(t);
            overriden.Clear();
        }

        protected override void Execute(Flow flow)
        {
             Transform vtarget = flow.GetValue<Transform>(target);
             ActiveOverride.Add(vtarget);
             vtarget.gameObject.SetActive(flow.GetValue<bool>(active));
        }
    }
}
