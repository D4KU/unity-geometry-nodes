using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Creates a given number of copies of a template object, without
    /// creating a common parent for them. This can be used if the template
    /// object resides inside a prefab and its hierarchy can't be changed.
    /// The remaining graph can then operate on the copies.
    /// </summary>
    [TypeIcon(typeof(IBranchUnit))]
    [UnitSubtitle(GeometryUnit.SUBTITLE)]
    public class Duplicate : GeometryUnit
    {
        [DoNotSerialize] public ValueInput original;
        [DoNotSerialize] public ValueInput count;
        [DoNotSerialize] public ValueOutput copies;

        /// <summary>
        /// List of copies created
        /// </summary>
        private readonly List<Transform> copiesOut = new();

        protected override IEnumerable<ValueInput> Required => new[] { original };

        protected override void Definition()
        {
            original = ValueInput<Transform>(nameof(original));
            count = ValueInput(nameof(count), 1);
            copies = ValueOutput<List<Transform>>(nameof(copies), _ => copiesOut);

            base.Definition();
            Assignment(input, copies);
        }

        public override void Clear()
        {
            foreach (Transform child in copiesOut)
                child.SafeDestroy();
            copiesOut.Clear();
        }

        protected override void Execute(Flow flow)
        {
            int vcount = flow.GetValue<int>(count);
            Transform voriginal = flow.GetValue<Transform>(original);

            if (copiesOut.Count > vcount)
            {
                // Destroy surplus copies
                for (int i = copiesOut.Count - 1; i >= vcount; i--)
                    copiesOut[i].SafeDestroy();
                copiesOut.RemoveRange(vcount, copiesOut.Count - vcount);
                return;
            }

            // Create newly wanted copies
            copiesOut.AddCopies(vcount, voriginal, voriginal.parent);
        }
    }
}
