using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Flags objects scaled through a node
    /// </summary>
    [DisallowMultipleComponent]
    internal class ScaleOverride : MonoBehaviour
    {
        [Tooltip("The object's local scale before any node touched it")]
        private Vector3 original;

        /// <summary>
        /// The object's local scale before any node touched it
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
            if (!t.GetOrAddComponent(out ScaleOverride o))
                o.Original = t.localScale;
        }

        /// <summary>
        /// If an override exists next to the given transform, remove it
        /// and restore the original scale.
        /// </summary>
        public static void Remove(Transform t)
        {
            if (t == null)
                return;

            if (t.TryGetComponent(out ScaleOverride o))
            {
                t.localScale = o.Original;
                o.SafeDestroy();
            }
        }
    }
}
