using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Flags objects whose activate state was changed through a node
    /// </summary>
    [DisallowMultipleComponent]
    internal class ActiveOverride : MonoBehaviour
    {
        [Tooltip("The object's active state before any node touched it")]
        private bool original;

        /// <summary>
        /// The object's active state before any node touched it
        /// </summary>
        public bool Original
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
            if (!t.GetOrAddComponent(out ActiveOverride o))
                o.Original = t.gameObject.activeSelf;
        }

        /// <summary>
        /// If an override exists next to the given transform, remove it
        /// and restore the original active state.
        /// </summary>
        public static void Remove(Transform t)
        {
            if (t.TryGetComponent(out ActiveOverride o))
            {
                t.gameObject.SetActive(o.Original);
                o.SafeDestroy();
            }
        }
    }
}
