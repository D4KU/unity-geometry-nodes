using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

namespace GeometryNodes
{
    /// <summary>
    /// Create a common group object for a variable number of child objects
    /// </summary>
    [TypeIcon(typeof(ISelectUnit))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Merge : GeometryUnit
    {
        /// <summary>
        /// Number of input objects to group
        /// </summary>
        [Serialize, Inspectable] public int valueInputCount = 2;

        [DoNotSerialize] public ValueInput[] targets;
        [DoNotSerialize] public ValueOutput parent;

        /// <summary>
        /// Group object created
        /// </summary>
        private Transform parentOut;

        protected override IEnumerable<ValueInput> Required => targets;

        protected override void Definition()
        {
            parent = ValueOutput(nameof(parent), _ => parentOut);

            // Create the specified number of input ports
            targets = new ValueInput[valueInputCount];
            for (int i = 0; i < targets.Length; i++)
                targets[i] = ValueInput<Transform>(i.ToString());

            base.Definition();
            Assignment(input, parent);
        }

        public override void Clear()
        {
            if (parentOut == null)
                return;

            foreach (Transform child in parentOut.ChildrenToRescue())
                child.parent = parentOut.parent;
            parentOut.SafeDestroy();
        }

        protected override void Execute(Flow flow)
        {
            foreach (ValueInput target in targets)
            {
                if (!target.hasValidConnection)
                    continue;

                Transform vtarget = flow.GetValue<Transform>(target);
                if (vtarget == null)
                    continue;

                if (parentOut == null)
                    parentOut = vtarget.Group(nameof(Merge));
                vtarget.parent = parentOut;
            }
        }
    }
}
