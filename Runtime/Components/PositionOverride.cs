using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Flags objects moved through a node
    /// </summary>
    [DisallowMultipleComponent]
    internal class PositionOverride : MonoBehaviour
    {
        [Tooltip("The object's local position before any node touched it")]
        private Vector3 original;

        /// <summary>
        /// The object's local position before any node touched it
        /// </summary>
        public Vector3 Original
        {
            get => original;
            set => original = value;
        }

        /// <summary>
        /// Add and initialize a new override if none exists next to the
        /// given Transform. Otherwise return the existing override.
        /// </summary>
        public static void Add(Transform t)
        {
            if (!t.GetOrAddComponent(out PositionOverride o))
                o.Original = t.localPosition;
        }

        /// <summary>
        /// If an override exists next to the given transform, remove it
        /// and restore the original position.
        /// </summary>
        public static void Remove(Transform t)
        {
            if (t.TryGetComponent(out PositionOverride o))
            {
                t.localPosition = o.Original;
                o.SafeDestroy();
            }
        }
    }
}
